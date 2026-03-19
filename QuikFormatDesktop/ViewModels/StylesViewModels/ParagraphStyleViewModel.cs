using QuikFormatDesktop.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using QuikFormatDesktop.ViewModels.Enums;

namespace QuikFormatDesktop.ViewModels.StylesViewModels
{
    public class ParagraphStyleViewModel : ViewModelBase
    {
        private readonly ParagraphService _paragraphService;

        private string _paragraphStyleName;
        private ParagraphAlignmentType _selectedAlignment;
        private double _firstLineIndent;
        private double _leftIndent;
        private double _rightIndent;
        private double _selectedInterval;
        private double _beforeInterval;
        private double _afterInterval;
        private bool _contextualSpacing;
        private string _pStatusMessage;

        private ObservableCollection<double> _intervals;

        private ICommand _textDeleteCommand;
        private ICommand _addParagraphCommand;

        public ParagraphStyleViewModel(ParagraphService paragraphService)
        {
            _paragraphService = paragraphService;
            Intervals = new ObservableCollection<double> { 1.0, 1.5, 2.0, 2.5, 3.0 };
        }

        public string ParagraphStyleName
        {
            get => _paragraphStyleName;
            set
            {
                _paragraphStyleName = value;
                OnPropertyChanged(nameof(ParagraphStyleName));
            }
        }

        public ParagraphAlignmentType SelectedAlignment
        {
            get => _selectedAlignment;
            set
            {
                _selectedAlignment = value;
                OnPropertyChanged(nameof(SelectedAlignment));
            }
        }

        public double FirstLineIndent
        {
            get => _firstLineIndent;
            set
            {
                _firstLineIndent = value;
                OnPropertyChanged(nameof(FirstLineIndent));
            }
        }

        public double LeftIndent
        {
            get => _leftIndent;
            set
            {
                _leftIndent = value;
                OnPropertyChanged(nameof(LeftIndent));
            }
        }

        public double RightIndent
        {
            get => _rightIndent;
            set
            {
                _rightIndent = value;
                OnPropertyChanged(nameof(RightIndent));
            }
        }

        public double SelectedInterval
        {
            get => _selectedInterval;
            set
            {
                _selectedInterval = value;
                OnPropertyChanged(nameof(SelectedInterval));
            }
        }

        public double BeforeInterval
        {
            get => _beforeInterval;
            set
            {
                _beforeInterval = value;
                OnPropertyChanged(nameof(BeforeInterval));
            }
        }

        public double AfterInterval
        {
            get => _afterInterval;
            set
            {
                _afterInterval = value;
                OnPropertyChanged(nameof(AfterInterval));
            }
        }

        public bool ContextualSpacing
        {
            get => _contextualSpacing;
            set
            {
                _contextualSpacing = value;
                OnPropertyChanged(nameof(ContextualSpacing));
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

        public ObservableCollection<double> Intervals
        {
            get => _intervals;
            set
            {
                _intervals = value;
                OnPropertyChanged(nameof(Intervals));
            }
        }

        public ICommand TextDeleteCommand;
        public ICommand AddParagraphCommand;
    }
}

