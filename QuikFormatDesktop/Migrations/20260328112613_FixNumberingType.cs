using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuikFormatDesktop.Migrations
{
    /// <inheritdoc />
    public partial class FixNumberingType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_template_text_style_marked_numbering_style",
                table: "template");

            migrationBuilder.DropForeignKey(
                name: "FK_template_text_style_numbered_numbering_style",
                table: "template");

            migrationBuilder.AddForeignKey(
                name: "FK_template_numbering_style_marked_numbering_style",
                table: "template",
                column: "marked_numbering_style",
                principalTable: "numbering_style",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_template_numbering_style_numbered_numbering_style",
                table: "template",
                column: "numbered_numbering_style",
                principalTable: "numbering_style",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_template_numbering_style_marked_numbering_style",
                table: "template");

            migrationBuilder.DropForeignKey(
                name: "FK_template_numbering_style_numbered_numbering_style",
                table: "template");

            migrationBuilder.AddForeignKey(
                name: "FK_template_text_style_marked_numbering_style",
                table: "template",
                column: "marked_numbering_style",
                principalTable: "text_style",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_template_text_style_numbered_numbering_style",
                table: "template",
                column: "numbered_numbering_style",
                principalTable: "text_style",
                principalColumn: "id");
        }
    }
}
