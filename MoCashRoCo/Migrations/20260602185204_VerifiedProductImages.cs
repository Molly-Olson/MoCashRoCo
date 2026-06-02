using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoCashRoCo.Migrations
{
    /// <inheritdoc />
    public partial class VerifiedProductImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                column: "ImageUrl",
                value: "https://images.unsplash.com/photo-1597336465111-a392afd218bc?w=600&h=400&fit=crop&auto=format");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                column: "ImageUrl",
                value: "https://images.unsplash.com/photo-1632980205460-e490e885e848?w=600&h=400&fit=crop&auto=format");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3,
                column: "ImageUrl",
                value: "https://images.unsplash.com/photo-1593259213062-57b0ce5906cf?w=600&h=400&fit=crop&auto=format");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 4,
                column: "ImageUrl",
                value: "https://images.unsplash.com/photo-1753522312806-a78c9fee860e?w=600&h=400&fit=crop&auto=format");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 5,
                column: "ImageUrl",
                value: "https://images.unsplash.com/photo-1534883031555-7d18c6cf52e7?w=600&h=400&fit=crop&auto=format");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 6,
                column: "ImageUrl",
                value: "https://images.unsplash.com/photo-1543384490-fc38bd91de41?w=600&h=400&fit=crop&auto=format");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 7,
                column: "ImageUrl",
                value: "https://images.unsplash.com/photo-1562162135-9f64f33e623b?w=600&h=400&fit=crop&auto=format");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 8,
                column: "ImageUrl",
                value: "https://images.unsplash.com/photo-1579291465308-fba6c5db2dfe?w=600&h=400&fit=crop&auto=format");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 9,
                column: "ImageUrl",
                value: "https://images.unsplash.com/photo-1662221720534-87433a842dc1?w=600&h=400&fit=crop&auto=format");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 10,
                column: "ImageUrl",
                value: "https://images.unsplash.com/photo-1662221720534-87433a842dc1?w=600&h=400&fit=crop&auto=format");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 11,
                column: "ImageUrl",
                value: "https://images.unsplash.com/photo-1627764627459-ba29d6051fe0?w=600&h=400&fit=crop&auto=format");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 12,
                column: "ImageUrl",
                value: "https://images.unsplash.com/photo-1612088486201-2cb53360306c?w=600&h=400&fit=crop&auto=format");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 13,
                column: "ImageUrl",
                value: "https://images.unsplash.com/photo-1556760647-90d218f7ca5b?w=600&h=400&fit=crop&auto=format");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 14,
                column: "ImageUrl",
                value: "https://images.unsplash.com/photo-1636392701520-e66bb50b313d?w=600&h=400&fit=crop&auto=format");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 15,
                column: "ImageUrl",
                value: "https://images.unsplash.com/photo-1599139449818-4ca5baa081bd?w=600&h=400&fit=crop&auto=format");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 16,
                column: "ImageUrl",
                value: "https://images.unsplash.com/photo-1701634441311-e624c7fdfedf?w=600&h=400&fit=crop&auto=format");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 17,
                column: "ImageUrl",
                value: "https://images.unsplash.com/photo-1620975522168-1ee4ca17ccb5?w=600&h=400&fit=crop&auto=format");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 18,
                column: "ImageUrl",
                value: "https://images.unsplash.com/photo-1604745372175-27daa24a0a0e?w=600&h=400&fit=crop&auto=format");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 19,
                column: "ImageUrl",
                value: "https://images.unsplash.com/photo-1504812333783-63b845853c20?w=600&h=400&fit=crop&auto=format");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                column: "ImageUrl",
                value: "https://loremflickr.com/600/400/selenite,crystal");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                column: "ImageUrl",
                value: "https://loremflickr.com/600/400/amethyst,crystal");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3,
                column: "ImageUrl",
                value: "https://loremflickr.com/600/400/rose,quartz");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 4,
                column: "ImageUrl",
                value: "https://loremflickr.com/600/400/black,tourmaline");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 5,
                column: "ImageUrl",
                value: "https://loremflickr.com/600/400/labradorite");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 6,
                column: "ImageUrl",
                value: "https://loremflickr.com/600/400/citrine,gemstone");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 7,
                column: "ImageUrl",
                value: "https://loremflickr.com/600/400/quartz,crystal,point");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 8,
                column: "ImageUrl",
                value: "https://loremflickr.com/600/400/singing,bowl,crystal");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 9,
                column: "ImageUrl",
                value: "https://loremflickr.com/600/400/tuning,fork,healing");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 10,
                column: "ImageUrl",
                value: "https://loremflickr.com/600/400/sound,healing,frequency");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 11,
                column: "ImageUrl",
                value: "https://loremflickr.com/600/400/tibetan,singing,bowl");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 12,
                column: "ImageUrl",
                value: "https://loremflickr.com/600/400/palo,santo,smudge");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 13,
                column: "ImageUrl",
                value: "https://loremflickr.com/600/400/mala,beads,meditation");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 14,
                column: "ImageUrl",
                value: "https://loremflickr.com/600/400/crystal,grid,sacred,geometry");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 15,
                column: "ImageUrl",
                value: "https://loremflickr.com/600/400/spirituality,meditation,book");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 16,
                column: "ImageUrl",
                value: "https://loremflickr.com/600/400/consciousness,awakening,meditation");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 17,
                column: "ImageUrl",
                value: "https://loremflickr.com/600/400/ancient,esoteric,mystical");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 18,
                column: "ImageUrl",
                value: "https://loremflickr.com/600/400/mind,cosmos,universe");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 19,
                column: "ImageUrl",
                value: "https://loremflickr.com/600/400/galaxy,cosmos,meditation");
        }
    }
}
