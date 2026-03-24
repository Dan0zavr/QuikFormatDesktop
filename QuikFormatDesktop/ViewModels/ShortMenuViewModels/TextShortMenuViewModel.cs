using QuikFormatDesktop.Models;
using QuikFormatDesktop.ViewModels.Commands;
using QuikFormatDesktop.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;

namespace QuikFormatDesktop.ViewModels.ShortMenuViewModels
{
    public class TextShortMenuViewModel : ViewModelBase
    {
        private readonly TextStyle _textStyle;
        private readonly TextService _textService;
        private readonly FontService _fontService;

        public TextShortMenuViewModel(TextStyle textStyle, TextService textService, FontService fontService)
        {
            _textStyle = textStyle;
            _textService = textService;
            _fontService = fontService;
            DeleteTextStyleCommand = new AsyncRelayCommand(DeleteTextStyle, CanDelete);
            
        }

        public string Name => _textStyle.Name;
        public string FontName
        {
            get
            {
                return _fontService.GetById(_textStyle.Font).GetAwaiter().GetResult()?.FontName;
            }
        }
        public int FontSize => _textStyle.FontSize;

        public ICommand DeleteTextStyleCommand { get; }


        private bool CanDelete(object? parametr)
        {
            return true;
        }

        private async Task DeleteTextStyle(object? parametr)
        {
            await _textService.Delete(_textStyle);
        }

    }
}
