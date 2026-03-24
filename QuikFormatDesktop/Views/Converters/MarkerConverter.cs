using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace QuikFormatDesktop.Views.Converters
{
    public class MarkerConverter : IValueConverter
    {
        private readonly CodeToStringConverter _codeConverter = new();
        private readonly UnicodeToCharConverter _unicodeConverter = new();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
            {
                // Логика выбора
                if (str.Contains("$"))
                    return _codeConverter.Convert(value, targetType, parameter, culture);

                if (str.StartsWith("\\u") || (str.StartsWith("&#") && str.EndsWith(";")))
                    return _unicodeConverter.Convert(value, targetType, parameter, culture);

                return str;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
