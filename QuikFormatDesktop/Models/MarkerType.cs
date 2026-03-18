using System;
using System.Collections.Generic;

namespace QuikFormatDesktop.Models;

public partial class MarkerType
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public virtual ICollection<Marker> Markers { get; set; } = new List<Marker>();
}
