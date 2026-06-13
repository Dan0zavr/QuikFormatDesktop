using System;
using System.Collections.Generic;
using System.Text;

namespace QuikFormatDesktop.Models.SupportModels
{
    public class SystemStyles
    {
        public List<Template> Templates { get; set; }
        public List<TextStyle> TextStyles { get; set; }
        public List<ParagraphStyle> ParagraphStyles { get; set; }
        public List<TableStyle> TableStyles { get; set; }
        public List<PictureStyle> PictureStyles { get; set; }
        public List<FormulaStyle> FormulaStyles { get; set; }
        public List<NumberingStyle> NumberingStyles { get; set; }
        public List<GlobalStyle> GlobalStyles { get; set; }
    }
}
