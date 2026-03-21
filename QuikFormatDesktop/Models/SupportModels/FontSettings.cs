using System;
using System.Collections.Generic;
using System.Text;

namespace QuikFormatDesktop.Models.SupportModels
{
    public class FontSettings
    {
        public string DefaultName { get; set; }
        public int DefaultSize { get; set; }
        public List<int> AllowedSizes { get; set; }
    }
}
