using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoCashRoCo.Data;
using MoCashRoCo.Models;
using MoCashRoCo.ViewModels;

namespace MoCashRoCo.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        private const string SessionUserRole = "UserRole";

        public AdminController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        // ── Auth guard ─────────────────────────────────────────────────
        private bool IsAdmin() =>
            HttpContext.Session.GetString(SessionUserRole) == "Admin";

        private IActionResult RequireAdmin()
        {
            TempData["Error"] = "Admin access required.";
            return RedirectToAction("AdminLogin", "Account",
                new { returnUrl = Request.Path });
        }

        // ── Dashboard ──────────────────────────────────────────────────
        public async Task<IActionResult> Dashboard()
        {
            if (!IsAdmin()) return RequireAdmin();

            var vm = new AdminDashboardViewModel
            {
                TotalProducts   = await _db.Products.CountAsync(p => p.IsActive),
                TotalOrders     = await _db.Orders.CountAsync(),
                TotalRevenue    = await _db.Orders.SumAsync(o => (decimal?)o.TotalAmount) ?? 0,
                PendingOrders   = await _db.Orders.CountAsync(o => o.Status == OrderStatus.Pending),
                TotalCustomers  = await _db.Customers.CountAsync(),
                LowStockProducts = await _db.Products
                    .Where(p => p.IsActive && p.StockQuantity < 10)
                    .CountAsync(),
                RecentOrders = await _db.Orders
                    .Include(o => o.Customer)
                    .OrderByDescending(o => o.OrderDate)
                    .Take(8)
                    .ToListAsync()
            };

            return View(vm);
        }

        // ── Products — list ────────────────────────────────────────────
        public async Task<IActionResult> Products(string? search, int? categoryId)
        {
            if (!IsAdmin()) return RequireAdmin();

            var query = _db.Products.Include(p => p.Category).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(p => p.Name.Contains(search));

            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId.Value);

            ViewBag.Search     = search;
            ViewBag.CategoryId = categoryId;
            ViewBag.Categories = await _db.Categories.OrderBy(c => c.Name).ToListAsync();

            return View(await query.OrderBy(p => p.Name).ToListAsync());
        }

        // ── Products — create ──────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
            if (!IsAdmin()) return RequireAdmin();
            await LoadCategorySelectList();
            return View(new ProductFormViewModel());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(ProductFormViewModel model)
        {
            if (!IsAdmin()) return RequireAdmin();

            if (!ModelState.IsValid)
            {
                await LoadCategorySelectList();
                return View(model);
            }

            var imageUrl = model.ImageUrl;
            if (model.ImageFile is { Length: > 0 })
            {
                var uploaded = await SaveProductImageAsync(model.ImageFile);
                if (uploaded != null) imageUrl = uploaded;
            }

            var product = new Product
            {
                Name          = model.Name,
                Description   = model.Description,
                Price         = model.Price,
                ImageUrl      = imageUrl,
                CategoryId    = model.CategoryId,
                StockQuantity = model.StockQuantity,
                IsActive      = model.IsActive,
                CreatedAt     = DateTime.UtcNow
            };

            _db.Products.Add(product);
            await _db.SaveChangesAsync();

            TempData["Success"] = $"Product \"{product.Name}\" created.";
            return RedirectToAction(nameof(Products));
        }

        // ── Products — edit ────────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> EditProduct(int id)
        {
            if (!IsAdmin()) return RequireAdmin();

            var product = await _db.Products.FindAsync(id);
            if (product == null) return NotFound();

            await LoadCategorySelectList(product.CategoryId);
            return View(new ProductFormViewModel
            {
                ProductId     = product.ProductId,
                Name          = product.Name,
                Description   = product.Description,
                Price         = product.Price,
                ImageUrl      = product.ImageUrl,
                CategoryId    = product.CategoryId,
                StockQuantity = product.StockQuantity,
                IsActive      = product.IsActive
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(int id, ProductFormViewModel model)
        {
            if (!IsAdmin()) return RequireAdmin();

            var product = await _db.Products.FindAsync(id);
            if (product == null) return NotFound();

            if (!ModelState.IsValid)
            {
                await LoadCategorySelectList(model.CategoryId);
                return View(model);
            }

            product.Name          = model.Name;
            product.Description   = model.Description;
            product.Price         = model.Price;
            product.CategoryId    = model.CategoryId;
            product.StockQuantity = model.StockQuantity;
            product.IsActive      = model.IsActive;

            if (model.ImageFile is { Length: > 0 })
            {
                var uploaded = await SaveProductImageAsync(model.ImageFile);
                if (uploaded != null) product.ImageUrl = uploaded;
            }
            else if (!string.IsNullOrWhiteSpace(model.ImageUrl))
            {
                product.ImageUrl = model.ImageUrl;
            }

            await _db.SaveChangesAsync();
            TempData["Success"] = $"Product \"{product.Name}\" updated.";
            return RedirectToAction(nameof(Products));
        }

        // ── Products — delete ──────────────────────────────────────────
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (!IsAdmin()) return RequireAdmin();

            var product = await _db.Products.FindAsync(id);
            if (product == null) return NotFound();

            var hasOrders = await _db.OrderItems.AnyAsync(oi => oi.ProductId == id);
            if (hasOrders)
            {
                product.IsActive = false;
                await _db.SaveChangesAsync();
                TempData["Info"] = $"\"{product.Name}\" has order history — it was deactivated instead of deleted.";
            }
            else
            {
                _db.Products.Remove(product);
                await _db.SaveChangesAsync();
                TempData["Success"] = $"Product \"{product.Name}\" deleted.";
            }

            return RedirectToAction(nameof(Products));
        }

        // ── Orders — list ──────────────────────────────────────────────
        public async Task<IActionResult> Orders(string? status)
        {
            if (!IsAdmin()) return RequireAdmin();

            var query = _db.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .AsQueryable();

            if (Enum.TryParse<OrderStatus>(status, out var parsedStatus))
                query = query.Where(o => o.Status == parsedStatus);

            ViewBag.CurrentStatus = status;

            return View(await query.OrderByDescending(o => o.OrderDate).ToListAsync());
        }

        // ── Orders — update status ─────────────────────────────────────
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOrderStatus(int id, OrderStatus status)
        {
            if (!IsAdmin()) return RequireAdmin();

            var order = await _db.Orders.FindAsync(id);
            if (order == null) return NotFound();

            order.Status = status;
            await _db.SaveChangesAsync();

            TempData["Success"] = $"Order #{id} updated to {status}.";
            return RedirectToAction(nameof(Orders));
        }

        // ── Helpers ────────────────────────────────────────────────────
        private async Task LoadCategorySelectList(int? selectedId = null)
        {
            var categories = await _db.Categories.OrderBy(c => c.Name).ToListAsync();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "Name", selectedId);
        }

        private static readonly string[] AllowedImageExtensions = [".jpg", ".jpeg", ".png", ".gif", ".webp"];

        private async Task<string?> SaveProductImageAsync(IFormFile file)
        {
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedImageExtensions.Contains(ext)) return null;

            var folder = Path.Combine(_env.WebRootPath, "images", "products");
            Directory.CreateDirectory(folder);

            var fileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(folder, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/images/products/{fileName}";
        }
    }
}
