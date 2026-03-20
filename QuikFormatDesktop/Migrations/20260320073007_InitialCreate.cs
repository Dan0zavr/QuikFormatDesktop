using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuikFormatDesktop.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "alignment",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false),
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
                    id = table.Column<int>(type: "INTEGER", nullable: false),
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
                    id = table.Column<int>(type: "INTEGER", nullable: false),
                    type = table.Column<string>(type: "varchar(13)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_marker_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "paragraph_style",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false),
                    name = table.Column<string>(type: "varchar(32)", nullable: false),
                    alignment = table.Column<int>(type: "INTEGER", nullable: false),
                    first_line_indent = table.Column<double>(type: "REAL", nullable: true),
                    left_indent = table.Column<double>(type: "REAL", nullable: true),
                    right_indent = table.Column<double>(type: "REAL", nullable: true),
                    interval_in_text = table.Column<double>(type: "REAL", nullable: false),
                    before_interval = table.Column<double>(type: "REAL", nullable: true),
                    after_interval = table.Column<double>(type: "REAL", nullable: true),
                    contextual_spacing = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_paragraph_style", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "position",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false),
                    position = table.Column<string>(type: "varchar(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_position", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "text_style",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false),
                    name = table.Column<string>(type: "varchar(32)", nullable: false),
                    font = table.Column<int>(type: "INTEGER", nullable: false),
                    font_size = table.Column<int>(type: "INTEGER", nullable: false)
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
                    id = table.Column<int>(type: "INTEGER", nullable: false),
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
                    id = table.Column<int>(type: "INTEGER", nullable: false),
                    name = table.Column<string>(type: "varchar(32)", nullable: false),
                    paragraph_style = table.Column<int>(type: "INTEGER", nullable: false),
                    generate_label = table.Column<bool>(type: "boolean", nullable: false),
                    label_value = table.Column<string>(type: "varchar(16)", nullable: true),
                    empty_line_around = table.Column<bool>(type: "bool", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_picture_style", x => x.id);
                    table.ForeignKey(
                        name: "FK_picture_style_paragraph_style_paragraph_style",
                        column: x => x.paragraph_style,
                        principalTable: "paragraph_style",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "table_style",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false),
                    name = table.Column<string>(type: "varchar(32)", nullable: false),
                    text_style = table.Column<int>(type: "INTEGER", nullable: false),
                    paragraph_style = table.Column<int>(type: "INTEGER", nullable: false),
                    alignment = table.Column<int>(type: "INTEGER", nullable: false),
                    border_thikness = table.Column<int>(type: "INTEGER", nullable: false),
                    border_color = table.Column<string>(type: "char(7)", nullable: false),
                    cell_padding = table.Column<double>(type: "REAL", nullable: false)
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
                    id = table.Column<int>(type: "INTEGER", nullable: false),
                    name = table.Column<string>(type: "varchar(32)", nullable: false),
                    numeration = table.Column<bool>(type: "bool", nullable: false),
                    empty_line_around = table.Column<bool>(type: "bool", nullable: false),
                    marker = table.Column<int>(type: "INTEGER", nullable: true),
                    position = table.Column<int>(type: "INTEGER", nullable: false)
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
                    id = table.Column<int>(type: "INTEGER", nullable: false),
                    name = table.Column<string>(type: "varchar(32)", nullable: false),
                    marker = table.Column<int>(type: "INTEGER", nullable: false)
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
                    id = table.Column<int>(type: "INTEGER", nullable: false),
                    name = table.Column<string>(type: "varchar(32)", nullable: false),
                    description = table.Column<string>(type: "TEXT", nullable: true),
                    text_style = table.Column<int>(type: "INTEGER", nullable: true),
                    paragraph_style = table.Column<int>(type: "INTEGER", nullable: true),
                    table_style = table.Column<int>(type: "INTEGER", nullable: true),
                    picture_style = table.Column<int>(type: "INTEGER", nullable: true),
                    formula_style = table.Column<int>(type: "INTEGER", nullable: true),
                    marked_numbering_style = table.Column<int>(type: "INTEGER", nullable: true),
                    numbered_numbering_style = table.Column<int>(type: "INTEGER", nullable: true),
                    formula = table.Column<int>(type: "style integer", nullable: true)
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
                        name: "FK_template_text_style_marked_numbering_style",
                        column: x => x.marked_numbering_style,
                        principalTable: "text_style",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_template_text_style_numbered_numbering_style",
                        column: x => x.numbered_numbering_style,
                        principalTable: "text_style",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_template_text_style_text_style",
                        column: x => x.text_style,
                        principalTable: "text_style",
                        principalColumn: "id");
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
                name: "numbering_style");

            migrationBuilder.DropTable(
                name: "template");

            migrationBuilder.DropTable(
                name: "formula_style");

            migrationBuilder.DropTable(
                name: "picture_style");

            migrationBuilder.DropTable(
                name: "table_style");

            migrationBuilder.DropTable(
                name: "marker");

            migrationBuilder.DropTable(
                name: "position");

            migrationBuilder.DropTable(
                name: "alignment");

            migrationBuilder.DropTable(
                name: "paragraph_style");

            migrationBuilder.DropTable(
                name: "text_style");

            migrationBuilder.DropTable(
                name: "marker_type");

            migrationBuilder.DropTable(
                name: "font");
        }
    }
}
