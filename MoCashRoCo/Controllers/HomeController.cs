using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoCashRoCo.Data;
using MoCashRoCo.Models;

namespace MoCashRoCo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _db;

        public HomeController(ILogger<HomeController> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var featured = await _db.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive)
                .Take(4)
                .ToListAsync();
            var categories = await _db.Categories.ToListAsync();
            ViewBag.Featured = featured;
            ViewBag.Categories = categories;
            return View();
        }

        public IActionResult About() => View();

        public IActionResult Contact() => View();

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
