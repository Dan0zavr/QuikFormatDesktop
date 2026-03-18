using System;
using System.Collections.Generic;

namespace QuikFormatDesktop.Models;

public partial class Marker
{
    public int Id { get; set; }

    public string Marker1 { get; set; } = null!;

    public int MarkerType { get; set; }

    public virtual ICollection<FormulaStyle> FormulaStyles { get; set; } = new List<FormulaStyle>();

    public virtual MarkerType MarkerTypeNavigation { get; set; } = null!;

    public virtual ICollection<NumberingStyle> NumberingStyles { get; set; } = new List<NumberingStyle>();
}
