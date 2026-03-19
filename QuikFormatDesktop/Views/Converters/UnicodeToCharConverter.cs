using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace QuikFormatDesktop.View.Converters
{
    public class UnicodeToCharConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string unicode)
            {
                // Поддержка формата \uXXXX
                if (unicode.StartsWith("\\u", StringComparison.OrdinalIgnoreCase))
                {
                    string hex = unicode.Substring(2);
                    if (int.TryParse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int code))
                        return char.ConvertFromUtf32(code);
                }

                // Поддержка формата &#NNNN;
                if (unicode.StartsWith("&#") && unicode.EndsWith(";"))
                {
                    string number = unicode.TrimStart('&', '#').TrimEnd(';');
                    if (int.TryParse(number, out int code))
                        return char.ConvertFromUtf32(code);
                }

                // Возврат как есть
                return unicode;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
