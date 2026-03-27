using System;
using System.Collections.Generic;

namespace QuikFormatDesktop.Models;

public partial class TableStyle : StyleObject
{
    public int TextStyle { get; set; }

    public int ParagraphStyle { get; set; }

    public int Alignment { get; set; }

    public int BorderThikness { get; set; }

    public string BorderColor { get; set; } = null!;

    public double CellPadding { get; set; }

    public virtual Alignment AlignmentNavigation { get; set; } = null!;

    public virtual ParagraphStyle ParagraphStyleNavigation { get; set; } = null!;

    public virtual ICollection<Template> Templates { get; set; } = new List<Template>();

    public virtual TextStyle TextStyleNavigation { get; set; } = null!;
}
