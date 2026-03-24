using QuikFormatDesktop.Models;
using QuikFormatDesktop.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace QuikFormatDesktop.ViewModels.ShortMenuViewModels
{
    public class TextShortMenuViewModel : ViewModelBase
    {
        private readonly TextStyle _textStyle;

        public TextShortMenuViewModel(TextStyle textStyle)
        {
            _textStyle = textStyle;
        }

        public string Name => _textStyle.Name;
        public string FontName => _textStyle.FontNavigation.FontName;
        public int FontSize => _textStyle.FontSize;

    }
}
