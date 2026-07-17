using Microsoft.EntityFrameworkCore;
using Backend.Entities;
using Backend.Data;

namespace Backend.Repositories;

/// <summary>
/// Repository implementation for User entity.
/// Implements Repository Pattern with EF Core, following Clean Architecture principles.
/// </summary>
/// <param name="context">The DbContext instance for data access.</param>
public class UserRepository(AppDbContext context) : IUserRepository
{
    /// <inheritdoc />
    public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await context.Users
            .AsNoTracking()
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == id && u.IsActive, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(username);

        return await context.Users
            .AsNoTracking()
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Username == username && u.IsActive, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(email);

        return await context.Users
            .AsNoTracking()
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == email && u.IsActive, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<User>> GetAllAsync(int pageSize = 50, int? cursor = null, CancellationToken cancellationToken = default)
    {
        // Cursor-based pagination (keyset pagination) for better performance
        var query = context.Users
            .AsNoTracking()
            .Include(u => u.Role)
            .Where(u => u.IsActive);

        if (cursor.HasValue)
        {
            query = query.Where(u => u.Id > cursor.Value);
        }

        return await query
            .OrderBy(u => u.Id)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<User>> GetByRoleAsync(int roleId, int pageSize = 50, int? cursor = null, CancellationToken cancellationToken = default)
    {
        var query = context.Users
            .AsNoTracking()
            .Include(u => u.Role)
            .Where(u => u.RoleId == roleId && u.IsActive);

        if (cursor.HasValue)
        {
            query = query.Where(u => u.Id > cursor.Value);
        }

        return await query
            .OrderBy(u => u.Id)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<User> CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(user);

        user.CreatedAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;

        context.Users.Add(user);
        await context.SaveChangesAsync(cancellationToken);

        return user;
    }

    /// <inheritdoc />
    public async Task<User> UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(user);

        user.UpdatedAt = DateTime.UtcNow;

        context.Users.Update(user);
        await context.SaveChangesAsync(cancellationToken);

        return user;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var user = await context.Users.FindAsync(new object[] { id }, cancellationToken);

        if (user is null)
            return false;

        user.IsActive = false;
        user.UpdatedAt = DateTime.UtcNow;

        context.Users.Update(user);
        await context.SaveChangesAsync(cancellationToken);

        return true;
    }

    /// <inheritdoc />
    public async Task<bool> PermanentDeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var user = await context.Users.FindAsync(new object[] { id }, cancellationToken);

        if (user is null)
            return false;

        context.Users.Remove(user);
        await context.SaveChangesAsync(cancellationToken);

        return true;
    }

    /// <inheritdoc />
    public async Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(username);

        return await context.Users
            .AsNoTracking()
            .AnyAsync(u => u.Username == username && u.IsActive, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(email);

        return await context.Users
            .AsNoTracking()
            .AnyAsync(u => u.Email == email && u.IsActive, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> UpdateLastLoginAsync(int userId, CancellationToken cancellationToken = default)
    {
        var user = await context.Users.FindAsync(new object[] { userId }, cancellationToken);

        if (user is null)
            return false;

        user.LastLoginAt = DateTime.UtcNow;
        await context.SaveChangesAsync(cancellationToken);

        return true;
    }

    /// <inheritdoc />
    public async Task<int> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return await context.Users
            .AsNoTracking()
            .Where(u => u.IsActive)
            .CountAsync(cancellationToken);
    }
}
