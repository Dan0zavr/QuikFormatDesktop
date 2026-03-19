using QuikFormatDesktop.Models;
using QuikFormatDesktop.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace QuikFormatDesktop.ViewModels.StylesViewModels
{
    public class TableStyleViewModel : ViewModelBase
    {
        private readonly TableService _tableService;

        private string _tableStyleName;
        private ParagraphStyle _selectedParagraphStyle;
        private TextStyle _selectedTextStyle;
        private VerticalAlignment _selectedVerticalAlignment;
        private double
            _padding;
        private int _borderThikness;
        private Color _borderColor;
        private string _pStatusMessage;

        private ObservableCollection<ParagraphStyle> _paragraphStyles;
        private ObservableCollection<TextStyle> _textStyles;

        private ICommand _textDeleteCommand;
        private ICommand _addParagraphCommand;

        public TableStyleViewModel()
        {
            ParagraphStyles = new ObservableCollection<ParagraphStyle>();
            TextStyles = new ObservableCollection<TextStyle>();
        }

        // Если потребуется сервис, можно добавить конструктор с параметром
        // public TableStyleViewModel(TableService tableService) : this() { _tableService = tableService; }

        public string TableStyleName
        {
            get => _tableStyleName;
            set
            {
                _tableStyleName = value;
                OnPropertyChanged(nameof(TableStyleName));
            }
        }

        public ObservableCollection<ParagraphStyle> ParagraphStyles
        {
            get => _paragraphStyles;
            set
            {
                _paragraphStyles = value;
                OnPropertyChanged(nameof(ParagraphStyles));
            }
        }

        public ParagraphStyle SelectedParagraphStyle
        {
            get => _selectedParagraphStyle;
            set
            {
                _selectedParagraphStyle = value;
                OnPropertyChanged(nameof(SelectedParagraphStyle));
            }
        }

        public ObservableCollection<TextStyle> TextStyles
        {
            get => _textStyles;
            set
            {
                _textStyles = value;
                OnPropertyChanged(nameof(TextStyles));
            }
        }

        public TextStyle SelectedTextStyle
        {
            get => _selectedTextStyle;
            set
            {
                _selectedTextStyle = value;
                OnPropertyChanged(nameof(SelectedTextStyle));
            }
        }

        public VerticalAlignment SelectedVerticalAlignment
        {
            get => _selectedVerticalAlignment;
            set
            {
                _selectedVerticalAlignment = value;
                OnPropertyChanged(nameof(SelectedVerticalAlignment));
            }
        }

        public double Padding
        {
            get => _padding;
            set
            {
                _padding = value;
                OnPropertyChanged(nameof(Padding));
            }
        }

        public int BorderThikness
        {
            get => _borderThikness;
            set
            {
                _borderThikness = value;
                OnPropertyChanged(nameof(BorderThikness));
            }
        }

        public Color BorderColor
        {
            get => _borderColor;
            set
            {
                _borderColor = value;
                OnPropertyChanged(nameof(BorderColor));
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

        public ICommand TextDeleteCommand;

        public ICommand AddParagraphCommand;
    }
}
