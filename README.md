# MoCashRo Web Co.

> A full-stack e-commerce platform built as an Application Development capstone — designed as a reusable, deployable template for small business clients.

**Stack:** ASP.NET Core 9 MVC · EF Core 9 · SQL Server · Bootstrap 5 · BCrypt

---

## What's Built

### Customer Storefront
- Product catalog with keyword search, category filter, and sort
- Product detail pages with related products
- Session-based shopping cart (add, remove, update quantity)
- Full checkout flow — contact info, shipping, simulated payment
- Orders persisted to database; inventory decremented on checkout
- Order confirmation page with order number

### Authentication
- Customer registration and login (BCrypt-hashed passwords)
- Admin login at `/Account/AdminLogin`
- Session-based role tracking (`UserRole` = `"Admin"` or `"Customer"`)
- Default admin seeded on first run: **username** `admin` / **password** `Admin1234!`

### Admin Portal (`/Admin/Dashboard`)
- Dashboard with live stats: total products, orders, revenue, pending orders, customers, low-stock alerts, recent order feed
- **Product management** — create, edit, delete (soft-deletes products with order history), search + category filter
- **Order management** — list all orders with status filter, update order status (Pending → Processing → Shipped → Delivered → Cancelled)
- **Image uploads** — admins can upload product images (JPG/PNG/GIF/WebP saved to `wwwroot/images/products/`) or paste a URL

### Branding
- CSS custom properties in `wwwroot/css/site.css` control all brand colors
- `SiteSettings` table holds per-client business name, colors, and contact info — change the DB row to rebrand a deployment

---

## Getting Started

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- SQL Server (LocalDB is fine — ships with Visual Studio)
- Visual Studio 2022 or VS Code with the C# extension

### Run locally

```bash
# 1. Clone the repo
git clone https://github.com/Molly-Olson/MoCashRoCo.git
cd MoCashRoCo

# 2. Apply migrations (creates MoCashRoDB on LocalDB + seeds data)
cd MoCashRoCo
dotnet ef database update

# 3. Start the dev server
dotnet watch run
```

Open `https://localhost:5001` (or whichever port `dotnet watch` reports).

**Default admin login:** `admin` / `Admin1234!`

### Common commands

```bash
# Build
dotnet build

# Run with hot reload
dotnet watch run

# Add a new migration after changing a model
dotnet ef migrations add <MigrationName>

# Apply pending migrations
dotnet ef database update

# Remove last (unapplied) migration
dotnet ef migrations remove
```

---

## Project Structure

```
MoCashRoCo/
├── Controllers/
│   ├── HomeController.cs        # Landing page, About, Contact, Privacy
│   ├── ProductsController.cs    # Catalog, search/filter, detail
│   ├── CartController.cs        # Add, remove, update, clear
│   ├── CheckoutController.cs    # Order form, confirmation
│   ├── AccountController.cs     # Customer + admin login/register
│   └── AdminController.cs       # Dashboard, product CRUD, order mgmt
├── Models/                      # EF Core entities: Product, Category, Order, Customer, Admin, SiteSettings
├── ViewModels/                  # View-specific models (cart, checkout, admin forms)
├── Views/
│   ├── Admin/                   # Dashboard, Products, CreateProduct, EditProduct, Orders
│   ├── Account/                 # Login, Register, AdminLogin
│   ├── Checkout/                # OrderForm, Confirmation
│   └── ...
├── Data/
│   └── AppDbContext.cs          # EF Core context + HasData seeding
├── Migrations/                  # EF Core migration history
└── wwwroot/
    ├── css/site.css             # Brand color variables + utility classes
    ├── images/products/         # Uploaded product images (created on first upload)
    └── ...
```

---

## Database

**Local dev:** `MoCashRoDB` on `(localdb)\mssqllocaldb` (connection string in `appsettings.json`)

Seeded data (via `HasData` in `AppDbContext`):
- 5 Categories
- 5 Products
- 1 SiteSettings row

The admin account is seeded at startup (not via migration) so the BCrypt hash is computed fresh each time on a clean DB.

---

## Deployment

The app targets .NET 9 and uses SQL Server — any host that supports both works.

**Recommended options:**
- **Azure App Service** (Free F1 tier) + **Azure SQL Database** — native .NET/SQL Server stack, no provider changes needed
- **Windows hosting** (SmarterASP.NET, GoDaddy Windows hosting) — deploy the published output, point connection string at a remote SQL Server

**To publish a release build:**
```bash
dotnet publish -c Release -o ./publish
```
Then deploy the `./publish` folder to your host.

**Environment variable for production connection string:**
```
ConnectionStrings__DefaultConnection=Server=YOUR_SERVER;Database=MoCashRoDB;User Id=...;Password=...;
```

---

## About

**Built by:** Molly Olson  
**Course:** Application Development Capstone  
**Instructor:** Professor Jesse Harlan  
**SRS:** See `MoCashRo Web Co._SRS_v0.3.docx` in the repo root

---

*MoCashRo Web Co. — misspelled on purpose. Built with intention.*
