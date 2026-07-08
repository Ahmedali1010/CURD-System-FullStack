# Project Overview

S-System is a full-stack CRUD application for managing product records with role-differentiated access control. The backend is implemented as a .NET 10 Minimal API using Entity Framework Core against a PostgreSQL database hosted on Supabase. The frontend is a React 19 single-page application written in TypeScript, communicating with the backend over a stateless, token-authenticated REST API.

# Architecture

The system follows a layered architecture with explicit separation of concerns across both the backend and frontend.

# Backend


Presentation layer — HTTP endpoints defined in Program.cs, grouped by domain (authentication, user/role management, products)
Contract layer — Data Transfer Objects (DTOs) define the request/response shape exposed to clients, decoupled from persistence models
Domain layer — Entities (User, Role, Product) represent the core data model and their relationships
Data access layer — AppDbContext (EF Core) manages entity mapping, relationships, and schema migrations against PostgreSQL


# Frontend


Presentation layer — Page components (LoginPage, DashboardPage) and reusable UI components
State layer — React Context providers (AuthContext for session state, I18nContext for localization) expose shared application state without prop drilling
Service layer — A centralized Axios client encapsulates all HTTP communication, including token attachment and response error handling


This separation keeps persistence, business rules, and presentation independently modifiable and testable.

# Key Technologies

Backend


.NET 10 / ASP.NET Core Minimal API
Entity Framework Core 10
Npgsql (PostgreSQL provider)
PostgreSQL (hosted on Supabase)
BCrypt.Net-Next (password hashing)
JWT Bearer Authentication


Frontend


React 19
TypeScript
Vite
React Router 7
Axios
jwt-decode


# Features


JWT-based authentication with stateless, bearer-token session handling
Role-based endpoint authorization distinguishing Admin and User access levels
Full CRUD operations for product records
Administrative user management, including role assignment
Stateless RESTful API design — no server-side session state; each request is authenticated independently via its bearer token
Bilingual UI (English / Kurdish Sorani) with right-to-left layout support
