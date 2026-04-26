using Microsoft.AspNetCore.Mvc;
using MoCashRoCo.Data;
using MoCashRoCo.Models;
using System.Text.Json;

namespace MoCashRoCo.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _db;
        private const string CartKey = "Cart";
        private const string CartCountKey = "CartItemCount";

        public CartController(AppDbContext db)
        {
            _db = db;
        }

        private List<CartItem> GetCart()
        {
            var json = HttpContext.Session.GetString(CartKey);
            return json == null ? new List<CartItem>() : JsonSerializer.Deserialize<List<CartItem>>(json) ?? new();
        }

        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Session.SetString(CartKey, JsonSerializer.Serialize(cart));
            HttpContext.Session.SetString(CartCountKey, cart.Sum(i => i.Quantity).ToString());
        }

        public IActionResult Index() => View(GetCart());

        [HttpPost]
        public async Task<IActionResult> Add(int productId, int quantity = 1)
        {
            var product = await _db.Products.FindAsync(productId);
            if (product == null) return NotFound();

            var cart = GetCart();
            var existing = cart.FirstOrDefault(i => i.ProductId == productId);
            if (existing != null)
                existing.Quantity += quantity;
            else
                cart.Add(new CartItem
                {
                    ProductId = productId,
                    Name = product.Name,
                    Price = product.Price,
                    ImageUrl = product.ImageUrl,
                    Quantity = quantity
                });

            SaveCart(cart);
            TempData["Success"] = $"'{product.Name}' added to cart!";

            var referer = Request.Headers.Referer.ToString();
            return Redirect(string.IsNullOrEmpty(referer) ? "/Products" : referer);
        }

        [HttpPost]
        public IActionResult Remove(int productId)
        {
            var cart = GetCart();
            cart.RemoveAll(i => i.ProductId == productId);
            SaveCart(cart);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int productId, int quantity)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                if (quantity <= 0) cart.Remove(item);
                else item.Quantity = quantity;
            }
            SaveCart(cart);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Clear()
        {
            HttpContext.Session.Remove(CartKey);
            HttpContext.Session.Remove(CartCountKey);
            return RedirectToAction("Index");
        }
    }
}
