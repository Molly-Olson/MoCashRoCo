# ==============================================================
# Setup-MoCashRo.ps1
# Run this from your project root:
#   C:\Users\olson\source\repos\Capstone\MoCashRoCo\MoCashRoCo\
#
# What it does:
#   1. Creates Models\ and Data\ folders
#   2. Writes all 7 model files + AppDbContext
#   3. Patches appsettings.json with the connection string
#   4. Patches Program.cs with the DbContext registration
#   5. Installs NuGet packages via dotnet CLI
#   6. Runs Add-Migration + Update-Database
# ==============================================================

$ErrorActionPreference = "Stop"
$projectRoot = Split-Path -Parent $MyInvocation.MyCommand.Path

Write-Host ""
Write-Host "==> MoCashRo Setup Script" -ForegroundColor Cyan
Write-Host "==> Project root: $projectRoot" -ForegroundColor Cyan
Write-Host ""

# ---------------------------------------------------------------
# 1. Create folders
# ---------------------------------------------------------------
$modelsDir = Join-Path $projectRoot "Models"
$dataDir   = Join-Path $projectRoot "Data"

foreach ($dir in @($modelsDir, $dataDir)) {
    if (-not (Test-Path $dir)) {
        New-Item -ItemType Directory -Path $dir | Out-Null
        Write-Host "[+] Created folder: $dir" -ForegroundColor Green
    } else {
        Write-Host "[~] Folder already exists: $dir" -ForegroundColor Yellow
    }
}

# ---------------------------------------------------------------
# 2. Model files
# ---------------------------------------------------------------
Write-Host ""
Write-Host "==> Writing model files..." -ForegroundColor Cyan

# --- Category.cs ---
Set-Content -Path (Join-Path $modelsDir "Category.cs") -Encoding UTF8 -Value @'
using System.ComponentModel.DataAnnotations;

