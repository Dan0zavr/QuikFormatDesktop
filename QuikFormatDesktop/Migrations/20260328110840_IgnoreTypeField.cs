using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuikFormatDesktop.Migrations
{
    /// <inheritdoc />
    public partial class IgnoreTypeField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
