using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using QuikFormatDesktop.Models;
using QuikFormatDesktop.ViewModels.Commands;
using QuikFormatDesktop.ViewModels.Commands.ModalCommands;
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
    public class TextShortMenuViewModel : ShortMenuViewModelBase
    {
        private TextStyle _textStyle;
        private readonly TextService _textService;
        private readonly FontService _fontService;

        private readonly IServiceProvider _provider;
        private readonly NavigationStore _navigationStore;
        private ModalNavigationService<DeleteWarningViewModel> _warningService;

        public TextShortMenuViewModel(TextService textService, FontService fontService, IServiceProvider provider, NavigationStore navigationStore, ModalNavigationService<DeleteWarningViewModel> warningService)
        {
            _textService = textService;
            _fontService = fontService;
            _provider = provider;
            _navigationStore = navigationStore;
            _warningService = warningService;
            DeleteTextStyleCommand = new RelayCommand<object?>(OpenDeleteWarning);
            DetailCommand = new RelayCommand<object?>(GoToDetailsWithAction);
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

        private async Task DeleteTextStyle(object? parametr)
        {
            await _textService.Delete(_textStyle);
            ClosePopup?.Invoke();
        }

        public void Load(TextStyle style)
        {
            if (style != null)
            {
                _textStyle = style;
            }
        }

        private void GoToDetailsWithAction(object? parameter)
        {
            new GoToDetailsCommand<FontStyleViewModel>(_provider, _navigationStore).Execute(parameter);
            ClosePopup?.Invoke();
        }

        private void OpenDeleteWarning(object? parameter)
        {
            new OpenDeleteWarningCommand(_warningService).Execute(parameter);
            if (_navigationStore.CurrentModalViewModel is DeleteWarningViewModel deleteWarning)
            {
                ClosePopup?.Invoke();
                deleteWarning.Load(_textStyle);
                deleteWarning.DeleteCommand = new AsyncRelayCommand<object?>(DeleteTextStyle);
            }
        }
    }
}
