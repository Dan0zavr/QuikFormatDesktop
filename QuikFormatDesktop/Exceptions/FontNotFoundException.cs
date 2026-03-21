using System;
using System.Collections.Generic;
using System.Text;

namespace QuikFormatDesktop.Exceptions
{
    public class FontNotFoundException : Exception
    {
        public FontNotFoundException() : base("Шрифт не найден, попробуйте переустановить приложение") { }
    }
}
