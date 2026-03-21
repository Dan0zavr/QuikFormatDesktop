using System;
using System.Collections.Generic;

namespace QuikFormatDesktop.Models;

public partial class PictureStyle : StyleObject
{
    public int ParagraphStyle { get; set; }

    public bool GenerateLabel { get; set; }

    public string? LabelValue { get; set; }

    public bool EmptyLineAround { get; set; }

    public virtual ParagraphStyle ParagraphStyleNavigation { get; set; } = null!;

    public virtual ICollection<Template> Templates { get; set; } = new List<Template>();
}
