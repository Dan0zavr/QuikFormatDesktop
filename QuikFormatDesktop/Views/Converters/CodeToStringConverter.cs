using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace QuikFormatDesktop.View.Converters
{
    public class CodeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is string code)
            {
                if (code.Contains("$"))
                {
                    code = code.Replace("$", "1");
                    return code;
                }
                return code;
            }
            return value;
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
