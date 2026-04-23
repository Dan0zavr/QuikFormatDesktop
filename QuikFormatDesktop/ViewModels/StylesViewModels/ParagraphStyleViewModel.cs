using CommunityToolkit.Mvvm.Input;
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
using System.Windows.Media;

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
        private bool _isEdit = false;

        private List<double> _intervals;

        private string _popupMessage;
        private bool _isPopupOpen = false;
        private Color _popupBackground;
        private Color _popupForeground;

        public ParagraphStyleViewModel(IOptions<ParagraphSettings> options, ParagraphService paragraphService, AlignmentService alignmentService, IDialogService dialogService)
        {
            _dialogService = dialogService;
            _paragraphService = paragraphService;
            _alignmentService = alignmentService;
            Intervals = options.Value.AllowedIntervals;
            SelectedAlignment = AlignmentType.Both;
            SelectedInterval = options.Value.DefaultInterval;

            AddParagraphCommand = new AsyncRelayCommand(AddParagraphStyleAsync, CanAddParagraphStyle);
            UpdateParagraphCommand = new AsyncRelayCommand(UpdateParagraphStyleAsync, CanAddParagraphStyle);
            ResetCommand = new RelayCommand(Reset);
            CancelCommand = new RelayCommand<object?>(_ => RequestReset?.Invoke());
        }

        public event Action RequestReset;

        public ICommand CancelCommand { get; }
        public ICommand ResetCommand { get; }
        public ICommand AddParagraphCommand { get; }
        public ICommand UpdateParagraphCommand { get; }

        public string CardName
        {
            get
            {
                return IsEdit ? "Редактирование стиля абзаца" : "Новый стиль абзаца";
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

        private int StyleId { get; set; }

        public string ParagraphStyleName
        {
            get => _paragraphStyleName;
            set
            {
                _paragraphStyleName = value;
                OnPropertyChanged(nameof(ParagraphStyleName));
                (AddParagraphCommand as IAsyncRelayCommand)?.NotifyCanExecuteChanged();
                (UpdateParagraphCommand as IAsyncRelayCommand)?.NotifyCanExecuteChanged();
            }
        }

        public string OldStyleName { get; set; }

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
                (AddParagraphCommand as IAsyncRelayCommand)?.NotifyCanExecuteChanged();
            }
        }

        public double LeftIndent
        {
            get => _leftIndent;
            set
            {
                _leftIndent = value;
                OnPropertyChanged(nameof(LeftIndent));
                (AddParagraphCommand as IAsyncRelayCommand)?.NotifyCanExecuteChanged();
            }
        }

        public double RightIndent
        {
            get => _rightIndent;
            set
            {
                _rightIndent = value;
                OnPropertyChanged(nameof(RightIndent));
                (AddParagraphCommand as IAsyncRelayCommand)?.NotifyCanExecuteChanged();
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
                (AddParagraphCommand as IAsyncRelayCommand)?.NotifyCanExecuteChanged();
            }
        }

        public double AfterInterval
        {
            get => _afterInterval;
            set
            {
                _afterInterval = value;
                OnPropertyChanged(nameof(AfterInterval));
                (AddParagraphCommand as IAsyncRelayCommand)?.NotifyCanExecuteChanged();
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

        public List<double> Intervals
        {
            get => _intervals;
            set
            {
                _intervals = value;
                OnPropertyChanged(nameof(Intervals));
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

        private bool CanAddParagraphStyle()
        {
            return !string.IsNullOrWhiteSpace(ParagraphStyleName) &&
                   FirstLineIndent != null &&
                   LeftIndent != null &&
                   RightIndent != null &&
                   AfterInterval != null &&
                   BeforeInterval != null;
        }

        private async Task AddParagraphStyleAsync()
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
                    await ShowPopup("Стиль успешно добавлен", PopupType.Good);
                }
                else
                {
                    await ShowPopup("Стиль с таким именем уже существует", PopupType.Bad);
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
            finally
            {
                RequestReset?.Invoke();
            }
        }

        private async Task UpdateParagraphStyleAsync()
        {
            try
            {
                int alignmentId = await _alignmentService.GetIdByType(SelectedAlignment);

                var paragraphStyle = new ParagraphStyle
                {
                    Id = StyleId,
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

                bool isUnique = true;

                if(OldStyleName != paragraphStyle.Name)
                {
                    isUnique = await _paragraphService.IsUnique(paragraphStyle.Name);
                }

                if (isUnique)
                {
                    await _paragraphService.Update(paragraphStyle);
                    await ShowPopup("Стиль успешно обновлен", PopupType.Good);
                }
                else
                {
                    await ShowPopup("Стиль с таким именем уже существует", PopupType.Good);
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
            finally
            {
                RequestReset?.Invoke();
            }
        }

        public void Reset()
        {
            IsEdit = false;
            ParagraphStyleName = string.Empty;
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
            IsEdit = isEdit;

            StyleId = paragraphStyle.Id;
            ParagraphStyleName = paragraphStyle.Name;
            OldStyleName = paragraphStyle.Name;
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

