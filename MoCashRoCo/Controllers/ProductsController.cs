using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoCashRoCo.Data;

namespace MoCashRoCo.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _db;

        public ProductsController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(int? category, string? search, string? sort)
        {
            var categories = await _db.Categories.ToListAsync();
            ViewBag.Categories = categories;
            ViewBag.SelectedCategory = category;
            ViewBag.Search = search;
            ViewBag.Sort = sort;

            var query = _db.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive);

            if (category.HasValue)
                query = query.Where(p => p.CategoryId == category.Value);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(p => p.Name.Contains(search) ||
                    (p.Description != null && p.Description.Contains(search)));

            var products = sort switch
            {
                "price-asc" => await query.OrderBy(p => p.Price).ToListAsync(),
                "price-desc" => await query.OrderByDescending(p => p.Price).ToListAsync(),
                _ => await query.OrderBy(p => p.Name).ToListAsync()
            };

            return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _db.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == id && p.IsActive);

            if (product == null) return NotFound();

            var related = await _db.Products
                .Where(p => p.CategoryId == product.CategoryId && p.ProductId != id && p.IsActive)
                .Take(4)
                .ToListAsync();

            ViewBag.Related = related;
            return View(product);
        }
    }
}
