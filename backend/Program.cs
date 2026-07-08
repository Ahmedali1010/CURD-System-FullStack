using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Backend.Data;
using Backend.Entities;
using Backend.DTOs;

var builder = WebApplication.CreateBuilder(args);

// ── Database Configuration ──────────────────────────────────────────────────────
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// ── Authentication & Authorization ────────────────────────────────────────────
var jwtKey = builder.Configuration["Jwt:Key"]!;
var jwtIssuer = builder.Configuration["Jwt:Issuer"]!;
var jwtAudience = builder.Configuration["Jwt:Audience"]!;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

// ── CORS ──────────────────────────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:4173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// ── Database Migration & Seeding ──────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        db.Database.Migrate(); // Ensures DB is created and seeds are applied
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error migrating DB: {ex.Message}");
    }
}

// ── Middleware Pipeline ───────────────────────────────────────────────────────
app.UseCors("FrontendPolicy");
app.UseAuthentication();
app.UseAuthorization();

// ═══════════════════════════════════════════════════════════════════════════════
//  AUTH ENDPOINTS
// ═══════════════════════════════════════════════════════════════════════════════

// POST /api/auth/register — public
app.MapPost("/api/auth/register", async (RegisterDto dto, AppDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
        return Results.BadRequest(new { message = "Username and password are required." });

    if (await db.Users.AnyAsync(u => u.Username == dto.Username))
        return Results.Conflict(new { message = "Username already taken." });

    var userRole = await db.Roles.FirstOrDefaultAsync(r => r.Name == "User");
    if (userRole is null)
        return Results.Problem("Roles are not configured. Please contact an administrator.");

    var user = new User
    {
        Username = dto.Username.Trim(),
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password, workFactor: 12),
        RoleId = userRole.Id
    };

    db.Users.Add(user);
    await db.SaveChangesAsync();
    return Results.Ok(new { message = "Registered successfully." });
});

// POST /api/auth/login — public
app.MapPost("/api/auth/login", async (LoginDto dto, AppDbContext db, IConfiguration config) =>
{
    if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
        return Results.BadRequest(new { message = "Username and password are required." });

    var user = await db.Users
        .Include(u => u.Role)
        .FirstOrDefaultAsync(u => u.Username == dto.Username);

    if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        return Results.Unauthorized();

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var claims = new[]
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new Claim("username", user.Username),
        new Claim("role", user.Role!.Name),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

    var token = new JwtSecurityToken(
        issuer: config["Jwt:Issuer"],
        audience: config["Jwt:Audience"],
        claims: claims,
        expires: DateTime.UtcNow.AddMinutes(60),
        signingCredentials: creds
    );

    return Results.Ok(new
    {
        token = new JwtSecurityTokenHandler().WriteToken(token),
        username = user.Username,
        role = user.Role.Name
    });
});

// ═══════════════════════════════════════════════════════════════════════════════
//  USER & ROLE ENDPOINTS
// ═══════════════════════════════════════════════════════════════════════════════

// GET /api/users — Admin only
app.MapGet("/api/users", async (AppDbContext db) =>
{
    var users = await db.Users
        .Include(u => u.Role)
        .Select(u => new UserDto(u.Id, u.Username, u.Role!.Name))
        .ToListAsync();
    return Results.Ok(users);
})
.RequireAuthorization(policy => policy.RequireRole("Admin"));

// PUT /api/users/{id}/role — Admin only
app.MapPut("/api/users/{id:int}/role", async (int id, UpdateUserRoleDto dto, AppDbContext db) =>
{
    var user = await db.Users.FindAsync(id);
    if (user is null) return Results.NotFound(new { message = $"User {id} not found." });

    var role = await db.Roles.FindAsync(dto.RoleId);
    if (role is null) return Results.BadRequest(new { message = $"Role {dto.RoleId} does not exist." });

    user.RoleId = dto.RoleId;
    await db.SaveChangesAsync();

    return Results.Ok(new { message = "User role updated successfully." });
})
.RequireAuthorization(policy => policy.RequireRole("Admin"));

// ═══════════════════════════════════════════════════════════════════════════════
//  PRODUCT ENDPOINTS
// ═══════════════════════════════════════════════════════════════════════════════

// GET /api/products — authenticated (Admin + User)
app.MapGet("/api/products", async (AppDbContext db) =>
{
    var products = await db.Products
        .OrderByDescending(p => p.CreatedAt)
        .Select(p => new ProductDto(p.Id, p.Name, p.Description, p.Price, p.CreatedAt, p.UpdatedAt))
        .ToListAsync();
    return Results.Ok(products);
})
.RequireAuthorization();

// GET /api/products/{id} — authenticated (Admin + User)
app.MapGet("/api/products/{id:int}", async (int id, AppDbContext db) =>
{
    var p = await db.Products.FindAsync(id);
    return p is null
        ? Results.NotFound(new { message = $"Product {id} not found." })
        : Results.Ok(new ProductDto(p.Id, p.Name, p.Description, p.Price, p.CreatedAt, p.UpdatedAt));
})
.RequireAuthorization();

// POST /api/products — Admin only
app.MapPost("/api/products", async (CreateProductDto dto, AppDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(dto.Name))
        return Results.BadRequest(new { message = "Product name is required." });

    var product = new Product
    {
        Name = dto.Name.Trim(),
        Description = dto.Description?.Trim() ?? string.Empty,
        Price = dto.Price,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    };

    db.Products.Add(product);
    await db.SaveChangesAsync();

    return Results.Created(
        $"/api/products/{product.Id}",
        new ProductDto(product.Id, product.Name, product.Description, product.Price, product.CreatedAt, product.UpdatedAt)
    );
})
.RequireAuthorization(policy => policy.RequireRole("Admin"));

// PUT /api/products/{id} — Admin only
app.MapPut("/api/products/{id:int}", async (int id, UpdateProductDto dto, AppDbContext db) =>
{
    var product = await db.Products.FindAsync(id);
    if (product is null) return Results.NotFound(new { message = $"Product {id} not found." });

    if (string.IsNullOrWhiteSpace(dto.Name))
        return Results.BadRequest(new { message = "Product name is required." });

    product.Name = dto.Name.Trim();
    product.Description = dto.Description?.Trim() ?? string.Empty;
    product.Price = dto.Price;
    product.UpdatedAt = DateTime.UtcNow;

    await db.SaveChangesAsync();
    return Results.Ok(new ProductDto(product.Id, product.Name, product.Description, product.Price, product.CreatedAt, product.UpdatedAt));
})
.RequireAuthorization(policy => policy.RequireRole("Admin"));

// DELETE /api/products/{id} — Admin only
app.MapDelete("/api/products/{id:int}", async (int id, AppDbContext db) =>
{
    var product = await db.Products.FindAsync(id);
    if (product is null) return Results.NotFound(new { message = $"Product {id} not found." });

    db.Products.Remove(product);
    await db.SaveChangesAsync();
    return Results.NoContent();
})
.RequireAuthorization(policy => policy.RequireRole("Admin"));

app.Run();
