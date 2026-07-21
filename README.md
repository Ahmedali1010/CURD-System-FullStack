# CURD-System-FullStack

A full-stack product-management web application built with **.NET 10 Minimal API**, **React 19 + TypeScript**, and **PostgreSQL**. It features JWT-based authentication, role-based access control, bilingual support (English & Kurdish Sorani), and a clean, responsive UI.

---

## Features

- **Authentication** — Register and login with BCrypt-hashed passwords; JWT tokens (60-minute expiry)
- **Role-based access control** — Two roles: `Admin` and `User`; Admin-only endpoints and UI elements are protected at both the API and component level
- **Product CRUD** — Admins can create, edit, and delete products; all authenticated users can browse the product list
- **User management** — Admins can view all users and update their roles
- **Bilingual UI** — Full English and Central Kurdish (Sorani / RTL) interface switchable at runtime
- **Auto-migration** — Database is migrated and seeded automatically on first run
- **Toast notifications** — Inline feedback for every create / update / delete action

---

## Tech Stack

| Layer | Technology |
|---|---|
| Backend | .NET 10 Minimal API |
| ORM | Entity Framework Core 10 + Npgsql |
| Auth | JWT Bearer (`Microsoft.AspNetCore.Authentication.JwtBearer`) |
| Password hashing | BCrypt.Net-Next |
| Frontend | React 19, TypeScript, Vite 8 |
| Routing | React Router v7 |
| HTTP client | Axios |
| Database | PostgreSQL (Supabase) |
| i18n | Custom context — English + Kurdish Sorani (RTL) |

---
## Project Structure

```
S-System/
├── backend/
│   ├── Data/
│   │   └── AppDbContext.cs       # EF Core DbContext + seeding
│   ├── DTOs/
│   │   ├── AuthDtos.cs           # RegisterDto, LoginDto
│   │   ├── ProductDtos.cs        # ProductDto, CreateProductDto, UpdateProductDto
│   │   └── UserDtos.cs           # UserDto, UpdateUserRoleDto
│   ├── Entities/
│   │   ├── Product.cs
│   │   ├── Role.cs
│   │   └── User.cs
│   ├── Migrations/
│   ├── Program.cs                # All endpoints, middleware, DI
│   ├── appsettings.json
│   └── backend.csproj
└── frontend/
    └── src/
        ├── api/
        │   ├── axios.ts          # Axios instance with auth interceptor
        │   └── products.ts       # productApi helper
        ├── components/
        │   ├── ConfirmDialog.tsx
        │   ├── Navbar.tsx
        │   ├── ProductModal.tsx
        │   ├── ProtectedRoute.tsx
        │   └── Toast.tsx
        ├── contexts/
        │   ├── AuthContext.tsx    # JWT parsing, login/logout
        │   └── I18nContext.tsx    # Language switching, RTL
        ├── locales/
        │   ├── en.ts
        │   └── ckb.ts            # Kurdish Sorani translations
        ├── pages/
        │   ├── LoginPage.tsx
        │   └── DashboardPage.tsx
        └── App.tsx
```
