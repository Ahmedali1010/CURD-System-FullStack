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
