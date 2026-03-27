using System;
using System.Collections.Generic;

namespace QuikFormatDesktop.Models;

public partial class Position
{
    public int Id { get; set; }

    public string Position1 { get; set; } = null!;

    public virtual ICollection<FormulaStyle> FormulaStyles { get; set; } = new List<FormulaStyle>();
}
