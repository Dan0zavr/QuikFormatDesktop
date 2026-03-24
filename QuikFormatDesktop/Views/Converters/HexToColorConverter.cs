using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace QuikFormatDesktop.Views.Converters
{
    public class HexToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // string? → Color? (для отображения в ColorPicker)
            if (value is string hexColor && !string.IsNullOrEmpty(hexColor))
            {
                try
                {
                    return (Color)ColorConverter.ConvertFromString(hexColor);
                }
                catch
                {
                    return Colors.Black; // или null, если допустимо
                }
            }
            return null; // или Colors.Transparent, если нужно
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Color? → string? (для сохранения в ViewModel)
            if (value is Color color)
            {
                return $"#{color.R:X2}{color.G:X2}{color.B:X2}"; // Формат #RRGGBB
            }
            return null;
        }
    }
}
