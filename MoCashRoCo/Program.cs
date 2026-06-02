using MoCashRoCo.Data;
using MoCashRoCo.Models;
using Microsoft.EntityFrameworkCore;

namespace MoCashRoCo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            builder.Services.AddHttpContextAccessor();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseSession();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            // Ensure DB is up to date and seed a default admin account if none exists
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.Migrate();
                if (!db.Admins.Any())
                {
                    db.Admins.Add(new Admin
                    {
                        Username     = "admin",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin1234!"),
                        Role         = "Admin",
                        CreatedAt    = DateTime.UtcNow
                    });
                    db.SaveChanges();
                }
            }

            app.Run();
        }
    }
}
