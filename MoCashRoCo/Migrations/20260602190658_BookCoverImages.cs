using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoCashRoCo.Migrations
{
    /// <inheritdoc />
    public partial class BookCoverImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 15,
                column: "ImageUrl",
                value: "https://covers.openlibrary.org/b/isbn/9781401938093-L.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 16,
                column: "ImageUrl",
                value: "https://covers.openlibrary.org/b/isbn/9781401953119-L.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 17,
                column: "ImageUrl",
                value: "https://covers.openlibrary.org/b/isbn/9781585426133-L.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 18,
                column: "ImageUrl",
                value: "https://covers.openlibrary.org/b/isbn/9780312964139-L.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 19,
                column: "ImageUrl",
                value: "https://covers.openlibrary.org/b/isbn/9780517563700-L.jpg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
