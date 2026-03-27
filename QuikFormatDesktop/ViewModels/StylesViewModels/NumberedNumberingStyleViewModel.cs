using QuikFormatDesktop.Models;
using QuikFormatDesktop.ViewModels.Commands;
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

        private bool _isEdit = false;

        public NumberedNumberingStyleViewModel(NumberingService numberingService, MarkerService markerService, IDialogService dialogService)
        {
            _numberingService = numberingService;
            _markerService = markerService;
            _dialogService = dialogService;
            LoadMarkers();

            AddNumberingCommand = new AddNumberingStyleCommand(this, _numberingService, _dialogService);
            UpdateNumberingCommand = new UpdateNumberingStyleCommand(this, _numberingService, _dialogService);
            ResetCommand = new RelayCommand(Reset);
            CancelCommand = new RelayCommand(_ => RequestReset?.Invoke());
        }

        public event Action RequestReset;

        public void RaiseRequestReset()
        {
            RequestReset?.Invoke();
        }

        public ICommand CancelCommand { get; }
        public ICommand ResetCommand { get; }
        public ICommand AddNumberingCommand { get; }
        public ICommand UpdateNumberingCommand { get; }

        public string CardName
        {
            get
            {
                return IsEdit ? "Редактирование стиля нумерованного списка" : "Новый стиль нумерованного списка";
            }
        }

        public bool IsEdit
        {
            get => _isEdit;
            set
            {
                _isEdit = value;
                OnPropertyChanged(nameof(IsEdit));
                OnPropertyChanged(nameof(CardName));
            }

        }

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
            IsEdit = false;
            NumberingStyleName = string.Empty;
            SelectedMarker = _markers.FirstOrDefault();
        }

        public void Reset(object? parameter)
        {
            Reset();
        }

        private async Task LoadMarkers()
        {
            _markers.Clear();
            _markers = await _markerService.GetByType(MarkerTypeEnum.Numberd);
            _selectedMarker = _markers.FirstOrDefault();
        }
    }
}
