using QuikFormatDesktop.Models;
using QuikFormatDesktop.ViewModels.Commands;
using QuikFormatDesktop.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace QuikFormatDesktop.ViewModels.ShortMenuViewModels
{
    public class NumberingShortMenuViewModel : ViewModelBase
    {
        private readonly NumberingStyle _numberingStyle;
        private readonly NumberingService _numberingService;
        private readonly MarkerService _markerService;
        public NumberingShortMenuViewModel(NumberingStyle numberingStyle, NumberingService numberingService, MarkerService markerService)
        {
            _numberingStyle = numberingStyle;
            _numberingService = numberingService;
            _markerService = markerService;

            DeleteNumberingStyleCommand = new AsyncRelayCommand(DeleteNumberingStyle, CanDelete);
        }

        public ICommand DeleteNumberingStyleCommand { get; }

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
    }
}
