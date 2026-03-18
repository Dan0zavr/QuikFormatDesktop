using System;
using System.Collections.Generic;

namespace QuikFormatDesktop.Models;

public partial class NumberingStyle
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Marker { get; set; }

    public virtual Marker MarkerNavigation { get; set; } = null!;
}
