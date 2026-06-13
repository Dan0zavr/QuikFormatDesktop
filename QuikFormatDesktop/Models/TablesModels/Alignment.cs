using System;
using System.Collections.Generic;

namespace QuikFormatDesktop.Models;

public partial class Alignment
{
    public int Id { get; set; }

    public string Alignment1 { get; set; } = null!;

    public virtual ICollection<TableStyle> TableStyles { get; set; } = new List<TableStyle>();
    public virtual ICollection<ParagraphStyle> ParagraphStyles { get; set; } = new List<ParagraphStyle>();
    public virtual ICollection<GlobalStyle> GlobalStyles { get; set; } = new List<GlobalStyle>();
}
