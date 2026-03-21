using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QuikFormatDesktop.Migrations
{
    /// <inheritdoc />
    public partial class AddAlignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "alignment",
                columns: new[] { "id", "alignment" },
                values: new object[,]
                {
                    { 1, "left" },
                    { 2, "right" },
                    { 3, "center" },
                    { 4, "both" },
                    { 5, "top" },
                    { 6, "bottom" },
                    { 7, "center" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "alignment",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "alignment",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "alignment",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "alignment",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "alignment",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "alignment",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "alignment",
                keyColumn: "id",
                keyValue: 7);
        }
    }
}
