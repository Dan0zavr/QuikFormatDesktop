using System;
using System.Collections.Generic;

namespace QuikFormatDesktop.Models;

public partial class NumberingStyle : StyleObject
{
    public int Marker { get; set; }

    public virtual Marker MarkerNavigation { get; set; } = null!;
}
