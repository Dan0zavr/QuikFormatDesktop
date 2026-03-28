using System;
using System.Collections.Generic;

namespace QuikFormatDesktop.Models;

public partial class TextStyle : StyleObject
{   
    public int Font { get; set; }

    public int FontSize { get; set; }

    public virtual Font FontNavigation { get; set; } = null!;

    public virtual ICollection<TableStyle> TableStyles { get; set; } = new List<TableStyle>();

    public virtual ICollection<Template> TemplateTextStyleNavigations { get; set; } = new List<Template>();
}
