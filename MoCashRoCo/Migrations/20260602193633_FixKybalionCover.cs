using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoCashRoCo.Migrations
{
    /// <inheritdoc />
    public partial class FixKybalionCover : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 17,
                column: "ImageUrl",
                value: "https://m.media-amazon.com/images/I/A1V0D0EpG+L._SL500_.jpg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 17,
                column: "ImageUrl",
                value: "https://covers.openlibrary.org/b/isbn/9780486474564-L.jpg");
        }
    }
}
