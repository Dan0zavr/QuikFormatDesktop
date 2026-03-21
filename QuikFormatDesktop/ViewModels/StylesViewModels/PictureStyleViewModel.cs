using QuikFormatDesktop.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using QuikFormatDesktop.ViewModels.Enums;

namespace QuikFormatDesktop.ViewModels.StylesViewModels
{
    public class PictureStyleViewModel : ViewModelBase
    {
        private readonly PictureService _pictureService; // опционально, если будет использоваться

        private string _pictureStyleName;
        private HorizontalAlignmentType _selectedAlignment;
        private double _firstLineIndent;
        private double _leftIndent;
        private double _rightIndent;
        private double _interval;
        private double _beforeInterval;
        private double _afterInterval;
        private bool _contextualSpacing;
        private bool _autoGenerateCaption;
        private string _captionText;
        private bool _insertBlankLines;
        private string _pStatusMessage;

        private ObservableCollection<double> _intervals;

        public PictureStyleViewModel()
        {
            Intervals = new ObservableCollection<double> { 1.0, 1.5, 2.0, 2.5, 3.0 };
        }

        public string PictureStyleName
        {
            get => _pictureStyleName;
            set
            {
                _pictureStyleName = value;
                OnPropertyChanged(nameof(PictureStyleName));
            }
        }

        public HorizontalAlignmentType SelectedAlignment
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

        public double Interval
        {
            get => _interval;
            set
            {
                _interval = value;
                OnPropertyChanged(nameof(Interval));
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

        public bool AutoGenerateCaption
        {
            get => _autoGenerateCaption;
            set
            {
                _autoGenerateCaption = value;
                OnPropertyChanged(nameof(AutoGenerateCaption));
            }
        }

        public string CaptionText
        {
            get => _captionText;
            set
            {
                _captionText = value;
                OnPropertyChanged(nameof(CaptionText));
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

        public ICommand PictureDeleteCommand;

        public ICommand AddPictureCommand;
    }
}
