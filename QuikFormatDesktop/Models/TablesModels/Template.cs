using System;
using System.Collections.Generic;

namespace QuikFormatDesktop.Models;

public partial class Template : StyleObject
{

    public string? Description { get; set; }

    public int? TextStyle { get; set; }

    public int? ParagraphStyle { get; set; }

    public int? TableStyle { get; set; }

    public int? PictureStyle { get; set; }

    public int? FormulaStyle { get; set; }

    public int? MarkedNumberingStyle { get; set; }

    public int? NumberedNumberingStyle { get; set; }

    public virtual FormulaStyle? FormulaStyleNavigation { get; set; }

    public virtual TextStyle? MarkedNumberingStyleNavigation { get; set; }

    public virtual TextStyle? NumberedNumberingStyleNavigation { get; set; }

    public virtual ParagraphStyle? ParagraphStyleNavigation { get; set; }

    public virtual PictureStyle? PictureStyleNavigation { get; set; }

    public virtual TableStyle? TableStyleNavigation { get; set; }

    public virtual TextStyle? TextStyleNavigation { get; set; }
}
