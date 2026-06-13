using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Options;
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
    public class GlobalStyleViewModel : ViewModelBase, ILoadable, IResetable
    {
        private readonly GlobalStyleService _globalStyleService;
        private readonly AlignmentService _alignmentService;
        private readonly IDialogService _dialogService;

        private string _globalStyleName;
        private double _leftMargin;
        private double _rightMargin;
        private double _topMargin;
        private double _bottomMargin;
        private string _specialColontitul;
        private int? _lastNoNumberingPage;
        private AlignmentType _selectedAlignment;
        private bool _useSpecialColontitul;
        private bool _useLastNoNumberingPage;

        private bool _isEdit = false;

        private string _popupMessage;
        private bool _isPopupOpen = false;
        private Color _popupBackground;
        private Color _popupForeground;

        private string _validationError;

        public GlobalStyleViewModel(GlobalStyleService globalStyleService, AlignmentService alignmentService, IDialogService dialogService)
        {
            _globalStyleService = globalStyleService;
            _alignmentService = alignmentService;
            _dialogService = dialogService;

            AddGlobalStyleCommand = new AsyncRelayCommand<object?>(AddGlobalStyle, CanAddGlobalStyle);
            UpdateGlobalStyleCommand = new AsyncRelayCommand<object?>(UpdateGlobalStyle, CanAddGlobalStyle);
            ResetCommand = new RelayCommand<object?>(Reset);

            SetDefault();
        }

        public string CardName
        {
            get
            {
                return IsEdit ? "Редактирование глобального стиля" : "Новый глобальный стиль";
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
                OnPropertyChanged(nameof(IsAddMode));
                (AddGlobalStyleCommand as IAsyncRelayCommand)?.NotifyCanExecuteChanged();
                (UpdateGlobalStyleCommand as IAsyncRelayCommand)?.NotifyCanExecuteChanged();
            }
        }

        public bool IsAddMode => !IsEdit;

        public ICommand ResetCommand { get; }
        public ICommand AddGlobalStyleCommand { get; }
        public ICommand UpdateGlobalStyleCommand { get; }

        private int StyleId { get; set; }

        public string GlobalStyleName
        {
            get => _globalStyleName;
            set
            {
                _globalStyleName = value;
                OnPropertyChanged(nameof(GlobalStyleName));
                ValidateFields();
                (AddGlobalStyleCommand as IAsyncRelayCommand)?.NotifyCanExecuteChanged();
                (UpdateGlobalStyleCommand as IAsyncRelayCommand)?.NotifyCanExecuteChanged();
            }
        }

        public double LeftMargin
        {
            get => _leftMargin;
            set
            {
                _leftMargin = value;
                OnPropertyChanged(nameof(LeftMargin));
                ValidateFields();
                (AddGlobalStyleCommand as IAsyncRelayCommand)?.NotifyCanExecuteChanged();
                (UpdateGlobalStyleCommand as IAsyncRelayCommand)?.NotifyCanExecuteChanged();
            }
        }

        public double RightMargin
        {
            get => _rightMargin;
            set
            {
                _rightMargin = value;
                OnPropertyChanged(nameof(RightMargin));
                ValidateFields();
                (AddGlobalStyleCommand as IAsyncRelayCommand)?.NotifyCanExecuteChanged();
                (UpdateGlobalStyleCommand as IAsyncRelayCommand)?.NotifyCanExecuteChanged();
            }
        }

        public double TopMargin
        {
            get => _topMargin;
            set
            {
                _topMargin = value;
                OnPropertyChanged(nameof(TopMargin));
                ValidateFields();
                (AddGlobalStyleCommand as IAsyncRelayCommand)?.NotifyCanExecuteChanged();
                (UpdateGlobalStyleCommand as IAsyncRelayCommand)?.NotifyCanExecuteChanged();
            }
        }

        public double BottomMargin
        {
            get => _bottomMargin;
            set
            {
                _bottomMargin = value;
                OnPropertyChanged(nameof(BottomMargin));
                ValidateFields();
                (AddGlobalStyleCommand as IAsyncRelayCommand)?.NotifyCanExecuteChanged();
                (UpdateGlobalStyleCommand as IAsyncRelayCommand)?.NotifyCanExecuteChanged();
            }
        }

        public string SpecialColontitul
        {
            get => _specialColontitul;
            set
            {
                _specialColontitul = value;
                OnPropertyChanged(nameof(SpecialColontitul));
            }
        }

        public int? LastNoNumberingPage
        {
            get => _lastNoNumberingPage;
            set
            {
                _lastNoNumberingPage = value;
                OnPropertyChanged(nameof(LastNoNumberingPage));
            }
        }

        public bool UseSpecialColontitul
        {
            get => _useSpecialColontitul;
            set
            {
                _useSpecialColontitul = value;
                OnPropertyChanged(nameof(UseSpecialColontitul));
                if (!value)
                    SpecialColontitul = null;
            }
        }

        public bool UseLastNoNumberingPage
        {
            get => _useLastNoNumberingPage;
            set
            {
                _useLastNoNumberingPage = value;
                OnPropertyChanged(nameof(UseLastNoNumberingPage));
                if (!value)
                    LastNoNumberingPage = null;
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

        public string ValidationError
        {
            get => _validationError;
            set
            {
                _validationError = value;
                OnPropertyChanged(nameof(ValidationError));
                OnPropertyChanged(nameof(IsValid));
            }
        }

        public bool IsValid => string.IsNullOrEmpty(ValidationError);

        private void ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(GlobalStyleName))
            {
                ValidationError = "Имя стиля обязательно";
                return;
            }

            if (LeftMargin < 0 || RightMargin < 0 || TopMargin < 0 || BottomMargin < 0)
            {
                ValidationError = "Поля не могут быть отрицательными";
                return;
            }

            if (UseSpecialColontitul && !string.IsNullOrEmpty(SpecialColontitul)
                && SpecialColontitul.Length > 16)
            {
                ValidationError = "Специальный колонтитул не может превышать 16 символов";
                return;
            }

            if (UseLastNoNumberingPage)
            {
                if (LastNoNumberingPage == null)
                {
                    ValidationError = "Номер страницы обязателен";
                    return;
                }
                if (LastNoNumberingPage < 0)
                {
                    ValidationError = "Номер страницы не может быть отрицательным";
                    return;
                }
            }

            ValidationError = null;
        }

        private void SetDefault()
        {
            IsEdit = false;
            GlobalStyleName = null;
            LeftMargin = 3.0;
            RightMargin = 1.5;
            TopMargin = 2.0;
            BottomMargin = 2.0;
            SpecialColontitul = null;
            LastNoNumberingPage = null;
            UseSpecialColontitul = false;
            UseLastNoNumberingPage = false;
            SelectedAlignment = AlignmentType.Left;
            ValidationError = null;
        }

        private bool CanAddGlobalStyle(object? parameter)
        {
            return IsValid;
        }

        private async Task AddGlobalStyle(object? parameter)
        {
            try
            {
                if (!IsValid)
                {
                    await ShowPopup("Пожалуйста, исправьте ошибки", PopupType.Bad);
                    return;
                }

                if (await _globalStyleService.IsUnique(GlobalStyleName))
                {
                    int alignmentId = await _alignmentService.GetIdByType(SelectedAlignment);

                    GlobalStyle globalStyle = new GlobalStyle
                    {
                        Name = GlobalStyleName,
                        LeftMargin = LeftMargin,
                        RightMargin = RightMargin,
                        TopMargin = TopMargin,
                        BottomMargin = BottomMargin,
                        SpecialColontitul = UseSpecialColontitul ? SpecialColontitul : null,
                        LastNoNumberingPage = UseLastNoNumberingPage ? LastNoNumberingPage : null,
                        AlignmentId = alignmentId
                    };

                    await _globalStyleService.Add(globalStyle);
                    await ShowPopup("Глобальный стиль успешно добавлен", PopupType.Good);
                }
                else
                {
                    await ShowPopup("Стиль с таким именем уже существует", PopupType.Bad);
                }
            }
            catch (Exception ex)
            {
                _dialogService.ShowError($"Ошибка. Код: {ex.HResult}");
            }
            finally
            {
                Reset();
            }
        }

        private async Task UpdateGlobalStyle(object? parameter)
        {
            try
            {
                if (!IsValid)
                {
                    await ShowPopup("Пожалуйста, исправьте ошибки", PopupType.Bad);
                    return;
                }

                bool isUnique = true;

                if (OldGlobalStyleName != GlobalStyleName)
                {
                    isUnique = await _globalStyleService.IsUnique(GlobalStyleName);
                }

                if (isUnique)
                {
                    int alignmentId = await _alignmentService.GetIdByType(SelectedAlignment);

                    GlobalStyle globalStyle = new GlobalStyle
                    {
                        Id = StyleId,
                        Name = GlobalStyleName,
                        LeftMargin = LeftMargin,
                        RightMargin = RightMargin,
                        TopMargin = TopMargin,
                        BottomMargin = BottomMargin,
                        SpecialColontitul = UseSpecialColontitul ? SpecialColontitul : null,
                        LastNoNumberingPage = UseLastNoNumberingPage ? LastNoNumberingPage : null,
                        AlignmentId = alignmentId
                    };

                    await _globalStyleService.Update(globalStyle);
                    await ShowPopup("Глобальный стиль успешно обновлен", PopupType.Good);
                }
                else
                {
                    await ShowPopup("Стиль с таким именем уже существует", PopupType.Bad);
                }
            }
            catch (Exception ex)
            {
                _dialogService.ShowError($"Ошибка. Код: {ex.HResult}");
            }
            finally
            {
                Reset();
            }
        }

        private string OldGlobalStyleName { get; set; }

        public void Reset()
        {
            SetDefault();
        }

        public void Reset(object? parameter)
        {
            Reset();
        }

        public void Load(object parametr, bool isEdit = false)
        {
            Reset();
            IsEdit = isEdit;

            if (parametr is GlobalStyle globalStyle)
            {
                StyleId = globalStyle.Id;
                GlobalStyleName = globalStyle.Name;
                OldGlobalStyleName = globalStyle.Name;
                LeftMargin = globalStyle.LeftMargin;
                RightMargin = globalStyle.RightMargin;
                TopMargin = globalStyle.TopMargin;
                BottomMargin = globalStyle.BottomMargin;

                UseSpecialColontitul = !string.IsNullOrEmpty(globalStyle.SpecialColontitul);
                SpecialColontitul = globalStyle.SpecialColontitul;

                UseLastNoNumberingPage = globalStyle.LastNoNumberingPage.HasValue;
                LastNoNumberingPage = globalStyle.LastNoNumberingPage;

                Alignment alignment = _alignmentService.GetById(globalStyle.AlignmentId).GetAwaiter().GetResult();
                Enum.TryParse(alignment.Alignment1, true, out AlignmentType alignmentType);
                SelectedAlignment = alignmentType;
            }
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
