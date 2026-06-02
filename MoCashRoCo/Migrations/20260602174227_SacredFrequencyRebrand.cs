using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MoCashRoCo.Migrations
{
    /// <inheritdoc />
    public partial class SacredFrequencyRebrand : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1,
                columns: new[] { "Description", "Name" },
                values: new object[] { "High-vibration crystals and gemstones for healing, protection, and spiritual growth", "Crystals & Stones" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Singing bowls, tuning forks, and frequency tools to restore balance and harmony", "Sound Healing" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Sacred tools to deepen your practice and raise your consciousness", "Meditation Tools" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 4,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Books and programs that will expand your mind and change your reality", "Recommended Reading" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                columns: new[] { "Description", "ImageUrl", "Name", "Price", "StockQuantity" },
                values: new object[] { "A powerful cleansing and charging crystal, selenite carries an incredibly high vibration that instantly clears stagnant energy. Use it to cleanse other crystals, your aura, or your space. This silky-smooth wand is approximately 6 inches and sourced ethically.", "https://picsum.photos/seed/selenite01/600/400", "Selenite Wand", 14.99m, 60 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                columns: new[] { "CategoryId", "Description", "ImageUrl", "Name", "Price", "StockQuantity" },
                values: new object[] { 1, "Known as the stone of spiritual protection and purification, amethyst quiets the mind and supports deep meditation. This natural cluster is a stunning centerpiece for any altar or shelf, radiating calming purple energy throughout your space.", "https://picsum.photos/seed/amethyst02/600/400", "Amethyst Cluster", 28.99m, 40 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3,
                columns: new[] { "CategoryId", "Description", "ImageUrl", "Name", "Price", "StockQuantity" },
                values: new object[] { 1, "The ultimate crystal of unconditional love and self-compassion. Polished into a smooth palm stone, this rose quartz is perfect for holding during meditation, placing on your heart chakra, or carrying with you as a reminder to lead with love.", "https://picsum.photos/seed/rosequartz03/600/400", "Rose Quartz Palm Stone", 12.99m, 80 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 4,
                columns: new[] { "CategoryId", "Description", "ImageUrl", "Name", "Price", "StockQuantity" },
                values: new object[] { 1, "One of the most powerful protective stones on Earth, black tourmaline creates an energetic shield against negativity, EMF, and psychic attack. Keep a piece near your front door, in your workspace, or carry it with you for constant energetic protection.", "https://picsum.photos/seed/tourmaline04/600/400", "Black Tourmaline Raw", 9.99m, 100 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 5,
                columns: new[] { "CategoryId", "Description", "ImageUrl", "Name", "Price", "StockQuantity" },
                values: new object[] { 1, "A stone of transformation and magic, labradorite is the companion of seekers and healers. Its stunning blue-green flash (known as labradorescence) makes each sphere completely unique. Supports intuition, psychic abilities, and seeing through illusion.", "https://picsum.photos/seed/labradorite05/600/400", "Labradorite Sphere", 44.99m, 20 });

            // Delete old category 5 (Books & Media) AFTER product 5 has been moved off it
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 5);

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "CategoryId", "CreatedAt", "Description", "ImageUrl", "IsActive", "Name", "Price", "StockQuantity" },
                values: new object[,]
                {
                    { 6, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Known as the Merchant's Stone and the Light Maker, citrine carries the energy of the sun. It never needs cleansing, only giving. Use it to attract abundance, boost confidence, and clear away negative thought patterns. Tumbled smooth and warm in the hand.", "https://picsum.photos/seed/citrine06/600/400", true, "Citrine Tumbled Stone", 7.99m, 150 },
                    { 7, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "The master healer and amplifier of all crystals, clear quartz raises the vibrational frequency of everything around it. This natural generator point directs energy upward and outward, making it perfect for grids, altars, and intention-setting rituals.", "https://picsum.photos/seed/clearquartz07/600/400", true, "Clear Quartz Generator Point", 19.99m, 55 },
                    { 8, 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Handcrafted from 99.9% pure quartz crystal, this 7-inch singing bowl resonates at the B note, directly activating the crown chakra and opening your connection to higher consciousness. The sustained tone is deeply meditative and profoundly clearing.", "https://picsum.photos/seed/singingbowl08/600/400", true, "Crystal Singing Bowl — 7\" Crown Chakra (B Note)", 89.99m, 15 },
                    { 9, 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "528 Hz is known as the Miracle Tone — the frequency of love, DNA repair, and transformation. Used by healers, researchers, and sound therapists worldwide. Strike it, place the stem on the body or a crystal, and feel the resonance move through you.", "https://picsum.photos/seed/tuningfork528/600/400", true, "528 Hz Tuning Fork — Love Frequency", 34.99m, 30 },
                    { 10, 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "The complete set of nine sacred Solfeggio frequencies: 174 Hz, 285 Hz, 396 Hz, 417 Hz, 528 Hz, 639 Hz, 741 Hz, 852 Hz, and 963 Hz. Each frequency targets a specific energetic center and supports healing on a cellular level. Includes storage pouch and activator mallet.", "https://picsum.photos/seed/solfeggioset10/600/400", true, "Solfeggio Tuning Fork Set (9-piece)", 129.99m, 12 },
                    { 11, 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Handmade in Nepal by traditional craftsmen using a seven-metal alloy, this Tibetan bowl produces warm, complex overtones that ground and center the nervous system. Includes wooden striker and cushion ring. Ideal for beginners and seasoned practitioners alike.", "https://picsum.photos/seed/tibetanbowl11/600/400", true, "Tibetan Singing Bowl Set", 54.99m, 25 },
                    { 12, 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Sustainably sourced from naturally fallen trees in Ecuador, palo santo (holy wood) has been used in shamanic ceremony for centuries. Its sweet, woody smoke clears negative energy, invites good spirits, and grounds the mind before meditation.", "https://picsum.photos/seed/palosanto12/600/400", true, "Palo Santo Bundle (6 sticks)", 11.99m, 90 },
                    { 13, 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Traditionally used in Japa meditation to count mantras, this handknotted mala features 108 genuine amethyst beads with a guru bead and tassel. The number 108 is sacred across Hindu, Buddhist, and yogic traditions. Amethyst supports stillness and inner wisdom.", "https://picsum.photos/seed/mala13/600/400", true, "108-Bead Amethyst Mala", 38.99m, 35 },
                    { 14, 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Everything you need to build your first crystal grid. Includes a large Flower of Life cloth (18\"), one clear quartz center stone, six amethyst points, six rose quartz tumbles, and an instruction card. Set your intentions and let the geometry do the work.", "https://picsum.photos/seed/crystalgrid14/600/400", true, "Crystal Grid Kit — Flower of Life", 49.99m, 18 },
                    { 15, 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Dr. Joe Dispenza combines quantum physics, neuroscience, brain chemistry, and genetics to show you that you are not doomed by your genes. This landmark book teaches you that you can rewire your brain and literally become a new person. One of the most important books you will ever read.", "https://picsum.photos/seed/dispenza15/600/400", true, "Breaking the Habit of Being Yourself — Dr. Joe Dispenza", 18.99m, 200 },
                    { 16, 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "The follow-up to Breaking the Habit, this book documents how ordinary people are doing the extraordinary. Through meditation, pineal gland activation, and heart-brain coherence, Dispenza shows how humans can access mystical states and heal themselves. Required reading for anyone serious about consciousness.", "https://picsum.photos/seed/supernatural16/600/400", true, "Becoming Supernatural — Dr. Joe Dispenza", 19.99m, 200 },
                    { 17, 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "First published in 1908, The Kybalion presents the seven Hermetic principles that govern all of existence: Mentalism, Correspondence, Vibration, Polarity, Rhythm, Cause and Effect, and Gender. Once you understand these laws, you cannot unsee them. A cornerstone of esoteric study.", "https://picsum.photos/seed/kybalion17/600/400", true, "The Kybalion — Three Initiates", 12.99m, 200 },
                    { 18, 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "A firsthand account of the U.S. Army's classified remote viewing program, written by one of its operatives. David Morehouse takes you inside the world of controlled remote viewing — the ability to perceive locations, people, and events across time and space. Gripping, credible, and mind-expanding.", "https://picsum.photos/seed/morehouse18/600/400", true, "Psychic Warrior — David Morehouse", 15.99m, 200 },
                    { 19, 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "The legendary audio program developed by Robert Monroe and the Monroe Institute, declassified and studied by the CIA. Using Hemi-Sync binaural beat technology, this program guides you into altered states of consciousness, out-of-body experiences, and expanded awareness. This is the real deal.", "https://picsum.photos/seed/gateway19/600/400", true, "The Gateway Experience — Monroe Institute (Digital Program)", 149.99m, 999 }
                });

            migrationBuilder.UpdateData(
                table: "SiteSettings",
                keyColumn: "SiteSettingsId",
                keyValue: 1,
                columns: new[] { "AccentColor", "BusinessName", "PrimaryColor", "Tagline" },
                values: new object[] { "#c9a84c", "Sacred Frequency", "#3b1f6e", "Raise your vibration. Trust the frequency." });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 19);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Cutting-edge gadgets and devices", "Electronics" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Premium apparel and accessories", "Clothing" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Everything for your living space", "Home & Kitchen" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 4,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Gear up for any adventure", "Sports & Outdoors" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Description", "Name" },
                values: new object[] { 5, "Expand your mind and entertainment", "Books & Media" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                columns: new[] { "Description", "ImageUrl", "Name", "Price", "StockQuantity" },
                values: new object[] { "Experience crystal-clear audio with our premium wireless headphones. Featuring 30-hour battery life, active noise cancellation, and a comfortable over-ear design perfect for work or travel.", "https://picsum.photos/seed/headphones42/600/400", "Wireless Noise-Cancelling Headphones", 89.99m, 48 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                columns: new[] { "CategoryId", "Description", "ImageUrl", "Name", "Price", "StockQuantity" },
                values: new object[] { 2, "Crafted from 100% organic cotton, this everyday essential delivers unmatched comfort and durability. Available in a range of timeless colors to complete any casual look.", "https://picsum.photos/seed/tshirt88/600/400", "Classic Fit Premium Cotton T-Shirt", 24.99m, 200 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3,
                columns: new[] { "CategoryId", "Description", "ImageUrl", "Name", "Price", "StockQuantity" },
                values: new object[] { 3, "Keep your drinks cold for 24 hours or hot for 12. Our double-wall vacuum-insulated bottle is made from food-grade stainless steel and is perfect for the gym, office, or outdoors.", "https://picsum.photos/seed/waterbottle15/600/400", "Insulated Stainless Steel Water Bottle", 34.99m, 120 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 4,
                columns: new[] { "CategoryId", "Description", "ImageUrl", "Name", "Price", "StockQuantity" },
                values: new object[] { 4, "Engineered for speed and comfort, these lightweight running shoes feature responsive cushioning, breathable mesh upper, and a grippy rubber outsole. Your personal best starts here.", "https://picsum.photos/seed/runshoes22/600/400", "Performance Running Shoes", 64.99m, 75 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 5,
                columns: new[] { "CategoryId", "Description", "ImageUrl", "Name", "Price", "StockQuantity" },
                values: new object[] { 5, "A landmark guide in the field of software development, featuring best practices for writing maintainable, readable, and efficient code. An essential read for every developer.", "https://picsum.photos/seed/bookcode77/600/400", "Clean Code: A Software Craftsman's Guide", 39.99m, 300 });

            migrationBuilder.UpdateData(
                table: "SiteSettings",
                keyColumn: "SiteSettingsId",
                keyValue: 1,
                columns: new[] { "AccentColor", "BusinessName", "PrimaryColor", "Tagline" },
                values: new object[] { "#ff6f00", "MoCashRo Web Co.", "#2e7d32", "Quality you can count on." });
        }
    }
}
