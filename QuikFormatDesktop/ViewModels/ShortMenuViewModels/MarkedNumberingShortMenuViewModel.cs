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
    public class MarkedNumberingShortMenuViewModel : ViewModelBase
    {
        private NumberingStyle _numberingStyle;
        private readonly NumberingService _numberingService;
        private readonly MarkerService _markerService;

        private readonly IServiceProvider _provider;
        private readonly NavigationStore _navigationStore;
        public MarkedNumberingShortMenuViewModel(NumberingService numberingService,
            MarkerService markerService, IServiceProvider provider, NavigationStore navigationStore)
        {
            _numberingService = numberingService;
            _markerService = markerService;

            _provider = provider;
            _navigationStore = navigationStore;

            DeleteNumberingStyleCommand = new AsyncRelayCommand(DeleteNumberingStyle, CanDelete);
            DetailCommand = new GoToDetailsCommand<NumberingStyleViewModel>(_provider, _navigationStore);
            
        }

        public ICommand DeleteNumberingStyleCommand { get; }
        public ICommand DetailCommand { get; }

        public NumberingStyle Style => _numberingStyle; 
        public string Name => _numberingStyle.Name;
        public string Marker => _markerService.GetById(_numberingStyle.Marker).GetAwaiter().GetResult().Marker1;

        private bool CanDelete(object? parametr)
        {
            return true;
        }

        private async Task DeleteNumberingStyle(object? parametr)
        {
            await _numberingService.Delete(_numberingStyle);
        }

        public void Load(NumberingStyle style)
        {
            if (style != null)
            {
                _numberingStyle = style;
            }
        }
    }
}
