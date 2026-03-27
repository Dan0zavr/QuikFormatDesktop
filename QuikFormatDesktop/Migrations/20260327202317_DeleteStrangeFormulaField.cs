using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuikFormatDesktop.Migrations
{
    /// <inheritdoc />
    public partial class DeleteStrangeFormulaField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "formula",
                table: "template");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "formula",
                table: "template",
                type: "style integer",
                nullable: true);
        }
    }
}
