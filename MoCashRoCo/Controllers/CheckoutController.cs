using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoCashRoCo.Data;
using MoCashRoCo.Models;
using System.Text.Json;

namespace MoCashRoCo.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly AppDbContext _db;
        private const string CartKey = "Cart";
        private const string CartCountKey = "CartItemCount";

        public CheckoutController(AppDbContext db) => _db = db;

        // ── GET /Checkout ─────────────────────────────────────────────
        [HttpGet]
        public IActionResult Index()
        {
            var cart = GetCart();
            if (!cart.Any())
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("Index", "Cart");
            }

            var model = new CheckoutViewModel { CartItems = cart };

            // Pre-fill from session if customer is logged in
            var userName = HttpContext.Session.GetString("UserName");
            var userId   = HttpContext.Session.GetString("UserId");
            if (userName != null) model.Name = userName;

            return View(model);
        }

        // ── POST /Checkout ────────────────────────────────────────────
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(CheckoutViewModel model)
        {
            var cart = GetCart();
            model.CartItems = cart;

            if (!cart.Any())
                return RedirectToAction("Index", "Cart");

            if (!ModelState.IsValid)
                return View(model);

            // Check stock for all items before doing anything
            foreach (var item in cart)
            {
                var product = await _db.Products.FindAsync(item.ProductId);
                if (product == null || !product.IsActive)
                {
                    ModelState.AddModelError(string.Empty, $"'{item.Name}' is no longer available.");
                    return View(model);
                }
                if (product.StockQuantity < item.Quantity)
                {
                    ModelState.AddModelError(string.Empty,
                        $"Only {product.StockQuantity} of '{item.Name}' left in stock.");
                    return View(model);
                }
            }

            // Resolve or create customer
            var sessionUserId = HttpContext.Session.GetString("UserId");
            var sessionRole   = HttpContext.Session.GetString("UserRole");
            int? customerId   = null;

            if (sessionRole == "Customer" && int.TryParse(sessionUserId, out var sid))
            {
                customerId = sid;
            }
            else
            {
                // Guest checkout — look up or create a lightweight customer record
                var existing = await _db.Customers.FirstOrDefaultAsync(c => c.Email == model.Email);
                if (existing != null)
                {
                    customerId = existing.CustomerId;
                }
                else
                {
                    var guest = new Customer
                    {
                        Name         = model.Name,
                        Email        = model.Email,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString()),
                        RegisteredAt = DateTime.UtcNow
                    };
                    _db.Customers.Add(guest);
                    await _db.SaveChangesAsync();
                    customerId = guest.CustomerId;
                }
            }

            // Build order
            var shipping = model.Subtotal >= 50 ? 0m : 7.99m;
            var order = new Order
            {
                CustomerId      = customerId,
                ContactName     = model.Name,
                ContactEmail    = model.Email,
                OrderDate       = DateTime.UtcNow,
                Status          = OrderStatus.Pending,
                TotalAmount     = model.Total,
                ShippingAddress = $"{model.Address}, {model.City}, {model.State} {model.Zip}",
                Notes           = model.Notes
            };
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            // Add order items + decrement stock
            foreach (var item in cart)
            {
                _db.OrderItems.Add(new OrderItem
                {
                    OrderId   = order.OrderId,
                    ProductId = item.ProductId,
                    Quantity  = item.Quantity,
                    UnitPrice = item.Price
                });

                var product = await _db.Products.FindAsync(item.ProductId);
                if (product != null)
                    product.StockQuantity -= item.Quantity;
            }

            await _db.SaveChangesAsync();

            // Clear cart
            HttpContext.Session.Remove(CartKey);
            HttpContext.Session.Remove(CartCountKey);

            return RedirectToAction("Confirmation", new { id = order.OrderId });
        }

        // ── GET /Checkout/Confirmation/{id} ───────────────────────────
        public async Task<IActionResult> Confirmation(int id)
        {
            var order = await _db.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null) return NotFound();

            return View(order);
        }

        private List<CartItem> GetCart()
        {
            var json = HttpContext.Session.GetString(CartKey);
            return json == null ? new List<CartItem>()
                : JsonSerializer.Deserialize<List<CartItem>>(json) ?? new();
        }
    }
}
