using QuikFormatDesktop.Models;
using QuikFormatDesktop.ViewModels.Commands.NumberingStyleCommand;
using QuikFormatDesktop.ViewModels.Enums;
using QuikFormatDesktop.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace QuikFormatDesktop.ViewModels.StylesViewModels
{
    public class MarkedNumberingStyleViewModel : ViewModelBase, INumbering, IResetable, ILoadable
    {
        public readonly NumberingService _numberingService;
        public readonly MarkerService _markerService;
        public readonly IDialogService _dialogService;

        private string _numberingStyleName;
        private List<Marker> _markers = new List<Marker>();
        private Marker _selectedMarker;
        private string _pStatusMessage;

        public MarkedNumberingStyleViewModel(NumberingService numberingService, MarkerService markerService, IDialogService dialogService)
        {
            _numberingService = numberingService;
            _markerService = markerService;
            _dialogService = dialogService;
            LoadMarkers();

            AddNumberingCommand = new AddNumberingStyleCommand(this, numberingService, dialogService);
        }

        public ICommand NumberingDeleteCommand { get; }
        public ICommand AddNumberingCommand { get; }

        public bool IsEdit { get; private set; } = false;

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

        public void Load(object parametr, bool isEdit = false)
        {
            IsEdit = isEdit;

            if (parametr is NumberingStyle numberingStyle)
            {
                NumberingStyleName = numberingStyle.Name;
                SelectedMarker = _markers.Where(x => x.Id == numberingStyle.Marker).FirstOrDefault();
            }
        }

        public void Reset()
        {
            NumberingStyleName = null;
            SelectedMarker = _markers.FirstOrDefault();
        }

        private async Task LoadMarkers()
        {
            _markers.Clear();
            _markers = await _markerService.GetByType(MarkerTypeEnum.Marked);
            _selectedMarker = _markers.FirstOrDefault();
        }
    }
}
