using QuikFormatDesktop.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Windows.Data;

namespace QuikFormatDesktop.Views.Converters
{
    public class StyleTypeToDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Если пришла строка (как Name группы), пробуем распарсить в StyleType
            if (value is string enumString && Enum.TryParse<StyleType>(enumString, out var styleType))
            {
                return GetDescription(styleType);
            }
            // Если пришло само значение enum
            if (value is StyleType style)
            {
                return GetDescription(style);
            }
            // На всякий случай возвращаем строковое представление
            return value?.ToString() ?? string.Empty;
        }

        private string GetDescription(StyleType value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field?.GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Description ?? value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
