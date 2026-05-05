using CommunityToolkit.Mvvm.Input;
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
using System.Windows.Media;

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
        private string _popupMessage;

        private bool _isEdit = false;

        private bool _isPopupOpen = false;
        private Color _popupBackground;
        private Color _popupForeground;

        public NumberedNumberingStyleViewModel(NumberingService numberingService, MarkerService markerService, IDialogService dialogService)
        {
            _numberingService = numberingService;
            _markerService = markerService;
            _dialogService = dialogService;
            LoadMarkers();

            AddNumberingCommand = new AddNumberingStyleCommand(this, _numberingService, _dialogService);
            UpdateNumberingCommand = new UpdateNumberingStyleCommand(this, _numberingService, _dialogService);
            ResetCommand = new RelayCommand<object?>(Reset);
            CancelCommand = new RelayCommand<object?>(_ => RequestReset?.Invoke());
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

        public string PopupMessage
        {
            get => _popupMessage;
            set
            {
                _popupMessage = value;
                OnPropertyChanged(nameof(PopupMessage));
            }
        }

        public bool IsPopupOpen
        {
            get => _isPopupOpen;
            set
            {
                _isPopupOpen = value;
                OnPropertyChanged(nameof(IsPopupOpen));
            }
        }

        public Color PopupBackground
        {
            get => _popupBackground;
            set
            {
                _popupBackground = value;
                OnPropertyChanged(nameof(PopupBackground));
            }
        }

        public Color PopupForeground
        {
            get => _popupForeground;
            set
            {
                _popupForeground = value;
                OnPropertyChanged(nameof(PopupForeground));
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
            _markers = await _markerService.GetByType(MarkerTypeEnum.Numbered);
            _selectedMarker = _markers.FirstOrDefault();
        }

        private async Task ShowPopup(string message, PopupType type)
        {
            switch (type)
            {
                case PopupType.Bad:
                    PopupBackground = (Color)ColorConverter.ConvertFromString("#fc9d9d");
                    PopupForeground = (Color)ColorConverter.ConvertFromString("#570000");
                    break;
                case PopupType.Good:
                    PopupBackground = (Color)ColorConverter.ConvertFromString("#b1ffa8");
                    PopupForeground = (Color)ColorConverter.ConvertFromString("#085200");
                    break;
                default:
                    PopupBackground = Colors.White;
                    PopupForeground = Colors.Black;
                    break;
            }

            PopupMessage = message;
            IsPopupOpen = true;
            await Task.Delay(2000);
            IsPopupOpen = false;
        }
    }
}
