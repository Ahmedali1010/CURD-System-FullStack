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
---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js 20+](https://nodejs.org/)
- A PostgreSQL instance (local or [Supabase](https://supabase.com/))

---

### 1 — Backend Setup

```bash
cd backend
```

Open `appsettings.json` and configure your connection string and JWT secret:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=<host>;Port=5432;Database=<db>;Username=<user>;Password=<password>;SslMode=Require;"
  },
  "Jwt": {
    "Key": "<your-secret-key-at-least-32-chars>",
    "Issuer": "SSystemAPI",
    "Audience": "SSystemClient"
  }
}
```

Run the API (the app migrates and seeds the database automatically on startup):

```bash
dotnet run
```

The API will be available at `http://localhost:5000` (or whichever port is configured in `launchSettings.json`).

---

### 2 — Frontend Setup

```bash
cd frontend
npm install
```

Create a `.env` file (or edit the existing one):

```
VITE_API_URL=http://localhost:5000
```

Start the dev server:

```bash
npm run dev
```

The app will be available at `http://localhost:5173`.

---

## Default Credentials

Two accounts are seeded automatically on the first run:

| Username | Password | Role |
|---|---|---|
| `admin` | `Admin@123` | Admin |
| `user` | `User@123` | User |

> **Note:** Change these credentials before deploying to production.

---

## API Endpoints

### Auth

| Method | Path | Access | Description |
|---|---|---|---|
| `POST` | `/api/auth/register` | Public | Create a new `User` account |
| `POST` | `/api/auth/login` | Public | Returns a JWT token |

### Products

| Method | Path | Access | Description |
|---|---|---|---|
| `GET` | `/api/products` | Authenticated | List all products |
| `GET` | `/api/products/{id}` | Authenticated | Get a single product |
| `POST` | `/api/products` | Admin only | Create a product |
| `PUT` | `/api/products/{id}` | Admin only | Update a product |
| `DELETE` | `/api/products/{id}` | Admin only | Delete a product |

### Users

| Method | Path | Access | Description |
|---|---|---|---|
| `GET` | `/api/users` | Admin only | List all users with roles |
| `PUT` | `/api/users/{id}/role` | Admin only | Change a user's role |

---

## Internationalization

The UI supports two languages switchable at runtime:

| Code | Language | Direction |
|---|---|---|
| `en` | English | LTR |
| `ckb` | Kurdish Sorani (سۆرانی) | RTL |

The selected language is persisted to `localStorage`. The `<html>` element's `dir` and `lang` attributes update automatically.

---

## License

This project is for educational and portfolio purposes. Feel free to fork and adapt it.

