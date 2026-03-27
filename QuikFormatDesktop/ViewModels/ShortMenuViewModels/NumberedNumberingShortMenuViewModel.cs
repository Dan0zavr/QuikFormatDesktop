using QuikFormatDesktop.Models;
using QuikFormatDesktop.ViewModels.Commands;
using QuikFormatDesktop.ViewModels.Navigation;
using QuikFormatDesktop.ViewModels.Services;
using QuikFormatDesktop.ViewModels.StylesViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace QuikFormatDesktop.ViewModels.ShortMenuViewModels
{
    public class NumberedNumberingShortMenuViewModel : ShortMenuViewModelBase
    {
        private NumberingStyle _numberingStyle;
        private readonly NumberingService _numberingService;
        private readonly MarkerService _markerService;

        private readonly IServiceProvider _serviceProvider;
        private readonly NavigationStore _navigationStore;
        public NumberedNumberingShortMenuViewModel(NumberingService numberingService, MarkerService markerService,
            IServiceProvider serviceProvider, NavigationStore navigationStore)
        {
            _numberingService = numberingService;
            _markerService = markerService;

            _serviceProvider = serviceProvider;
            _navigationStore = navigationStore;

            DeleteNumberingStyleCommand = new AsyncRelayCommand(DeleteNumberingStyle);
            DetailCommand = new RelayCommand(GoToDetailsWithAction);
            
        }

        public ICommand DeleteNumberingStyleCommand { get; }
        public ICommand DetailCommand { get; }
        
        public NumberingStyle Style  => _numberingStyle;
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
            new GoToDetailsCommand<NumberedNumberingStyleViewModel>(_serviceProvider, _navigationStore).Execute(parameter);
            ClosePopup?.Invoke();
        }
    }
}
