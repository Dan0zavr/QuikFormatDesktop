using System;
using System.Collections.Generic;

namespace QuikFormatDesktop.Models;

public partial class NumberingStyle : StyleObject
{
    public int Marker { get; set; }

    public virtual Marker MarkerNavigation { get; set; } = null!;

    public virtual ICollection<Template> TemplateMarkedNumberingStyleNavigations { get; set; } = new List<Template>();

    public virtual ICollection<Template> TemplateNumberedNumberingStyleNavigations { get; set; } = new List<Template>();
}