namespace MoCashRoCo.Models
{
    /// <summary>
    /// Represents a product category (e.g., "Lawn Care", "Mulch", "Seasonal").
    /// One Category can have many Products (one-to-many).
    /// </summary>
    public class Category
    {
        public int CategoryId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        // Navigation property — EF Core uses this to JOIN Products to Categories.
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
'@

# --- Product.cs ---
Set-Content -Path (Join-Path $modelsDir "Product.cs") -Encoding UTF8 -Value @'
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoCashRoCo.Models
{
    /// <summary>
    /// Represents a product listing in the storefront and inventory system.
    /// Belongs to one Category; can appear in many OrderItems.
    /// </summary>
    public class Product
    {
        public int ProductId { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [MaxLength(500)]
        public string? ImageUrl { get; set; }

        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }

        [Required]
        public int StockQuantity { get; set; }

        // Soft-delete: hides product from storefront without destroying order history.
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
'@

# --- Customer.cs ---
Set-Content -Path (Join-Path $modelsDir "Customer.cs") -Encoding UTF8 -Value @'
using System.ComponentModel.DataAnnotations;

namespace MoCashRoCo.Models
{
    /// <summary>
    /// Represents a registered customer account.
    /// Separate from Admins — different tables, different auth flows.
    /// </summary>
    public class Customer
    {
        public int CustomerId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
'@

# --- Order.cs ---
Set-Content -Path (Join-Path $modelsDir "Order.cs") -Encoding UTF8 -Value @'
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoCashRoCo.Models
{
    public enum OrderStatus
    {
        Pending = 0,
        Processing = 1,
        Shipped = 2,
        Complete = 3,
        Cancelled = 4
    }

    /// <summary>
    /// Represents a completed purchase. One Order has many OrderItems.
    /// </summary>
    public class Order
    {
        public int OrderId { get; set; }

        // Nullable: guest checkouts won't have a CustomerId.
        public int? CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }

        [Required, MaxLength(200)]
        public string ContactName { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        [EmailAddress]
        public string ContactEmail { get; set; } = string.Empty;

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
'@

# --- OrderItem.cs ---
Set-Content -Path (Join-Path $modelsDir "OrderItem.cs") -Encoding UTF8 -Value @'
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoCashRoCo.Models
{
    /// <summary>
    /// A single line item within an Order — the JOIN between Orders and Products.
    /// UnitPrice is a snapshot of price at purchase time.
    /// </summary>
    public class OrderItem
    {
        public int OrderItemId { get; set; }

        public int OrderId { get; set; }
        public virtual Order? Order { get; set; }

        public int ProductId { get; set; }
        public virtual Product? Product { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        // Calculated on the fly — no DB column created.
        [NotMapped]
        public decimal LineTotal => UnitPrice * Quantity;
    }
}
'@

# --- Admin.cs ---
Set-Content -Path (Join-Path $modelsDir "Admin.cs") -Encoding UTF8 -Value @'
using System.ComponentModel.DataAnnotations;

namespace MoCashRoCo.Models
{
    /// <summary>
    /// Represents an admin user (business owner or staff).
    /// Kept separate from Customer for independent role management.
    /// </summary>
    public class Admin
    {
        public int AdminId { get; set; }

        [Required, MaxLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string Role { get; set; } = "Admin";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
'@

# --- SiteSettings.cs ---
Set-Content -Path (Join-Path $modelsDir "SiteSettings.cs") -Encoding UTF8 -Value @'
using System.ComponentModel.DataAnnotations;

namespace MoCashRoCo.Models
{
    /// <summary>
    /// Tenant-specific config: business name, branding, contact info.
    /// Swap this row and the whole site reflects the new client.
    /// </summary>
    public class SiteSettings
    {
        public int SiteSettingsId { get; set; }

        [Required, MaxLength(200)]
        public string BusinessName { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Tagline { get; set; }

        [MaxLength(500)]
        public string? LogoUrl { get; set; }

        [MaxLength(200)]
        [EmailAddress]
        public string? ContactEmail { get; set; }

        [MaxLength(20)]
        public string? ContactPhone { get; set; }

        [MaxLength(500)]
        public string? Address { get; set; }

        [MaxLength(7)]
        public string PrimaryColor { get; set; } = "#2e7d32";

        [MaxLength(7)]
        public string AccentColor { get; set; } = "#ff6f00";
    }
}
'@

Write-Host "[+] All 7 model files written." -ForegroundColor Green

# ---------------------------------------------------------------
# 3. AppDbContext.cs
# ---------------------------------------------------------------
Set-Content -Path (Join-Path $dataDir "AppDbContext.cs") -Encoding UTF8 -Value @'
using Microsoft.EntityFrameworkCore;
using MoCashRoCo.Models;

namespace MoCashRoCo.Data
{
    /// <summary>
    /// The single gateway between C# and SQL.
    /// Each DbSet<T> maps to a database table.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<SiteSettings> SiteSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cascade delete order items when an order is deleted
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Don't cascade delete order items if a product is deleted
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Guest orders allowed (null CustomerId); set null if customer deleted
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            // Prevent category deletion if products still reference it
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Unique constraints
            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.Email).IsUnique();

            modelBuilder.Entity<Admin>()
                .HasIndex(a => a.Username).IsUnique();

            // Seed a default SiteSettings row so the app doesn't crash on first run
            modelBuilder.Entity<SiteSettings>().HasData(new SiteSettings
            {
                SiteSettingsId = 1,
                BusinessName   = "MoCashRo Web Co.",
                Tagline        = "Quality you can count on.",
                PrimaryColor   = "#2e7d32",
                AccentColor    = "#ff6f00"
            });
        }
    }
}
'@

Write-Host "[+] AppDbContext.cs written." -ForegroundColor Green

# ---------------------------------------------------------------
# 4. Patch appsettings.json
# ---------------------------------------------------------------
Write-Host ""
Write-Host "==> Patching appsettings.json..." -ForegroundColor Cyan

$appSettingsPath = Join-Path $projectRoot "appsettings.json"

if (Test-Path $appSettingsPath) {
    $json = Get-Content $appSettingsPath -Raw | ConvertFrom-Json

    if (-not $json.ConnectionStrings) {
        $json | Add-Member -MemberType NoteProperty -Name "ConnectionStrings" -Value @{
            DefaultConnection = "Server=(localdb)\\mssqllocaldb;Database=MoCashRoDB;Trusted_Connection=True;MultipleActiveResultSets=true"
        }
        $json | ConvertTo-Json -Depth 10 | Set-Content $appSettingsPath -Encoding UTF8
        Write-Host "[+] ConnectionStrings added to appsettings.json." -ForegroundColor Green
    } else {
        Write-Host "[~] ConnectionStrings already exists in appsettings.json — skipping." -ForegroundColor Yellow
    }
} else {
    Write-Host "[!] appsettings.json not found at $appSettingsPath — skipping." -ForegroundColor Red
    Write-Host "    Add this manually inside the root {} of appsettings.json:" -ForegroundColor Red
    Write-Host '    "ConnectionStrings": { "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=MoCashRoDB;Trusted_Connection=True;" }' -ForegroundColor White
}

# ---------------------------------------------------------------
# 5. Patch Program.cs
# ---------------------------------------------------------------
Write-Host ""
Write-Host "==> Patching Program.cs..." -ForegroundColor Cyan

$programPath = Join-Path $projectRoot "Program.cs"

if (Test-Path $programPath) {
    $programContent = Get-Content $programPath -Raw

    $usingLine  = "using MoCashRoCo.Data;"
    $dbLine     = 'builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));'

    $changed = $false

    if ($programContent -notmatch [regex]::Escape($usingLine)) {
        $programContent = $usingLine + "`r`n" + $programContent
        $changed = $true
        Write-Host "[+] Added using MoCashRoCo.Data;" -ForegroundColor Green
    } else {
        Write-Host "[~] using MoCashRoCo.Data already present — skipping." -ForegroundColor Yellow
    }

    if ($programContent -notmatch "AddDbContext") {
        # Insert before builder.Build()
        $programContent = $programContent -replace "(var app = builder\.Build\(\);)", "$dbLine`r`n`r`n`$1"
        $changed = $true
        Write-Host "[+] Added AddDbContext registration to Program.cs." -ForegroundColor Green
    } else {
        Write-Host "[~] AddDbContext already present in Program.cs — skipping." -ForegroundColor Yellow
    }

    if ($changed) {
        Set-Content $programPath -Value $programContent -Encoding UTF8
    }
} else {
    Write-Host "[!] Program.cs not found — skipping. Add the DbContext registration manually." -ForegroundColor Red
}

# ---------------------------------------------------------------
# 6. Install NuGet packages via dotnet CLI
# ---------------------------------------------------------------
Write-Host ""
Write-Host "==> Installing NuGet packages..." -ForegroundColor Cyan

$csprojFiles = Get-ChildItem -Path $projectRoot -Filter "*.csproj" -Recurse | Select-Object -First 1

if ($csprojFiles) {
    $csprojDir = $csprojFiles.DirectoryName
    Write-Host "    Found project: $($csprojFiles.Name)" -ForegroundColor Gray

    dotnet add $csprojFiles.FullName package Microsoft.EntityFrameworkCore.SqlServer
    dotnet add $csprojFiles.FullName package Microsoft.EntityFrameworkCore.Tools
    dotnet add $csprojFiles.FullName package Microsoft.EntityFrameworkCore.Design

    Write-Host "[+] NuGet packages installed." -ForegroundColor Green
} else {
    Write-Host "[!] No .csproj found — skipping package install. Run these manually in Package Manager Console:" -ForegroundColor Red
    Write-Host "    Install-Package Microsoft.EntityFrameworkCore.SqlServer" -ForegroundColor White
    Write-Host "    Install-Package Microsoft.EntityFrameworkCore.Tools" -ForegroundColor White
    Write-Host "    Install-Package Microsoft.EntityFrameworkCore.Design" -ForegroundColor White
}

# ---------------------------------------------------------------
# 7. Run migration
# ---------------------------------------------------------------
Write-Host ""
Write-Host "==> Running EF Core migration..." -ForegroundColor Cyan

try {
    dotnet ef migrations add InitialCreate --project $projectRoot
    dotnet ef database update --project $projectRoot
    Write-Host "[+] Database created successfully!" -ForegroundColor Green
} catch {
    Write-Host "[!] Migration failed. This is usually fine — just run these in Visual Studio's Package Manager Console:" -ForegroundColor Yellow
    Write-Host "    Add-Migration InitialCreate" -ForegroundColor White
    Write-Host "    Update-Database" -ForegroundColor White
}

# ---------------------------------------------------------------
# Done
# ---------------------------------------------------------------
Write-Host ""
Write-Host "================================================" -ForegroundColor Cyan
Write-Host "  Setup complete! Next steps in Visual Studio:" -ForegroundColor Cyan
Write-Host "  1. Reload the solution (File > Reload)" -ForegroundColor White
Write-Host "  2. Build the solution (Ctrl+Shift+B)" -ForegroundColor White
Write-Host "  3. Check SQL Server Object Explorer for your new tables" -ForegroundColor White
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""
