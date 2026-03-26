using QuikFormatDesktop.Models;
using QuikFormatDesktop.ViewModels.Commands;
using QuikFormatDesktop.ViewModels.Navigation;
using QuikFormatDesktop.ViewModels.Services;
using QuikFormatDesktop.ViewModels.StylesViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;

namespace QuikFormatDesktop.ViewModels.ShortMenuViewModels
{
    public class TextShortMenuViewModel : ViewModelBase
    {
        private TextStyle _textStyle;
        private readonly TextService _textService;
        private readonly FontService _fontService;

        private readonly IServiceProvider _provider;
        private readonly NavigationStore _navigationStore;

        public TextShortMenuViewModel(TextService textService, FontService fontService, IServiceProvider provider, NavigationStore navigationStore)
        {
            _textService = textService;
            _fontService = fontService;
            _provider = provider;
            _navigationStore = navigationStore;
            DeleteTextStyleCommand = new AsyncRelayCommand(DeleteTextStyle, CanDelete);
            DetailCommand = new GoToDetailsCommand<TextStyleViewModel>(_provider, _navigationStore);
        }

        public TextStyle Style => _textStyle;
        public string Name => _textStyle.Name;
        public Font Font
        {
            get
            {
                return _fontService.GetById(_textStyle.Font).GetAwaiter().GetResult();
            }
        }
        public int FontSize => _textStyle.FontSize;

        public ICommand DeleteTextStyleCommand { get; }
        public ICommand DetailCommand { get; }

        private bool CanDelete(object? parametr)
        {
            return true;
        }

        private async Task DeleteTextStyle(object? parametr)
        {
            await _textService.Delete(_textStyle);
        }

        public void Load(TextStyle style)
        {
            if (style != null)
            {
                _textStyle = style;
            }
        }

    }
}
