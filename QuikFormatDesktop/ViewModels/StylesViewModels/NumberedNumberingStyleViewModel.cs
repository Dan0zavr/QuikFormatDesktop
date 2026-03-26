using QuikFormatDesktop.Models;
using QuikFormatDesktop.ViewModels.Commands.NumberingStyleCommand;
using QuikFormatDesktop.ViewModels.Commands.NumberingStyleCommands;
using QuikFormatDesktop.ViewModels.Enums;
using QuikFormatDesktop.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace QuikFormatDesktop.ViewModels.StylesViewModels
{
    public class NumberedNumberingStyleViewModel : ViewModelBase, INumbering, IResetable, ILoadable
    {
        public readonly NumberingService _numberingService;
        public readonly MarkerService _markerService;
        public readonly IDialogService _dialogService;

        private string _numberingStyleName;
        private List<Marker> _markers = new List<Marker>();
        private Marker _selectedMarker;
        private string _pStatusMessage;

        public NumberedNumberingStyleViewModel(NumberingService numberingService, MarkerService markerService, IDialogService dialogService)
        {
            _numberingService = numberingService;
            _markerService = markerService;
            _dialogService = dialogService;
            LoadMarkers();

            AddNumberingCommand = new AddNumberingStyleCommand(this, _numberingService, _dialogService);
            UpdateNumberingCommand = new UpdateNumberingStyleCommand(this, _numberingService, _dialogService);
        }

        public ICommand NumberingDeleteCommand { get; }
        public ICommand AddNumberingCommand { get; }
        public ICommand UpdateNumberingCommand { get; }

        public bool IsEdit { get; private set; } = false;

        public int StyleId { get; set; }
        public string NumberingStyleName
        {
            get => _numberingStyleName;
            set
            {
                _numberingStyleName = value;
                OnPropertyChanged(nameof(NumberingStyleName));
                (AddNumberingCommand as AddNumberingStyleCommand)?.RaiseCanExecuteChanged();
                (UpdateNumberingCommand as UpdateNumberingStyleCommand)?.RaiseCanExecuteChanged();
            }
        }

        public string OldStyleName { get; set; }

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

        public void Load(object paramter, bool isEdit = false)
        {
            IsEdit = isEdit;

            if (paramter is NumberingStyle numberingStyle)
            {
                StyleId = numberingStyle.Id;
                NumberingStyleName = numberingStyle.Name;
                OldStyleName = numberingStyle.Name;
                SelectedMarker = _markers.Where(x => x.Id == numberingStyle.Marker).FirstOrDefault() ;
            }
        }

        public void Reset()
        {
            NumberingStyleName = string.Empty;
            SelectedMarker = _markers.FirstOrDefault();
        }

        private async Task LoadMarkers()
        {
            _markers.Clear();
            _markers = await _markerService.GetByType(MarkerTypeEnum.Numberd);
            _selectedMarker = _markers.FirstOrDefault();
        }
    }
}
