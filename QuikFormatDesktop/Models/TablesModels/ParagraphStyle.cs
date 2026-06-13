using System;
using System.Collections.Generic;

namespace QuikFormatDesktop.Models;

public partial class ParagraphStyle : StyleObject
{
    public int Alignment { get; set; }

    public double? FirstLineIndent { get; set; }

    public double? LeftIndent { get; set; }

    public double? RightIndent { get; set; }

    public double IntervalInText { get; set; }

    public double? BeforeInterval { get; set; }

    public double? AfterInterval { get; set; }

    public bool ContextualSpacing { get; set; }
    public virtual Alignment AlignmentNavigation { get; set; } = null;

    public virtual ICollection<PictureStyle> PictureStyles { get; set; } = new List<PictureStyle>();

    public virtual ICollection<TableStyle> TableStyles { get; set; } = new List<TableStyle>();

    public virtual ICollection<Template> Templates { get; set; } = new List<Template>();
}
