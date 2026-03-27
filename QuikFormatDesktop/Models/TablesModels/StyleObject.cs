using QuikFormatDesktop.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuikFormatDesktop.Models
{
    public class StyleObject
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public StyleType Type { get; set; }
    }
}
