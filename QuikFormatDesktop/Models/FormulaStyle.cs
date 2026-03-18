using System;
using System.Collections.Generic;

namespace QuikFormatDesktop.Models;

public partial class FormulaStyle
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool Numeration { get; set; }

    public bool EmptyLineAround { get; set; }

    public int? Marker { get; set; }

    public int Position { get; set; }

    public virtual Marker? MarkerNavigation { get; set; }

    public virtual Position PositionNavigation { get; set; } = null!;

    public virtual ICollection<Template> Templates { get; set; } = new List<Template>();
}
