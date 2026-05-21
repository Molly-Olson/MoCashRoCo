# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

MoCashRo Web Co. is an ASP.NET Core 9 MVC e-commerce template built as a capstone project. The goal is a reusable, marketable storefront + POS system that can be deployed and resold to small business clients. The SRS documents live in the repo root (`MoCashRo Web Co._SRS_v0.3.docx` is the current version).

## Common Commands

```bash
# Build
dotnet build

# Run (dev server with hot reload)
dotnet watch run

# Add a new EF Core migration (run after changing any model or AppDbContext)
dotnet ef migrations add <MigrationName>

# Apply pending migrations to the local database
dotnet ef database update

# Remove the last migration (if not yet applied)
dotnet ef migrations remove
```

No test project exists yet.

## Architecture

**Stack:** ASP.NET Core 9 MVC · EF Core 9 · SQL Server (localdb in dev) · Bootstrap 5 · Bootstrap Icons CDN · jQuery

**Request flow:** Browser → Controller → EF Core (`AppDbContext`) → SQL Server → Razor View → Browser. No API layer; all logic lives in controllers.

**Database:** `MoCashRoDB` on `(localdb)\mssqllocaldb`. Connection string is in `appsettings.json`. The migration `InitialCreate` has been applied and seeds Categories (5), Products (5), and SiteSettings (1) via `HasData` in `AppDbContext.OnModelCreating`.

**Session (cart):** `CartController` stores the cart as a JSON string in ASP.NET session under the key `"Cart"`, and the item count as a string under `"CartItemCount"`. The count is read directly in `_Layout.cshtml` via `ViewContext.HttpContext.Session.GetString("CartItemCount")` to drive the navbar badge. Session is configured in `Program.cs` with a 30-minute idle timeout.

**Theme:** CSS custom properties in `wwwroot/css/site.css` drive all brand colors:
- `--brand` / `--brand-dark` / `--brand-light` — primary color (currently Rebecca Purple `#663399`)
- `--accent` / `--accent-light` — secondary color (currently light yellow `#fde047`)

Utility classes `.bg-brand`, `.btn-brand`, `.btn-accent`, `.text-brand`, `.bg-brand-soft`, etc. are defined in `site.css` and used throughout views. To retheme, only the `:root` variables need to change.

**Image uploads:** `AdminController` injects `IWebHostEnvironment` and has a `SaveProductImageAsync` helper that saves uploaded files to `wwwroot/images/products/{guid}{ext}` and returns the relative URL `/images/products/...`. Allowed extensions: `.jpg`, `.jpeg`, `.png`, `.gif`, `.webp`. The `ProductFormViewModel` has both `ImageUrl` (string, fallback) and `ImageFile` (IFormFile, takes priority). Both product forms use `enctype="multipart/form-data"`.

**Controllers and their responsibilities:**
- `HomeController` — injects `AppDbContext`; loads featured products + categories for the landing page; also serves About, Contact, Privacy
- `ProductsController` — product listing with category filter, keyword search, and sort; product detail with related products
- `CartController` — all cart operations (add, remove, update quantity, clear); reads referer header to redirect back after add
- `AdminController` — injects `AppDbContext` + `IWebHostEnvironment`; dashboard stats, product CRUD with image upload, order list + status update; all actions guarded by `IsAdmin()` session check

**Key models:** `Product` (has `CategoryId` FK, `ImageUrl`, `IsActive`, `StockQuantity`), `Category`, `Order`/`OrderItem` (order pipeline), `Customer`, `Admin`, `SiteSettings` (singleton branding row). `CartItem` is a session-only view model, not a DB entity.

**Views layout:** All views inherit `_Layout.cshtml` via `_ViewStart.cshtml`. The layout is fully custom (not the default ASP.NET scaffold) and includes the sticky navbar, Bootstrap Icons CDN link, and a multi-column footer.

## Things to Know

- `btn-accent` uses dark text (`color: #1a1a1a`) because the accent is a light yellow — don't change it to `color: #fff`.
- Seeded products use `picsum.photos` URLs. Admins can replace them via the Edit Product form with a real upload or URL.
- The admin portal, checkout, and auth are all fully implemented — ignore any SRS notes saying they're "planned".
- `SiteSettings` is designed to hold per-client branding (business name, colors, logo, contact info). Currently seeded with one row; the intent is that a future admin UI edits this row to rebrand a deployment.
- EF Core `HasData` seeding uses a fixed `DateTime` (`new DateTime(2025, 1, 1, ..., DateTimeKind.Utc)`) — never use `DateTime.UtcNow` in `HasData` as it breaks migration snapshots.
- For production deployment: Azure App Service (Free F1) + Azure SQL Database is the recommended path — no provider changes needed. Set the connection string via `ConnectionStrings__DefaultConnection` environment variable.
