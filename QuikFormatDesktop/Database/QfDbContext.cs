using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using QuikFormatDesktop.Models;
using QuikFormatDesktop.ViewModels.Enums;

namespace QuikFormatDesktop.Database;

public partial class QfDbContext : DbContext
{
    public QfDbContext()
    {
    }

    public QfDbContext(DbContextOptions<QfDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Alignment> Alignments { get; set; }

    public virtual DbSet<Font> Fonts { get; set; }

    public virtual DbSet<FormulaStyle> FormulaStyles { get; set; }

    public virtual DbSet<Marker> Markers { get; set; }

    public virtual DbSet<MarkerType> MarkerTypes { get; set; }

    public virtual DbSet<NumberingStyle> NumberingStyles { get; set; }

    public virtual DbSet<ParagraphStyle> ParagraphStyles { get; set; }

    public virtual DbSet<PictureStyle> PictureStyles { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<TableStyle> TableStyles { get; set; }

    public virtual DbSet<Template> Templates { get; set; }

    public virtual DbSet<TextStyle> TextStyles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Alignment>(entity =>
        {
            entity.ToTable("alignment");

            entity.HasIndex(e => e.Alignment1, "IX_alignment_alignment").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnName("id");
            entity.Property(e => e.Alignment1)
                .HasColumnType("varchar(6)")
                .HasColumnName("alignment");
        });

        modelBuilder.Entity<Font>(entity =>
        {
            entity.ToTable("font");

            entity.HasIndex(e => e.FontName, "IX_font_font_name").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnName("id");
            entity.Property(e => e.FontName)
                .HasColumnType("varchar(32)")
                .HasColumnName("font_name");
        });

        modelBuilder.Entity<FormulaStyle>(entity =>
        {
            entity.ToTable("formula_style");

            entity.HasIndex(e => e.Name, "IX_formula_style_name").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnName("id");
            entity.Property(e => e.EmptyLineAround)
                .HasColumnType("bool")
                .HasColumnName("empty_line_around");
            entity.Property(e => e.Marker).HasColumnName("marker");
            entity.Property(e => e.Name)
                .HasColumnType("varchar(32)")
                .HasColumnName("name");
            entity.Property(e => e.Numeration)
                .HasColumnType("bool")
                .HasColumnName("numeration");
            entity.Property(e => e.Position).HasColumnName("position");

            entity.Ignore(e => e.Type);

            entity.HasOne(d => d.MarkerNavigation).WithMany(p => p.FormulaStyles)
                .HasForeignKey(d => d.Marker)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.PositionNavigation).WithMany(p => p.FormulaStyles).HasForeignKey(d => d.Position);
        });

        modelBuilder.Entity<Marker>(entity =>
        {
            entity.ToTable("marker");

            entity.HasIndex(e => e.Marker1, "IX_marker_marker").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnName("id");
            entity.Property(e => e.Marker1)
                .HasColumnType("varchar(8)")
                .HasColumnName("marker");
            entity.Property(e => e.MarkerType)
                .HasColumnType("INT")
                .HasColumnName("marker_type");

            entity.HasOne(d => d.MarkerTypeNavigation).WithMany(p => p.Markers).HasForeignKey(d => d.MarkerType);
        });

        modelBuilder.Entity<MarkerType>(entity =>
        {
            entity.ToTable("marker_type");

            entity.HasIndex(e => e.Type, "IX_marker_type_type").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnName("id");
            entity.Property(e => e.Type)
                .HasColumnType("varchar(13)")
                .HasColumnName("type");
        });

        modelBuilder.Entity<NumberingStyle>(entity =>
        {
            entity.ToTable("numbering_style");

            entity.HasIndex(e => e.Name, "IX_numbering_style_name").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnName("id");
            entity.Property(e => e.Marker).HasColumnName("marker");
            entity.Property(e => e.Name)
                .HasColumnType("varchar(32)")
                .HasColumnName("name");

            entity.Ignore(e => e.Type);

            entity.HasOne(d => d.MarkerNavigation).WithMany(p => p.NumberingStyles).HasForeignKey(d => d.Marker);
        });

        modelBuilder.Entity<ParagraphStyle>(entity =>
        {
            entity.ToTable("paragraph_style");

            entity.HasIndex(e => e.Name, "IX_paragraph_style_name").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnName("id");
            entity.Property(e => e.AfterInterval).HasColumnName("after_interval");
            entity.Property(e => e.Alignment).HasColumnName("alignment");
            entity.Property(e => e.BeforeInterval).HasColumnName("before_interval");
            entity.Property(e => e.ContextualSpacing)
                .HasColumnType("boolean")
                .HasColumnName("contextual_spacing");
            entity.Property(e => e.FirstLineIndent).HasColumnName("first_line_indent");
            entity.Property(e => e.IntervalInText).HasColumnName("interval_in_text");
            entity.Property(e => e.LeftIndent).HasColumnName("left_indent");
            entity.Property(e => e.Name)
                .HasColumnType("varchar(32)")
                .HasColumnName("name");
            entity.Property(e => e.RightIndent).HasColumnName("right_indent");

            entity.Ignore(e => e.Type);
        });

        modelBuilder.Entity<PictureStyle>(entity =>
        {
            entity.ToTable("picture_style");

            entity.HasIndex(e => e.Name, "IX_picture_style_name").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnName("id");
            entity.Property(e => e.EmptyLineAround)
                .HasColumnType("bool")
                .HasColumnName("empty_line_around");
            entity.Property(e => e.GenerateLabel)
                .HasColumnType("boolean")
                .HasColumnName("generate_label");
            entity.Property(e => e.LabelValue)
                .HasColumnType("varchar(16)")
                .HasColumnName("label_value");
            entity.Property(e => e.Name)
                .HasColumnType("varchar(32)")
                .HasColumnName("name");
            entity.Property(e => e.ParagraphStyle).HasColumnName("paragraph_style");

            entity.Ignore(e => e.Type);

            entity.HasOne(d => d.ParagraphStyleNavigation).WithMany(p => p.PictureStyles)
                .HasForeignKey(d => d.ParagraphStyle)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.ToTable("position");

            entity.HasIndex(e => e.Position1, "IX_position_position").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnName("id");
            entity.Property(e => e.Position1)
                .HasColumnType("varchar(11)")
                .HasColumnName("position");
        });

        modelBuilder.Entity<TableStyle>(entity =>
        {
            entity.ToTable("table_style");

            entity.HasIndex(e => e.Name, "IX_table_style_name").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnName("id");
            entity.Property(e => e.Alignment).HasColumnName("alignment");
            entity.Property(e => e.BorderColor)
                .HasColumnType("char(7)")
                .HasColumnName("border_color");
            entity.Property(e => e.BorderThikness).HasColumnName("border_thikness");
            entity.Property(e => e.CellPadding).HasColumnName("cell_padding");
            entity.Property(e => e.Name)
                .HasColumnType("varchar(32)")
                .HasColumnName("name");
            entity.Property(e => e.ParagraphStyle).HasColumnName("paragraph_style");
            entity.Property(e => e.TextStyle).HasColumnName("text_style");

            entity.Ignore(e => e.Type);

            entity.HasOne(d => d.AlignmentNavigation).WithMany(p => p.TableStyles).HasForeignKey(d => d.Alignment);

            entity.HasOne(d => d.ParagraphStyleNavigation).WithMany(p => p.TableStyles).HasForeignKey(d => d.ParagraphStyle);

            entity.HasOne(d => d.TextStyleNavigation).WithMany(p => p.TableStyles).HasForeignKey(d => d.TextStyle);
        });

        modelBuilder.Entity<Template>(entity =>
        {
            entity.ToTable("template");

            entity.HasIndex(e => e.Name, "IX_template_name").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.FormulaStyle).HasColumnName("formula_style");
            entity.Property(e => e.MarkedNumberingStyle).HasColumnName("marked_numbering_style");
            entity.Property(e => e.Name)
                .HasColumnType("varchar(32)")
                .HasColumnName("name");
            entity.Property(e => e.NumberedNumberingStyle).HasColumnName("numbered_numbering_style");
            entity.Property(e => e.ParagraphStyle).HasColumnName("paragraph_style");
            entity.Property(e => e.PictureStyle).HasColumnName("picture_style");
            entity.Property(e => e.TableStyle).HasColumnName("table_style");
            entity.Property(e => e.TextStyle).HasColumnName("text_style");

            entity.Ignore(e => e.Type);

            entity.HasOne(d => d.FormulaStyleNavigation).WithMany(p => p.Templates).HasForeignKey(d => d.FormulaStyle);

            entity.HasOne(d => d.MarkedNumberingStyleNavigation).WithMany(p => p.TemplateMarkedNumberingStyleNavigations).HasForeignKey(d => d.MarkedNumberingStyle);

            entity.HasOne(d => d.NumberedNumberingStyleNavigation).WithMany(p => p.TemplateNumberedNumberingStyleNavigations).HasForeignKey(d => d.NumberedNumberingStyle);

            entity.HasOne(d => d.ParagraphStyleNavigation).WithMany(p => p.Templates).HasForeignKey(d => d.ParagraphStyle);

            entity.HasOne(d => d.PictureStyleNavigation).WithMany(p => p.Templates).HasForeignKey(d => d.PictureStyle);

            entity.HasOne(d => d.TableStyleNavigation).WithMany(p => p.Templates).HasForeignKey(d => d.TableStyle);

            entity.HasOne(d => d.TextStyleNavigation).WithMany(p => p.TemplateTextStyleNavigations).HasForeignKey(d => d.TextStyle);
        });

