using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QuikFormatDesktop.Migrations
{
    /// <inheritdoc />
    public partial class AddGlobalStyleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "alignment",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    alignment = table.Column<string>(type: "varchar(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_alignment", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "font",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    font_name = table.Column<string>(type: "varchar(32)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_font", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "marker_type",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    type = table.Column<string>(type: "varchar(13)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_marker_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "position",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    position = table.Column<string>(type: "varchar(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_position", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "global_style",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    left_margin = table.Column<double>(type: "REAL", nullable: false),
                    right_margin = table.Column<double>(type: "REAL", nullable: false),
                    top_margin = table.Column<double>(type: "REAL", nullable: false),
                    bottom_margin = table.Column<double>(type: "REAL", nullable: false),
                    alignment = table.Column<int>(type: "INTEGER", nullable: false),
                    Alignment = table.Column<string>(type: "TEXT", nullable: false),
                    last_no_numbering_page = table.Column<int>(type: "INTEGER", nullable: true),
                    special_colontitul = table.Column<string>(type: "varchar(16)", nullable: true),
                    name = table.Column<string>(type: "varchar(32)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_global_style", x => x.id);
                    table.ForeignKey(
                        name: "FK_global_style_alignment_alignment",
                        column: x => x.alignment,
                        principalTable: "alignment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "paragraph_style",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    alignment = table.Column<int>(type: "INTEGER", nullable: false),
                    first_line_indent = table.Column<double>(type: "REAL", nullable: true),
                    left_indent = table.Column<double>(type: "REAL", nullable: true),
                    right_indent = table.Column<double>(type: "REAL", nullable: true),
                    interval_in_text = table.Column<double>(type: "REAL", nullable: false),
                    before_interval = table.Column<double>(type: "REAL", nullable: true),
                    after_interval = table.Column<double>(type: "REAL", nullable: true),
                    contextual_spacing = table.Column<bool>(type: "boolean", nullable: false),
                    name = table.Column<string>(type: "varchar(32)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_paragraph_style", x => x.id);
                    table.ForeignKey(
                        name: "FK_paragraph_style_alignment_alignment",
                        column: x => x.alignment,
                        principalTable: "alignment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "text_style",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    font = table.Column<int>(type: "INTEGER", nullable: false),
                    font_size = table.Column<int>(type: "INTEGER", nullable: false),
                    name = table.Column<string>(type: "varchar(32)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_text_style", x => x.id);
                    table.ForeignKey(
                        name: "FK_text_style_font_font",
                        column: x => x.font,
                        principalTable: "font",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "marker",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    marker = table.Column<string>(type: "varchar(8)", nullable: false),
                    marker_type = table.Column<int>(type: "INT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_marker", x => x.id);
                    table.ForeignKey(
                        name: "FK_marker_marker_type_marker_type",
                        column: x => x.marker_type,
                        principalTable: "marker_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "picture_style",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    paragraph_style = table.Column<int>(type: "INTEGER", nullable: false),
                    generate_label = table.Column<bool>(type: "boolean", nullable: false),
                    label_value = table.Column<string>(type: "varchar(16)", nullable: true),
                    empty_line_around = table.Column<bool>(type: "bool", nullable: false),
                    name = table.Column<string>(type: "varchar(32)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_picture_style", x => x.id);
                    table.ForeignKey(
                        name: "FK_picture_style_paragraph_style_paragraph_style",
                        column: x => x.paragraph_style,
                        principalTable: "paragraph_style",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "table_style",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    text_style = table.Column<int>(type: "INTEGER", nullable: false),
                    paragraph_style = table.Column<int>(type: "INTEGER", nullable: false),
                    alignment = table.Column<int>(type: "INTEGER", nullable: false),
                    border_thikness = table.Column<int>(type: "INTEGER", nullable: false),
                    border_color = table.Column<string>(type: "char(7)", nullable: false),
                    cell_padding = table.Column<double>(type: "REAL", nullable: false),
                    name = table.Column<string>(type: "varchar(32)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_table_style", x => x.id);
                    table.ForeignKey(
                        name: "FK_table_style_alignment_alignment",
                        column: x => x.alignment,
                        principalTable: "alignment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_table_style_paragraph_style_paragraph_style",
                        column: x => x.paragraph_style,
                        principalTable: "paragraph_style",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_table_style_text_style_text_style",
                        column: x => x.text_style,
                        principalTable: "text_style",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "formula_style",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    numeration = table.Column<bool>(type: "bool", nullable: false),
                    empty_line_around = table.Column<bool>(type: "bool", nullable: false),
                    marker = table.Column<int>(type: "INTEGER", nullable: true),
                    position = table.Column<int>(type: "INTEGER", nullable: false),
                    name = table.Column<string>(type: "varchar(32)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_formula_style", x => x.id);
                    table.ForeignKey(
                        name: "FK_formula_style_marker_marker",
                        column: x => x.marker,
                        principalTable: "marker",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_formula_style_position_position",
                        column: x => x.position,
                        principalTable: "position",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "numbering_style",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    marker = table.Column<int>(type: "INTEGER", nullable: false),
                    name = table.Column<string>(type: "varchar(32)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_numbering_style", x => x.id);
                    table.ForeignKey(
                        name: "FK_numbering_style_marker_marker",
                        column: x => x.marker,
                        principalTable: "marker",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "template",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    description = table.Column<string>(type: "TEXT", nullable: true),
                    text_style = table.Column<int>(type: "INTEGER", nullable: true),
                    paragraph_style = table.Column<int>(type: "INTEGER", nullable: true),
                    table_style = table.Column<int>(type: "INTEGER", nullable: true),
                    picture_style = table.Column<int>(type: "INTEGER", nullable: true),
                    formula_style = table.Column<int>(type: "INTEGER", nullable: true),
                    marked_numbering_style = table.Column<int>(type: "INTEGER", nullable: true),
                    numbered_numbering_style = table.Column<int>(type: "INTEGER", nullable: true),
                    global_style = table.Column<int>(type: "INTEGER", nullable: true),
                    name = table.Column<string>(type: "varchar(32)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_template", x => x.id);
                    table.ForeignKey(
                        name: "FK_template_formula_style_formula_style",
                        column: x => x.formula_style,
                        principalTable: "formula_style",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_template_global_style_global_style",
                        column: x => x.global_style,
                        principalTable: "global_style",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_template_numbering_style_marked_numbering_style",
                        column: x => x.marked_numbering_style,
                        principalTable: "numbering_style",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_template_numbering_style_numbered_numbering_style",
                        column: x => x.numbered_numbering_style,
                        principalTable: "numbering_style",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_template_paragraph_style_paragraph_style",
                        column: x => x.paragraph_style,
                        principalTable: "paragraph_style",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_template_picture_style_picture_style",
                        column: x => x.picture_style,
                        principalTable: "picture_style",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_template_table_style_table_style",
                        column: x => x.table_style,
                        principalTable: "table_style",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_template_text_style_text_style",
                        column: x => x.text_style,
                        principalTable: "text_style",
                        principalColumn: "id");
                });

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
                    { 6, "bottom" }
                });

            migrationBuilder.InsertData(
                table: "font",
                columns: new[] { "id", "font_name" },
                values: new object[,]
                {
                    { 1, "Times New Roman" },
                    { 2, "Arial" },
                    { 3, "Calibri" }
                });

            migrationBuilder.InsertData(
                table: "marker_type",
                columns: new[] { "id", "type" },
                values: new object[,]
                {
                    { 1, "marked" },
                    { 2, "numbered" }
                });

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

            migrationBuilder.InsertData(
                table: "marker",
                columns: new[] { "id", "marker", "marker_type" },
                values: new object[,]
                {
                    { 1, "&#8211;", 1 },
                    { 2, "&#8226;", 1 },
                    { 3, "&#8227;", 1 },
                    { 4, "–", 1 },
                    { 5, "$.", 2 },
                    { 6, "$)", 2 },
                    { 7, "($)", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_alignment_alignment",
                table: "alignment",
                column: "alignment",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_font_font_name",
                table: "font",
                column: "font_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_formula_style_marker",
                table: "formula_style",
                column: "marker");

            migrationBuilder.CreateIndex(
                name: "IX_formula_style_name",
                table: "formula_style",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_formula_style_position",
                table: "formula_style",
                column: "position");

            migrationBuilder.CreateIndex(
                name: "IX_global_style_alignment",
                table: "global_style",
                column: "alignment");

            migrationBuilder.CreateIndex(
                name: "IX_global_style_name",
                table: "global_style",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_marker_marker",
                table: "marker",
                column: "marker",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_marker_marker_type",
                table: "marker",
                column: "marker_type");

            migrationBuilder.CreateIndex(
                name: "IX_marker_type_type",
                table: "marker_type",
                column: "type",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_numbering_style_marker",
                table: "numbering_style",
                column: "marker");

            migrationBuilder.CreateIndex(
                name: "IX_numbering_style_name",
                table: "numbering_style",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_paragraph_style_alignment",
                table: "paragraph_style",
                column: "alignment");

            migrationBuilder.CreateIndex(
                name: "IX_paragraph_style_name",
                table: "paragraph_style",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_picture_style_name",
                table: "picture_style",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_picture_style_paragraph_style",
                table: "picture_style",
                column: "paragraph_style");

            migrationBuilder.CreateIndex(
                name: "IX_position_position",
                table: "position",
                column: "position",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_table_style_alignment",
                table: "table_style",
                column: "alignment");

            migrationBuilder.CreateIndex(
                name: "IX_table_style_name",
                table: "table_style",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_table_style_paragraph_style",
                table: "table_style",
                column: "paragraph_style");

            migrationBuilder.CreateIndex(
                name: "IX_table_style_text_style",
                table: "table_style",
                column: "text_style");

            migrationBuilder.CreateIndex(
                name: "IX_template_formula_style",
                table: "template",
                column: "formula_style");

            migrationBuilder.CreateIndex(
                name: "IX_template_global_style",
                table: "template",
                column: "global_style");

            migrationBuilder.CreateIndex(
                name: "IX_template_marked_numbering_style",
                table: "template",
                column: "marked_numbering_style");

            migrationBuilder.CreateIndex(
                name: "IX_template_name",
                table: "template",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_template_numbered_numbering_style",
                table: "template",
                column: "numbered_numbering_style");

            migrationBuilder.CreateIndex(
                name: "IX_template_paragraph_style",
                table: "template",
                column: "paragraph_style");

            migrationBuilder.CreateIndex(
                name: "IX_template_picture_style",
                table: "template",
                column: "picture_style");

            migrationBuilder.CreateIndex(
                name: "IX_template_table_style",
                table: "template",
                column: "table_style");

            migrationBuilder.CreateIndex(
                name: "IX_template_text_style",
                table: "template",
                column: "text_style");

            migrationBuilder.CreateIndex(
                name: "IX_text_style_font",
                table: "text_style",
                column: "font");

            migrationBuilder.CreateIndex(
                name: "IX_text_style_name",
                table: "text_style",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "template");

            migrationBuilder.DropTable(
                name: "formula_style");

            migrationBuilder.DropTable(
                name: "global_style");

            migrationBuilder.DropTable(
                name: "numbering_style");

            migrationBuilder.DropTable(
                name: "picture_style");

            migrationBuilder.DropTable(
                name: "table_style");

            migrationBuilder.DropTable(
                name: "position");

            migrationBuilder.DropTable(
                name: "marker");

            migrationBuilder.DropTable(
                name: "paragraph_style");

            migrationBuilder.DropTable(
                name: "text_style");

            migrationBuilder.DropTable(
                name: "marker_type");

            migrationBuilder.DropTable(
                name: "alignment");

            migrationBuilder.DropTable(
                name: "font");
        }
    }
}
