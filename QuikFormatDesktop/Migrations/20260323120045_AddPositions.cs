using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QuikFormatDesktop.Migrations
{
    /// <inheritdoc />
    public partial class AddPositions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "position",
                columns: new[] { "id", "position" },
                values: new object[,]
                {
                    { 1, "centerleft" },
                    { 2, "centerright" },
                    { 3, "rightleft" },
                    { 4, "leftright" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "position",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "position",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "position",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "position",
                keyColumn: "id",
                keyValue: 4);
        }
    }
}
