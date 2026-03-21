using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuikFormatDesktop.Migrations
{
    /// <inheritdoc />
    public partial class SyncMarkerType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "marker_type",
                keyColumn: "id",
                keyValue: 2,
                column: "type",
                value: "numberd");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "marker_type",
                keyColumn: "id",
                keyValue: 2,
                column: "type",
                value: "numbered");
        }
    }
}
