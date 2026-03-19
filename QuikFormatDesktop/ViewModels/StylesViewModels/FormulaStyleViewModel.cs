using QuikFormatDesktop.Models;
using QuikFormatDesktop.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace QuikFormatDesktop.ViewModels.StylesViewModels
{
    public class FormulaStyleViewModel : ViewModelBase
    {
        private readonly FormulaService _formulaService;

        private string _formulaStyleName;
        private Alignment _selectedPosition;
        private bool _insertBlankLines;
        private bool _isNumberingEnabled;
        private ObservableCollection<string> _numberingFormats;
        private string _selectedNumberingFormat;
        private string _pStatusMessage;


        public FormulaStyleViewModel()
        {
            NumberingFormats = new ObservableCollection<string>();
        }

        public string FormulaStyleName
        {
            get => _formulaStyleName;
            set
            {
                _formulaStyleName = value;
                OnPropertyChanged(nameof(FormulaStyleName));
            }
        }

        public Alignment SelectedPosition
        {
            get => _selectedPosition;
            set
            {
                _selectedPosition = value;
                OnPropertyChanged(nameof(SelectedPosition));
            }
        }

        public bool InsertBlankLines
        {
            get => _insertBlankLines;
            set
            {
                _insertBlankLines = value;
                OnPropertyChanged(nameof(InsertBlankLines));
            }
        }

        public bool IsNumberingEnabled
        {
            get => _isNumberingEnabled;
            set
            {
                _isNumberingEnabled = value;
                OnPropertyChanged(nameof(IsNumberingEnabled));
            }
        }

        public ObservableCollection<string> NumberingFormats
        {
            get => _numberingFormats;
            set
            {
                _numberingFormats = value;
                OnPropertyChanged(nameof(NumberingFormats));
            }
        }

        public string SelectedNumberingFormat
        {
            get => _selectedNumberingFormat;
            set
            {
                _selectedNumberingFormat = value;
                OnPropertyChanged(nameof(SelectedNumberingFormat));
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

        public ICommand FormulaDeleteCommand;

        public ICommand AddFormulaCommand;
    }
}
