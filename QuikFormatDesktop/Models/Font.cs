using System;
using System.Collections.Generic;

namespace QuikFormatDesktop.Models;

public partial class Font
{
    public int Id { get; set; }

    public string FontName { get; set; } = null!;

    public virtual ICollection<TextStyle> TextStyles { get; set; } = new List<TextStyle>();
}
