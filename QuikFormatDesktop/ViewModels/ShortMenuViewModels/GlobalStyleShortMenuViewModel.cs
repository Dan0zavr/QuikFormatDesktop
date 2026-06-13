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
using System.Text;
using System.Windows.Input;

namespace QuikFormatDesktop.ViewModels.ShortMenuViewModels
{
    public class GlobalStyleShortMenuViewModel : ShortMenuViewModelBase
    {
        private GlobalStyle _globalStyle;
        private readonly GlobalStyleService _globalStyleService;
        private readonly AlignmentService _alignmentService;

        private readonly IServiceProvider _provider;
        private readonly NavigationStore _navigationStore;
        private ModalNavigationService<DeleteWarningViewModel> _warningService;

        public GlobalStyleShortMenuViewModel(GlobalStyleService globalStyleService, AlignmentService alignmentService,
            IServiceProvider provider, NavigationStore navigationStore, ModalNavigationService<DeleteWarningViewModel> warningService)
        {
            _globalStyleService = globalStyleService;
            _alignmentService = alignmentService;
            _provider = provider;
            _navigationStore = navigationStore;
            _warningService = warningService;

            DeleteGlobalStyleCommand = new RelayCommand<object?>(OpenDeleteWarning);
            DetailCommand = new RelayCommand<object?>(GoToDetailsWithAction);
        }

        public ICommand DeleteGlobalStyleCommand { get; }
        public ICommand DetailCommand { get; }

        public GlobalStyle Style => _globalStyle;
        public string Name => _globalStyle.Name;
        public string AlignmentName => _alignmentService.GetById(_globalStyle.AlignmentId).GetAwaiter().GetResult().Alignment1;
        public double LeftMargin => _globalStyle.LeftMargin;
        public double RightMargin => _globalStyle.RightMargin;
        public double TopMargin => _globalStyle.TopMargin;
        public double BottomMargin => _globalStyle.BottomMargin;

        private async Task DeleteGlobalStyle(object? parametr)
        {
            await _globalStyleService.Delete(_globalStyle);
            ClosePopup?.Invoke();
        }

        public void Load(GlobalStyle style)
        {
            if (style != null)
            {
                _globalStyle = style;
            }
        }

        private void GoToDetailsWithAction(object? parameter)
        {
            new GoToDetailsCommand<GlobalStyleViewModel>(_provider, _navigationStore).Execute(parameter);
            ClosePopup?.Invoke();

        }

        private void OpenDeleteWarning(object? parameter)
        {
            new OpenDeleteWarningCommand(_warningService).Execute(parameter);
            if (_navigationStore.CurrentModalViewModel is DeleteWarningViewModel deleteWarning)
            {
                ClosePopup?.Invoke();
                deleteWarning.Load(_globalStyle);
                deleteWarning.DeleteCommand = new AsyncRelayCommand<object?>(DeleteGlobalStyle);
            }
        }
    }
}
