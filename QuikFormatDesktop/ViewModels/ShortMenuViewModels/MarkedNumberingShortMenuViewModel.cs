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
    public class MarkedNumberingShortMenuViewModel : ShortMenuViewModelBase
    {
        private NumberingStyle _numberingStyle;
        private readonly NumberingService _numberingService;
        private readonly MarkerService _markerService;

        private readonly IServiceProvider _provider;
        private readonly NavigationStore _navigationStore;
        private ModalNavigationService<DeleteWarningViewModel> _warningService;
        public MarkedNumberingShortMenuViewModel(NumberingService numberingService,
            MarkerService markerService, IServiceProvider provider, NavigationStore navigationStore, ModalNavigationService<DeleteWarningViewModel> warningService)
        {
            _numberingService = numberingService;
            _markerService = markerService;

            _provider = provider;
            _navigationStore = navigationStore;
            _warningService = warningService;

            DeleteNumberingStyleCommand = new RelayCommand<object?>(OpenDeleteWarning);
            DetailCommand = new RelayCommand<object?>(GoToDetailsWithAction);
        }

        public ICommand DeleteNumberingStyleCommand { get; }
        public ICommand DetailCommand { get; }

        public NumberingStyle Style => _numberingStyle; 
        public string Name => _numberingStyle.Name;
        public string Marker => _markerService.GetById(_numberingStyle.Marker).GetAwaiter().GetResult().Marker1;

        private async Task DeleteNumberingStyle(object? parametr)
        {
            await _numberingService.Delete(_numberingStyle);
            ClosePopup?.Invoke();
        }

        public void Load(NumberingStyle style)
        {
            if (style != null)
            {
                _numberingStyle = style;
            }
        }

        private void GoToDetailsWithAction(object? parameter)
        {
            new GoToDetailsCommand<MarkedNumberingStyleViewModel>(_provider, _navigationStore).Execute(parameter);
            ClosePopup?.Invoke();
        }

        private void OpenDeleteWarning(object? parameter)
        {
            new OpenDeleteWarningCommand(_warningService).Execute(parameter);
            if (_navigationStore.CurrentModalViewModel is DeleteWarningViewModel deleteWarning)
            {
                ClosePopup?.Invoke();
                deleteWarning.Load(_numberingStyle);
                deleteWarning.DeleteCommand = new AsyncRelayCommand<object?>(DeleteNumberingStyle);
            }
        }
    }
}
