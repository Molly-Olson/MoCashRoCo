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
                BusinessName = "Sacred Frequency",
                Tagline = "Raise your vibration. Trust the frequency.",
                PrimaryColor = "#3b1f6e",
                AccentColor = "#c9a84c"
            });

            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Crystals & Stones", Description = "High-vibration crystals and gemstones for healing, protection, and spiritual growth" },
                new Category { CategoryId = 2, Name = "Sound Healing", Description = "Singing bowls, tuning forks, and frequency tools to restore balance and harmony" },
                new Category { CategoryId = 3, Name = "Meditation Tools", Description = "Sacred tools to deepen your practice and raise your consciousness" },
                new Category { CategoryId = 4, Name = "Recommended Reading", Description = "Books and programs that will expand your mind and change your reality" }
            );

            var seedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            modelBuilder.Entity<Product>().HasData(
                // ── Crystals & Stones ──────────────────────────────────
                new Product
                {
                    ProductId = 1,
                    Name = "Selenite Wand",
                    Description = "A powerful cleansing and charging crystal, selenite carries an incredibly high vibration that instantly clears stagnant energy. Use it to cleanse other crystals, your aura, or your space. This silky-smooth wand is approximately 6 inches and sourced ethically.",
                    Price = 14.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1772911421293-362c64541490?w=600&h=400&fit=crop&auto=format",
                    CategoryId = 1,
                    StockQuantity = 60,
                    IsActive = true,
                    CreatedAt = seedDate
                },
                new Product
                {
                    ProductId = 2,
                    Name = "Amethyst Cluster",
                    Description = "Known as the stone of spiritual protection and purification, amethyst quiets the mind and supports deep meditation. This natural cluster is a stunning centerpiece for any altar or shelf, radiating calming purple energy throughout your space.",
                    Price = 28.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1626470408813-f0059745d58b?w=600&h=400&fit=crop&auto=format",
                    CategoryId = 1,
                    StockQuantity = 40,
                    IsActive = true,
                    CreatedAt = seedDate
                },
                new Product
                {
                    ProductId = 3,
                    Name = "Rose Quartz Palm Stone",
                    Description = "The ultimate crystal of unconditional love and self-compassion. Polished into a smooth palm stone, this rose quartz is perfect for holding during meditation, placing on your heart chakra, or carrying with you as a reminder to lead with love.",
                    Price = 12.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1753734051188-b58b9ba4cdc1?w=600&h=400&fit=crop&auto=format",
                    CategoryId = 1,
                    StockQuantity = 80,
                    IsActive = true,
                    CreatedAt = seedDate
                },
                new Product
                {
                    ProductId = 4,
                    Name = "Black Tourmaline Raw",
                    Description = "One of the most powerful protective stones on Earth, black tourmaline creates an energetic shield against negativity, EMF, and psychic attack. Keep a piece near your front door, in your workspace, or carry it with you for constant energetic protection.",
                    Price = 9.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1753522312806-a78c9fee860e?w=600&h=400&fit=crop&auto=format",
                    CategoryId = 1,
                    StockQuantity = 100,
                    IsActive = true,
                    CreatedAt = seedDate
                },
                new Product
                {
                    ProductId = 5,
                    Name = "Labradorite Sphere",
                    Description = "A stone of transformation and magic, labradorite is the companion of seekers and healers. Its stunning blue-green flash (known as labradorescence) makes each sphere completely unique. Supports intuition, psychic abilities, and seeing through illusion.",
                    Price = 44.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1659468550840-602345a513d9?w=600&h=400&fit=crop&auto=format",
                    CategoryId = 1,
                    StockQuantity = 20,
                    IsActive = true,
                    CreatedAt = seedDate
                },
                new Product
                {
                    ProductId = 6,
                    Name = "Citrine Tumbled Stone",
                    Description = "Known as the Merchant's Stone and the Light Maker, citrine carries the energy of the sun. It never needs cleansing, only giving. Use it to attract abundance, boost confidence, and clear away negative thought patterns. Tumbled smooth and warm in the hand.",
                    Price = 7.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1614092872241-c9a193f2b4aa?w=600&h=400&fit=crop&auto=format",
                    CategoryId = 1,
                    StockQuantity = 150,
                    IsActive = true,
                    CreatedAt = seedDate
                },
                new Product
                {
                    ProductId = 7,
                    Name = "Clear Quartz Generator Point",
                    Description = "The master healer and amplifier of all crystals, clear quartz raises the vibrational frequency of everything around it. This natural generator point directs energy upward and outward, making it perfect for grids, altars, and intention-setting rituals.",
                    Price = 19.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1767131543309-be0996beb61e?w=600&h=400&fit=crop&auto=format",
                    CategoryId = 1,
                    StockQuantity = 55,
                    IsActive = true,
                    CreatedAt = seedDate
                },
                // ── Sound Healing ──────────────────────────────────────
                new Product
                {
                    ProductId = 8,
                    Name = "Crystal Singing Bowl — 7\" Crown Chakra (B Note)",
                    Description = "Handcrafted from 99.9% pure quartz crystal, this 7-inch singing bowl resonates at the B note, directly activating the crown chakra and opening your connection to higher consciousness. The sustained tone is deeply meditative and profoundly clearing.",
                    Price = 89.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1740478949578-1b58ae46b584?w=600&h=400&fit=crop&auto=format",
                    CategoryId = 2,
                    StockQuantity = 15,
                    IsActive = true,
                    CreatedAt = seedDate
                },
                new Product
                {
                    ProductId = 9,
                    Name = "528 Hz Tuning Fork — Love Frequency",
                    Description = "528 Hz is known as the Miracle Tone — the frequency of love, DNA repair, and transformation. Used by healers, researchers, and sound therapists worldwide. Strike it, place the stem on the body or a crystal, and feel the resonance move through you.",
                    Price = 34.99m,
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/b/bb/Tuning_fork_on_resonator.jpg",
                    CategoryId = 2,
                    StockQuantity = 30,
                    IsActive = true,
                    CreatedAt = seedDate
                },
                new Product
                {
                    ProductId = 10,
                    Name = "Solfeggio Tuning Fork Set (9-piece)",
                    Description = "The complete set of nine sacred Solfeggio frequencies: 174 Hz, 285 Hz, 396 Hz, 417 Hz, 528 Hz, 639 Hz, 741 Hz, 852 Hz, and 963 Hz. Each frequency targets a specific energetic center and supports healing on a cellular level. Includes storage pouch and activator mallet.",
                    Price = 129.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1662221720534-87433a842dc1?w=600&h=400&fit=crop&auto=format",
                    CategoryId = 2,
                    StockQuantity = 12,
                    IsActive = true,
                    CreatedAt = seedDate
                },
                new Product
                {
                    ProductId = 11,
                    Name = "Tibetan Singing Bowl Set",
                    Description = "Handmade in Nepal by traditional craftsmen using a seven-metal alloy, this Tibetan bowl produces warm, complex overtones that ground and center the nervous system. Includes wooden striker and cushion ring. Ideal for beginners and seasoned practitioners alike.",
                    Price = 54.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1627764627459-ba29d6051fe0?w=600&h=400&fit=crop&auto=format",
                    CategoryId = 2,
                    StockQuantity = 25,
                    IsActive = true,
                    CreatedAt = seedDate
                },
                // ── Meditation Tools ───────────────────────────────────
                new Product
                {
                    ProductId = 12,
                    Name = "Palo Santo Bundle (6 sticks)",
                    Description = "Sustainably sourced from naturally fallen trees in Ecuador, palo santo (holy wood) has been used in shamanic ceremony for centuries. Its sweet, woody smoke clears negative energy, invites good spirits, and grounds the mind before meditation.",
                    Price = 11.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1612088486201-2cb53360306c?w=600&h=400&fit=crop&auto=format",
                    CategoryId = 3,
                    StockQuantity = 90,
                    IsActive = true,
                    CreatedAt = seedDate
                },
                new Product
                {
                    ProductId = 13,
                    Name = "108-Bead Amethyst Mala",
                    Description = "Traditionally used in Japa meditation to count mantras, this handknotted mala features 108 genuine amethyst beads with a guru bead and tassel. The number 108 is sacred across Hindu, Buddhist, and yogic traditions. Amethyst supports stillness and inner wisdom.",
                    Price = 38.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1556760647-90d218f7ca5b?w=600&h=400&fit=crop&auto=format",
                    CategoryId = 3,
                    StockQuantity = 35,
                    IsActive = true,
                    CreatedAt = seedDate
                },
                new Product
                {
                    ProductId = 14,
                    Name = "Crystal Grid Kit — Flower of Life",
                    Description = "Everything you need to build your first crystal grid. Includes a large Flower of Life cloth (18\"), one clear quartz center stone, six amethyst points, six rose quartz tumbles, and an instruction card. Set your intentions and let the geometry do the work.",
                    Price = 49.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1636392701520-e66bb50b313d?w=600&h=400&fit=crop&auto=format",
                    CategoryId = 3,
                    StockQuantity = 18,
                    IsActive = true,
                    CreatedAt = seedDate
                },
                // ── Recommended Reading ────────────────────────────────
                new Product
                {
                    ProductId = 15,
                    Name = "Breaking the Habit of Being Yourself — Dr. Joe Dispenza",
                    Description = "Dr. Joe Dispenza combines quantum physics, neuroscience, brain chemistry, and genetics to show you that you are not doomed by your genes. This landmark book teaches you that you can rewire your brain and literally become a new person. One of the most important books you will ever read.",
                    Price = 18.99m,
                    ImageUrl = "https://covers.openlibrary.org/b/isbn/9781401938093-L.jpg",
                    CategoryId = 4,
                    StockQuantity = 200,
                    IsActive = true,
                    CreatedAt = seedDate
                },
                new Product
                {
                    ProductId = 16,
                    Name = "Becoming Supernatural — Dr. Joe Dispenza",
                    Description = "The follow-up to Breaking the Habit, this book documents how ordinary people are doing the extraordinary. Through meditation, pineal gland activation, and heart-brain coherence, Dispenza shows how humans can access mystical states and heal themselves. Required reading for anyone serious about consciousness.",
                    Price = 19.99m,
                    ImageUrl = "https://covers.openlibrary.org/b/isbn/9781401953119-L.jpg",
                    CategoryId = 4,
                    StockQuantity = 200,
                    IsActive = true,
                    CreatedAt = seedDate
                },
                new Product
                {
                    ProductId = 17,
                    Name = "The Kybalion — Three Initiates",
                    Description = "First published in 1908, The Kybalion presents the seven Hermetic principles that govern all of existence: Mentalism, Correspondence, Vibration, Polarity, Rhythm, Cause and Effect, and Gender. Once you understand these laws, you cannot unsee them. A cornerstone of esoteric study.",
                    Price = 12.99m,
                    ImageUrl = "https://m.media-amazon.com/images/I/A1V0D0EpG+L._SL500_.jpg",
                    CategoryId = 4,
                    StockQuantity = 200,
                    IsActive = true,
                    CreatedAt = seedDate
                },
                new Product
                {
                    ProductId = 18,
                    Name = "Psychic Warrior — David Morehouse",
                    Description = "A firsthand account of the U.S. Army's classified remote viewing program, written by one of its operatives. David Morehouse takes you inside the world of controlled remote viewing — the ability to perceive locations, people, and events across time and space. Gripping, credible, and mind-expanding.",
                    Price = 15.99m,
                    ImageUrl = "https://covers.openlibrary.org/b/isbn/9780312964139-L.jpg",
                    CategoryId = 4,
                    StockQuantity = 200,
                    IsActive = true,
                    CreatedAt = seedDate
                },
                new Product
                {
                    ProductId = 19,
                    Name = "The Gateway Experience — Monroe Institute (Digital Program)",
                    Description = "The legendary audio program developed by Robert Monroe and the Monroe Institute, declassified and studied by the CIA. Using Hemi-Sync binaural beat technology, this program guides you into altered states of consciousness, out-of-body experiences, and expanded awareness. This is the real deal.",
                    Price = 149.99m,
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/d/dc/Analysis_and_Assessment_of_Gateway_Process.pdf/page1-250px-Analysis_and_Assessment_of_Gateway_Process.pdf.jpg",
                    CategoryId = 4,
                    StockQuantity = 999,
                    IsActive = true,
                    CreatedAt = seedDate
                }
            );
        }
    }
}
