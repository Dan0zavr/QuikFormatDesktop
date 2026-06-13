using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace QuikFormatDesktop.ViewModels.Enums
{
    public enum StyleType
    {
        [Description("Нет стиля")] None,
        [Description("Стили шрифта")] Text,
        [Description("Стили абзаца")] Paragraph,
        [Description("Стили маркированных списков")] MarkedNumbering,
        [Description("Стили нумерованноых списков")] NumberedNumbering,
        [Description("Стили таблиц")] Table,
        [Description("Стили картинок")] Picture,
        [Description("Стили формул")] Formula,
        [Description("Шаблоны")] Template,
        [Description("Втроенные шаблоны")] SystemTemplate,
        [Description("Глобальные стили")] Global
    }
}
