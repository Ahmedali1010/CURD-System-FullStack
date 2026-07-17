# AI Skill: .NET 10 & PostgreSQL Backend Standards

## System Context & Tech Stack

- **Framework**: .NET 10 Web API
- **Language**: C# 14 (leveraging modern features like Primary Constructors, Collection Expressions, and Enhanced Pattern Matching)
- **Database**: PostgreSQL
- **ORM**: Entity Framework Core (EF Core) 10 / Dapper (depending on query optimization needs)
- **Architecture**: Clean Architecture / Domain-Driven Design (DDD) with Repository Pattern

---

## Rules and Guidelines

### 1. C# 14 & .NET 10 Coding Standards

- **Implicit Usings & File-Scoped Namespaces**: Always use file-scoped namespaces to reduce indentation.
- **Primary Constructors**: Prefer primary constructors for dependency injection in controllers, services, and repositories.
- **Asynchronous Programming**: Every database, file I/O, or external API call must use `async/await` with `CancellationToken`.
- **Nullable Reference Types**: Ensure nullable reference types are explicitly handled to avoid `NullReferenceException`.

### 2. PostgreSQL & EF Core Best Practices

- **PostgreSQL JSONB**: When working with semi-structured data, map properties to PostgreSQL `jsonb` column types.
- **Data Seeding**: Always handle EF Core migrations and seeding programmatically using PostgreSQL-compliant syntax.
- **No-Tracking Queries**: Use `.AsNoTracking()` for read-only queries to optimize performance.
- **Pagination**: Implement cursor-based pagination for large datasets instead of offset-based pagination.

---

## Critical: What NOT to Do (Anti-Patterns)

- **NO SQL Injection**: Never concatenate strings to build SQL queries. Use EF Core parameterized queries or Dapper parameter binding.
- **NO Synchronous Blocking**: Never use `.Result` or `.Wait()` on asynchronous tasks. This blocks threads and causes thread pool starvation.
- **NO Hardcoded Credentials**: Never write connection strings or API keys in code. Always load them from `IConfiguration` or Environment Variables.
- **NO Unrestricted Queries**: Do not allow queries that fetch all records from a table without explicit pagination (`Take()` limit).
- **NO DB Leaks**: Do not expose Database Entity models directly to the presentation/API layer. Always map entities to DTOs (Data Transfer Objects).