        modelBuilder.Entity<TextStyle>(entity =>
        {
            entity.ToTable("text_style");

            entity.HasIndex(e => e.Name, "IX_text_style_name").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnName("id");
            entity.Property(e => e.Font).HasColumnName("font");
            entity.Property(e => e.FontSize).HasColumnName("font_size");
            entity.Property(e => e.Name)
                .HasColumnType("varchar(32)")
                .HasColumnName("name");

            entity.Ignore(e => e.Type);

            entity.HasOne(d => d.FontNavigation).WithMany(p => p.TextStyles).HasForeignKey(d => d.Font);
        });

        modelBuilder.Entity<Position>().HasData(
            new Position { Id = 1, Position1 = PositionType.CenterLeft.ToString().ToLower()},
            new Position { Id = 2, Position1 = PositionType.CenterRight.ToString().ToLower()},
            new Position { Id = 3, Position1 = PositionType.RightLeft.ToString().ToLower()},
            new Position { Id = 4, Position1 = PositionType.LeftRight.ToString().ToLower()}
            );

        modelBuilder.Entity<Alignment>().HasData(
            new Alignment { Id = 1, Alignment1 = AlignmentType.Left.ToString().ToLower()},
            new Alignment { Id = 2, Alignment1 = AlignmentType.Right.ToString().ToLower()},
            new Alignment { Id = 3, Alignment1 = AlignmentType.Center.ToString().ToLower()},
            new Alignment { Id = 4, Alignment1 = AlignmentType.Both.ToString().ToLower()},
            new Alignment { Id = 5, Alignment1 = AlignmentType.Top.ToString().ToLower()},
            new Alignment { Id = 6, Alignment1 = AlignmentType.Bottom.ToString().ToLower()}
        );

        modelBuilder.Entity<MarkerType>().HasData(
            new MarkerType { Id = 1, Type = MarkerTypeEnum.Marked.ToString().ToLower() },
            new MarkerType { Id = 2, Type = MarkerTypeEnum.Numberd.ToString().ToLower() });

        modelBuilder.Entity<Marker>().HasData(
            new Marker { Id = 1, Marker1 = "&#8211;", MarkerType = 1 },
            new Marker { Id = 2, Marker1 = "&#8226;", MarkerType = 1 },
            new Marker { Id = 3, Marker1 = "&#8227;", MarkerType = 1 },
            new Marker { Id = 4, Marker1 = "$.", MarkerType = 2 },
            new Marker { Id = 5, Marker1 = "$)", MarkerType = 2 });

        modelBuilder.Entity<Font>().HasData(
        new Font { Id = 1, FontName = "Arial" },
        new Font { Id = 2, FontName = "Times New Roman" },
        new Font { Id = 3, FontName = "Calibri" });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
