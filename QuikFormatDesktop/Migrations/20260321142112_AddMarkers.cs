using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QuikFormatDesktop.Migrations
{
    /// <inheritdoc />
    public partial class AddMarkers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "text_style",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "template",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "table_style",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "picture_style",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "paragraph_style",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "numbering_style",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "formula_style",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "marker_type",
                columns: new[] { "id", "type" },
                values: new object[,]
                {
                    { 1, "marked" },
                    { 2, "numbered" }
                });

            migrationBuilder.InsertData(
                table: "marker",
                columns: new[] { "id", "marker", "marker_type" },
                values: new object[,]
                {
                    { 1, "&#8211;", 1 },
                    { 2, "&#8226;", 1 },
                    { 3, "&#8227;", 1 },
                    { 4, "$.", 2 },
                    { 5, "$)", 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "marker",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "marker",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "marker",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "marker",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "marker",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "marker_type",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "marker_type",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "Type",
                table: "text_style");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "template");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "table_style");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "picture_style");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "paragraph_style");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "numbering_style");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "formula_style");
        }
    }
}
