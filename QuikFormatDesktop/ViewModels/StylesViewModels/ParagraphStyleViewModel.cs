using Microsoft.Extensions.Options;
using QuikFormatDesktop.Exceptions;
using QuikFormatDesktop.Models;
using QuikFormatDesktop.Models.SupportModels;
using QuikFormatDesktop.ViewModels.Commands;
using QuikFormatDesktop.ViewModels.Enums;
using QuikFormatDesktop.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace QuikFormatDesktop.ViewModels.StylesViewModels
{
    public class ParagraphStyleViewModel : ViewModelBase, IResetable
    {
        public IDialogService _dialogService;
        public readonly ParagraphService _paragraphService;
        public readonly AlignmentService _alignmentService;

        private string _paragraphStyleName;
        private AlignmentType _selectedAlignment;
        private double _firstLineIndent;
        private double _leftIndent;
        private double _rightIndent;
        private double _selectedInterval;
        private double _beforeInterval;
        private double _afterInterval;
        private bool _contextualSpacing = false;
        private string _pStatusMessage;

        private List<double> _intervals;

        public ParagraphStyleViewModel(IOptions<ParagraphSettings> options, ParagraphService paragraphService, AlignmentService alignmentService, IDialogService dialogService)
        {
            _dialogService = dialogService;
            _paragraphService = paragraphService;
            _alignmentService = alignmentService;
            Intervals = options.Value.AllowedIntervals;
            SelectedAlignment = AlignmentType.Both;
            SelectedInterval = options.Value.DefaultInterval;

            AddParagraphCommand = new AsyncRelayCommand(AddParagraphStyleAsync, CanAddParagraphStyle);
        }

        public ICommand TextDeleteCommand;
        public ICommand AddParagraphCommand { get; }

        public string ParagraphStyleName
        {
            get => _paragraphStyleName;
            set
            {
                _paragraphStyleName = value;
                OnPropertyChanged(nameof(ParagraphStyleName));
                (AddParagraphCommand as AsyncRelayCommand)?.RaiseCanExecuteChanged();
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
                (AddParagraphCommand as AsyncRelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public double LeftIndent
        {
            get => _leftIndent;
            set
            {
                _leftIndent = value;
                OnPropertyChanged(nameof(LeftIndent));
                (AddParagraphCommand as AsyncRelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public double RightIndent
        {
            get => _rightIndent;
            set
            {
                _rightIndent = value;
                OnPropertyChanged(nameof(RightIndent));
                (AddParagraphCommand as AsyncRelayCommand)?.RaiseCanExecuteChanged();
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
                (AddParagraphCommand as AsyncRelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public double AfterInterval
        {
            get => _afterInterval;
            set
            {
                _afterInterval = value;
                OnPropertyChanged(nameof(AfterInterval));
                (AddParagraphCommand as AsyncRelayCommand)?.RaiseCanExecuteChanged();
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

        public List<double> Intervals
        {
            get => _intervals;
            set
            {
                _intervals = value;
                OnPropertyChanged(nameof(Intervals));
            }
        }

        private bool CanAddParagraphStyle(object? parameter)
        {
            return !string.IsNullOrWhiteSpace(ParagraphStyleName) &&
                   FirstLineIndent != null &&
                   LeftIndent != null &&
                   RightIndent != null &&
                   AfterInterval != null &&
                   BeforeInterval != null;
        }

        private async Task AddParagraphStyleAsync(object? parameter)
        {
            try
            {
                int alignmentId = await _alignmentService.GetIdByType(SelectedAlignment);

                var paragraphStyle = new ParagraphStyle
                {
                    Name = ParagraphStyleName,
                    Alignment = alignmentId,
                    LeftIndent = LeftIndent,
                    RightIndent = RightIndent,
                    FirstLineIndent = FirstLineIndent,
                    IntervalInText = SelectedInterval,
                    BeforeInterval = BeforeInterval,
                    AfterInterval = AfterInterval,
                    ContextualSpacing = ContextualSpacing,
                };

                if (await _paragraphService.IsUnique(paragraphStyle.Name))
                {
                    await _paragraphService.Add(paragraphStyle);
                    PStatusMessage = "Стиль успешно добавлен";
                }
                else
                {
                    PStatusMessage = "Стиль с таким именем уже существует";
                }
            }
            catch (AlignmentNotFoundException aex)
            {
                _dialogService.ShowError(aex.Message);
            }
            catch (Exception ex)
            {
                _dialogService.ShowError($"Ошибка. Код: {ex.HResult}");
            }
        }

        public void Reset()
        {
            ParagraphStyleName = null;
            SelectedAlignment = AlignmentType.Both;
            FirstLineIndent = 0;
            LeftIndent = 0;
            RightIndent = 0;
            SelectedInterval = 1.5;
            BeforeInterval = 0;
            AfterInterval = 0;
            ContextualSpacing = false;
        }

        public void Load(ParagraphStyle paragraphStyle, bool isEdit)
        {  
            ParagraphStyleName = paragraphStyle.Name;
            string alName = _alignmentService.GetById(paragraphStyle.Alignment).GetAwaiter().GetResult().Alignment1;
            Enum.TryParse(alName, true, out AlignmentType alignment);
            SelectedAlignment = alignment;
            FirstLineIndent = (double)paragraphStyle.FirstLineIndent;
            LeftIndent = (double)paragraphStyle.LeftIndent;
            RightIndent = (double)paragraphStyle.RightIndent;
            SelectedInterval = paragraphStyle.IntervalInText;
            BeforeInterval = (double)paragraphStyle.BeforeInterval;
            AfterInterval =(double)paragraphStyle.AfterInterval;
            ContextualSpacing = paragraphStyle.ContextualSpacing;
        }
    }
}

