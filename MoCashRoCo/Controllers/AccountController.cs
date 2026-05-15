using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoCashRoCo.Data;
using MoCashRoCo.Models;

namespace MoCashRoCo.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _db;
        private const string SessionUserId   = "UserId";
        private const string SessionUserName = "UserName";
        private const string SessionUserRole = "UserRole";

        public AccountController(AppDbContext db) => _db = db;

        // ── Customer login ────────────────────────────────────────────
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (IsLoggedIn()) return RedirectToLocal(returnUrl);
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var customer = await _db.Customers
                .FirstOrDefaultAsync(c => c.Email == model.Email);

            if (customer == null || !BCrypt.Net.BCrypt.Verify(model.Password, customer.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View(model);
            }

            SetSession(customer.CustomerId, customer.Name, "Customer");
            return RedirectToLocal(model.ReturnUrl);
        }

        // ── Customer register ─────────────────────────────────────────
        [HttpGet]
        public IActionResult Register() =>
            IsLoggedIn() ? RedirectToAction("Index", "Home") : View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (await _db.Customers.AnyAsync(c => c.Email == model.Email))
            {
                ModelState.AddModelError("Email", "An account with that email already exists.");
                return View(model);
            }

            var customer = new Customer
            {
                Name         = model.Name,
                Email        = model.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                RegisteredAt = DateTime.UtcNow
            };

            _db.Customers.Add(customer);
            await _db.SaveChangesAsync();

            SetSession(customer.CustomerId, customer.Name, "Customer");
            TempData["Success"] = $"Welcome, {customer.Name}! Your account has been created.";
            return RedirectToAction("Index", "Home");
        }

        // ── Admin login ───────────────────────────────────────────────
        [HttpGet]
        public IActionResult AdminLogin(string? returnUrl = null)
        {
            if (HttpContext.Session.GetString(SessionUserRole) == "Admin")
                return RedirectToLocal(returnUrl);
            return View(new AdminLoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminLogin(AdminLoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var admin = await _db.Admins
                .FirstOrDefaultAsync(a => a.Username == model.Username);

            if (admin == null || !BCrypt.Net.BCrypt.Verify(model.Password, admin.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return View(model);
            }

            SetSession(admin.AdminId, admin.Username, "Admin");
            TempData["Success"] = $"Welcome back, {admin.Username}!";
            return RedirectToLocal(model.ReturnUrl);
        }

        // ── Logout ────────────────────────────────────────────────────
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["Success"] = "You have been signed out.";
            return RedirectToAction("Index", "Home");
        }

        // ── Helpers ───────────────────────────────────────────────────
        private bool IsLoggedIn() =>
            HttpContext.Session.GetString(SessionUserId) != null;

        private void SetSession(int id, string name, string role)
        {
            HttpContext.Session.SetString(SessionUserId,   id.ToString());
            HttpContext.Session.SetString(SessionUserName, name);
            HttpContext.Session.SetString(SessionUserRole, role);
        }

        private IActionResult RedirectToLocal(string? returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }
    }
}
