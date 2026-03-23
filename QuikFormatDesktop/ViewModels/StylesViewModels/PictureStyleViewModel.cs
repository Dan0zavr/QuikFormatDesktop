using QuikFormatDesktop.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using QuikFormatDesktop.ViewModels.Enums;
using Microsoft.Extensions.Options;
using QuikFormatDesktop.Models.SupportModels;
using QuikFormatDesktop.Models;
using QuikFormatDesktop.ViewModels.Commands;

namespace QuikFormatDesktop.ViewModels.StylesViewModels
{
    public class PictureStyleViewModel : ViewModelBase
    {
        private readonly PictureService _pictureService;
        private readonly ParagraphService _paragraphService;
        private readonly AlignmentService _alignmentService;
        private readonly IDialogService _dialogService;

        private string _pictureStyleName;
        private AlignmentType _selectedAlignment;
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

        private List<double> _intervals;

        public PictureStyleViewModel(PictureService pictureService, ParagraphService paragraphService, IOptions<ParagraphSettings> options, IDialogService dialogService, AlignmentService alignmentService)
        {
            _pictureService = pictureService;
            _paragraphService = paragraphService;
            _dialogService = dialogService;
            _alignmentService = alignmentService;
            SetDefault(options);

            AddPictureCommand = new AsyncRelayCommand(AddPictureStyle, CanAddPictureStyle);
        }

        public ICommand PictureDeleteCommand { get; }
        public ICommand AddPictureCommand { get; }

        public string PictureStyleName
        {
            get => _pictureStyleName;
            set
            {
                _pictureStyleName = value;
                OnPropertyChanged(nameof(PictureStyleName));
                (AddPictureCommand as AsyncRelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public AlignmentType SelectedAlignment
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
                (AddPictureCommand as AsyncRelayCommand)?.RaiseCanExecuteChanged();
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

        public List<double> Intervals
        {
            get => _intervals;
            set
            {
                _intervals = value;
                OnPropertyChanged(nameof(Intervals));
            }
        }

        private void SetDefault(IOptions<ParagraphSettings> options)
        {
            _intervals = options.Value.AllowedIntervals;

            _pictureStyleName = null;
            _selectedAlignment = AlignmentType.Center;
            FirstLineIndent = 0;
            LeftIndent = 0;
            RightIndent = 0;
            Interval = _intervals.FirstOrDefault();
            AfterInterval = 0;
            BeforeInterval = 0;
            AutoGenerateCaption = false;
            CaptionText = null;
            InsertBlankLines = false;
        }

        private bool CanAddPictureStyle(object? parametr)
        {
            if (!string.IsNullOrWhiteSpace(PictureStyleName))
            {
                if (_autoGenerateCaption)
                {
                    if (!string.IsNullOrWhiteSpace(CaptionText))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        private async Task AddPictureStyle(object? parametr)
        {
            try
            {
                if (await _pictureService.IsUnique(PictureStyleName))
                {
                    int paragraphId = await AddParagraphStyle();

                    PictureStyle pictureStyle = new PictureStyle
                    {
                        Name = PictureStyleName,
                        ParagraphStyle = paragraphId,
                        GenerateLabel = AutoGenerateCaption,
                        LabelValue = CaptionText,
                        EmptyLineAround = InsertBlankLines
                    };

                    await _pictureService.Add(pictureStyle);
                }
            }
            catch (Exception ex)
            {
                _dialogService.ShowError($"Ошибка. Код: {ex.HResult}");
            }
        }
        private async Task<int> AddParagraphStyle()
        {
            int alignmentId = await _alignmentService.GetIdByType(SelectedAlignment);
            string styleName = "Для рисунка " + $"\"{PictureStyleName}\""+ "(Generated)";
            ParagraphStyle style = new ParagraphStyle
            {
                Name = styleName,
                Alignment = alignmentId,
                FirstLineIndent = this.FirstLineIndent,
                LeftIndent = this.LeftIndent,
                RightIndent = this.RightIndent,
                IntervalInText = Interval,
                BeforeInterval = this.BeforeInterval,
                AfterInterval = this.AfterInterval,
                ContextualSpacing = this.ContextualSpacing
            };

            if (!await _paragraphService.IsUnique(styleName))
            {
                styleName = await EnsureUniqueParagraphName(styleName);
                style.Name = styleName;
            }

            await _paragraphService.Add(style);
            return await _paragraphService.GetIdByName(styleName);
        }

        private async Task<string> EnsureUniqueParagraphName(string name)
        {
            List<string> usedNames = await _paragraphService.GetLikeNames(name);
            string uniqueName = name;
            int count = 0;
            while (!usedNames.Contains(name))
            {
                count++;
                uniqueName = name + count.ToString();
            }

            return uniqueName;
        }
    }
}
