using QuikFormatDesktop.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace QuikFormatDesktop.ViewModels.StylesViewModels
{
    public class NumberedNumberingStyleViewModel : ViewModelBase
    {
        private readonly NumberingService _numberingService;

        private string _numberingStyleName;
        private ObservableCollection<string> _markers;
        private string _selectedMarker;
        private string _pStatusMessage;

        public NumberedNumberingStyleViewModel()
        {
        }

        public string NumberingStyleName
        {
            get => _numberingStyleName;
            set
            {
                _numberingStyleName = value;
                OnPropertyChanged(nameof(NumberingStyleName));
            }
        }

        public ObservableCollection<string> Markers
        {
            get => _markers;
            set
            {
                _markers = value;
                OnPropertyChanged(nameof(Markers));
            }
        }

        public string SelectedMarker
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

        public ICommand NumberingDeleteCommand;

        public ICommand AddNumberingCommand;
    }
}
