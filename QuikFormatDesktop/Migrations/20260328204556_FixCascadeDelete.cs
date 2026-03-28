using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuikFormatDesktop.Migrations
{
    /// <inheritdoc />
    public partial class FixCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_picture_style_paragraph_style_paragraph_style",
                table: "picture_style");

            migrationBuilder.AddForeignKey(
                name: "FK_picture_style_paragraph_style_paragraph_style",
                table: "picture_style",
                column: "paragraph_style",
                principalTable: "paragraph_style",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_picture_style_paragraph_style_paragraph_style",
                table: "picture_style");

            migrationBuilder.AddForeignKey(
                name: "FK_picture_style_paragraph_style_paragraph_style",
                table: "picture_style",
                column: "paragraph_style",
                principalTable: "paragraph_style",
                principalColumn: "id");
        }
    }
}
