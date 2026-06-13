using System;
using System.Collections.Generic;
using System.Text;

namespace QuikFormatDesktop.Models;
public class GlobalStyle : StyleObject
{
    public double LeftMargin { get; set; } = 3.0;
    public double RightMargin { get; set; } = 1.5;
    public double TopMargin { get; set; } = 2.0;
    public double BottomMargin { get; set; } = 2.0;

    public string? SpecialColontitul = null;
    public int? LastNoNumberingPage = null;
    public int AlignmentId { get; set; }
    public virtual Alignment AlignmentNavigation { get; set; } = null;
    public virtual ICollection<Template> TemplateGlobalStyleNavigations { get; set; } = new List<Template>();
}
