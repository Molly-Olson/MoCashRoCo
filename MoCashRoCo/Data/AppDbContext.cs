using Microsoft.EntityFrameworkCore;
using MoCashRoCo.Models;

namespace MoCashRoCo.Data
{
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
            modelBuilder.Entity<OrderItem>().HasOne(oi => oi.Order).WithMany(o => o.OrderItems).HasForeignKey(oi => oi.OrderId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<OrderItem>().HasOne(oi => oi.Product).WithMany().HasForeignKey(oi => oi.ProductId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Order>().HasOne(o => o.Customer).WithMany(c => c.Orders).HasForeignKey(o => o.CustomerId).IsRequired(false).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Product>().HasOne(p => p.Category).WithMany(c => c.Products).HasForeignKey(p => p.CategoryId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Customer>().HasIndex(c => c.Email).IsUnique();
            modelBuilder.Entity<Admin>().HasIndex(a => a.Username).IsUnique();

            modelBuilder.Entity<SiteSettings>().HasData(new SiteSettings
            {
                SiteSettingsId = 1,
                BusinessName = "MoCashRo Web Co.",
                Tagline = "Quality you can count on.",
                PrimaryColor = "#2e7d32",
                AccentColor = "#ff6f00"
            });

            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Electronics", Description = "Cutting-edge gadgets and devices" },
                new Category { CategoryId = 2, Name = "Clothing", Description = "Premium apparel and accessories" },
                new Category { CategoryId = 3, Name = "Home & Kitchen", Description = "Everything for your living space" },
                new Category { CategoryId = 4, Name = "Sports & Outdoors", Description = "Gear up for any adventure" },
                new Category { CategoryId = 5, Name = "Books & Media", Description = "Expand your mind and entertainment" }
            );

            var seedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    ProductId = 1,
                    Name = "Wireless Noise-Cancelling Headphones",
                    Description = "Experience crystal-clear audio with our premium wireless headphones. Featuring 30-hour battery life, active noise cancellation, and a comfortable over-ear design perfect for work or travel.",
                    Price = 89.99m,
                    ImageUrl = "https://picsum.photos/seed/headphones42/600/400",
                    CategoryId = 1,
                    StockQuantity = 48,
                    IsActive = true,
                    CreatedAt = seedDate
                },
                new Product
                {
                    ProductId = 2,
                    Name = "Classic Fit Premium Cotton T-Shirt",
                    Description = "Crafted from 100% organic cotton, this everyday essential delivers unmatched comfort and durability. Available in a range of timeless colors to complete any casual look.",
                    Price = 24.99m,
                    ImageUrl = "https://picsum.photos/seed/tshirt88/600/400",
                    CategoryId = 2,
                    StockQuantity = 200,
                    IsActive = true,
                    CreatedAt = seedDate
                },
                new Product
                {
                    ProductId = 3,
                    Name = "Insulated Stainless Steel Water Bottle",
                    Description = "Keep your drinks cold for 24 hours or hot for 12. Our double-wall vacuum-insulated bottle is made from food-grade stainless steel and is perfect for the gym, office, or outdoors.",
                    Price = 34.99m,
                    ImageUrl = "https://picsum.photos/seed/waterbottle15/600/400",
                    CategoryId = 3,
                    StockQuantity = 120,
                    IsActive = true,
                    CreatedAt = seedDate
                },
                new Product
                {
                    ProductId = 4,
                    Name = "Performance Running Shoes",
                    Description = "Engineered for speed and comfort, these lightweight running shoes feature responsive cushioning, breathable mesh upper, and a grippy rubber outsole. Your personal best starts here.",
                    Price = 64.99m,
                    ImageUrl = "https://picsum.photos/seed/runshoes22/600/400",
                    CategoryId = 4,
                    StockQuantity = 75,
                    IsActive = true,
                    CreatedAt = seedDate
                },
                new Product
                {
                    ProductId = 5,
                    Name = "Clean Code: A Software Craftsman's Guide",
                    Description = "A landmark guide in the field of software development, featuring best practices for writing maintainable, readable, and efficient code. An essential read for every developer.",
                    Price = 39.99m,
                    ImageUrl = "https://picsum.photos/seed/bookcode77/600/400",
                    CategoryId = 5,
                    StockQuantity = 300,
                    IsActive = true,
                    CreatedAt = seedDate
                }
            );
        }
    }
}
