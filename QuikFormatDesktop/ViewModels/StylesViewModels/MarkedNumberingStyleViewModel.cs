using QuikFormatDesktop.Models;
using QuikFormatDesktop.ViewModels.Commands.TextViewModelCommands.NumberingStyleCommand;
using QuikFormatDesktop.ViewModels.Enums;
using QuikFormatDesktop.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace QuikFormatDesktop.ViewModels.StylesViewModels
{
    public class MarkedNumberingStyleViewModel : ViewModelBase, INumbering
    {
        public readonly NumberingService numberingService;
        public readonly MarkerService markerService;
        public readonly IDialogService dialogService;

        private string _numberingStyleName;
        private List<Marker> _markers = new List<Marker>();
        private Marker _selectedMarker;
        private string _pStatusMessage;

        public MarkedNumberingStyleViewModel(NumberingService DiNumberingService, MarkerService DiMarkerService, IDialogService DiDialogService)
        {
            numberingService = DiNumberingService;
            markerService = DiMarkerService;
            dialogService = DiDialogService;
            LoadMarkers();

            AddNumberingCommand = new AddNumberingStyleCommand(this, numberingService, dialogService);
        }

        public ICommand NumberingDeleteCommand { get; }
        public ICommand AddNumberingCommand { get; }

        public string NumberingStyleName
        {
            get => _numberingStyleName;
            set
            {
                _numberingStyleName = value;
                OnPropertyChanged(nameof(NumberingStyleName));
                (AddNumberingCommand as AddNumberingStyleCommand)?.RaiseCanExecuteChanged();
            }
        }

        public List<Marker> Markers
        {
            get => _markers;
            set
            {
                _markers = value;
                OnPropertyChanged(nameof(Markers));
            }
        }

        public Marker SelectedMarker
        {
            get => _selectedMarker;
            set
            {
                _selectedMarker = value;
                OnPropertyChanged(nameof(SelectedMarker));
            }
        }

        public string PStatusMessage
        {
            get => _pStatusMessage;
            set
            {
                _pStatusMessage = value;
                OnPropertyChanged(nameof(PStatusMessage));
            }
        }

        public MarkerTypeEnum MarkerType => MarkerTypeEnum.Marked;

        private async Task LoadMarkers()
        {
            _markers.Clear();
            _markers = await markerService.GetByType(MarkerTypeEnum.Marked);
            _selectedMarker = _markers.FirstOrDefault();
        }
    }
}
