using System;
using System.Collections.Generic;
using System.Text;

namespace QuikFormatDesktop.Exceptions
{
    public class AlignmentNotFoundException : Exception
    {
        public AlignmentNotFoundException() : base("Выравнивание не найдено, попробуйте переустановить приложение") { }
    }
}
