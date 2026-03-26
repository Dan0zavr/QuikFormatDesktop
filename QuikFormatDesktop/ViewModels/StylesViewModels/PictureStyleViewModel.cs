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
    public class PictureStyleViewModel : ViewModelBase, ILoadable, IResetable
    {
        private ParagraphStyle _paragraphStyle;
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
        private IOptions<ParagraphSettings> _options;

        private bool _isEdit = false;

        private List<double> _intervals;

        public PictureStyleViewModel(PictureService pictureService, ParagraphService paragraphService, IOptions<ParagraphSettings> options, IDialogService dialogService, AlignmentService alignmentService)
        {
            _pictureService = pictureService;
            _paragraphService = paragraphService;
            _dialogService = dialogService;
            _alignmentService = alignmentService;
            _options = options;
            SetDefault(_options);

            AddPictureCommand = new AsyncRelayCommand(AddPictureStyle, CanAddPictureStyle);
            UpdatePictureCommand = new AsyncRelayCommand(UpdatePictureStyle, CanAddPictureStyle);
            ResetCommand = new RelayCommand(Reset);
        }

        public string CardName
        {
            get
            {
                return IsEdit ? "Редактирование стиля рисунка" : "Новый стиль рисунка";
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

        public ICommand ResetCommand { get; }
        public ICommand AddPictureCommand { get; }
        public ICommand UpdatePictureCommand { get; }

        private int StyleId { get; set; }
        private int ParagraphStyleId { get; set; }
        public string PictureStyleName
        {
            get => _pictureStyleName;
            set
            {
                _pictureStyleName = value;
                OnPropertyChanged(nameof(PictureStyleName));
                (AddPictureCommand as AsyncRelayCommand)?.RaiseCanExecuteChanged();
                (UpdatePictureCommand as AsyncRelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        private string OldPictureStyleName { get; set; }
        private string OldParagraphStyleName { get; set; }

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
                (UpdatePictureCommand as AsyncRelayCommand)?.RaiseCanExecuteChanged();
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
            IsEdit = false;

            Intervals = options.Value.AllowedIntervals;

            PictureStyleName = null;
            SelectedAlignment = AlignmentType.Center;
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

        private bool CanAddPictureStyle(object? parameter)
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

        private async Task AddPictureStyle(object? parameter)
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
        private async Task UpdatePictureStyle(object? parameter)
        {
            try
            {
                bool isUnique = true;

                if(OldPictureStyleName != PictureStyleName)
                {
                    isUnique = await _pictureService.IsUnique(PictureStyleName);
                }

                if (isUnique)
                {
                    await UpdateParagraphStyle();

                    PictureStyle pictureStyle = new PictureStyle
                    {
                        Id = StyleId,
                        Name = PictureStyleName,
                        ParagraphStyle = ParagraphStyleId,
                        GenerateLabel = AutoGenerateCaption,
                        LabelValue = CaptionText,
                        EmptyLineAround = InsertBlankLines
                    };

                    await _pictureService.Update(pictureStyle);
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

        private async Task UpdateParagraphStyle()
        {
            int alignmentId = await _alignmentService.GetIdByType(SelectedAlignment);
            string styleName = "Для рисунка " + $"\"{PictureStyleName}\"" + "(Generated)";
            ParagraphStyle style = new ParagraphStyle
            {
                Id = ParagraphStyleId,
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

            bool isUnique = true;

            if (OldParagraphStyleName != style.Name)
            {
                isUnique = await _paragraphService.IsUnique(styleName);
            }

            if (!isUnique)
            {
                styleName = await EnsureUniqueParagraphName(styleName);
                style.Name = styleName;
            }

            await _paragraphService.Update(style);
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

        public void Reset()
        {
            SetDefault(_options);
        }

        public void Reset(object? parameter)
        {
            Reset();
        }

        public void Load(object parametr, bool isEdit = false)
        {
            Reset();
            IsEdit = isEdit;

            if (parametr is PictureStyle pictureStyle)
            {
                _paragraphStyle = _paragraphService.GetById(pictureStyle.ParagraphStyle).GetAwaiter().GetResult();

                StyleId = pictureStyle.Id;
                ParagraphStyleId = pictureStyle.ParagraphStyle;
                PictureStyleName = pictureStyle.Name;
                OldPictureStyleName = pictureStyle.Name;
                Enum.TryParse(_alignmentService.GetById(_paragraphStyle.Alignment).GetAwaiter().GetResult().Alignment1, true, out AlignmentType alignment);

                OldParagraphStyleName = _paragraphStyle.Name;
                SelectedAlignment = alignment;
                FirstLineIndent = (double)_paragraphStyle.FirstLineIndent;
                LeftIndent = (double)_paragraphStyle.LeftIndent;
                RightIndent = (double)_paragraphStyle.RightIndent;
                Interval = _paragraphStyle.IntervalInText;
                BeforeInterval = (double)_paragraphStyle.BeforeInterval;
                AfterInterval = (double)_paragraphStyle.AfterInterval;
                ContextualSpacing = _paragraphStyle.ContextualSpacing;

                AutoGenerateCaption = pictureStyle.GenerateLabel;
                if (AutoGenerateCaption)
                {
                    CaptionText = pictureStyle.LabelValue;
                }
                InsertBlankLines = pictureStyle.EmptyLineAround;
            }
        }
    }
}
