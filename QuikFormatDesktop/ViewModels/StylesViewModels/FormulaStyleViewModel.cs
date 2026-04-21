using CommunityToolkit.Mvvm.Input;
using QuikFormatDesktop.Models;
using QuikFormatDesktop.ViewModels.Commands;
using QuikFormatDesktop.ViewModels.Enums;
using QuikFormatDesktop.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Printing;
using System.Security.AccessControl;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;

namespace QuikFormatDesktop.ViewModels.StylesViewModels
{
    public class FormulaStyleViewModel : ViewModelBase, ILoadable, IResetable
    {
        private readonly FormulaService _formulaService;
        private readonly PositionService _positionService;
        private readonly MarkerService _markerService;
        private readonly IDialogService _dialogService;

        private string _formulaStyleName;
        private PositionType _selectedPosition;
        private bool _insertBlankLines;
        private bool _isNumberingEnabled;
        private List<Marker> _numberingFormats;
        private Marker _selectedNumberingFormat;

        private string _popupMessage;
        private bool _isPopupOpen = false;
        private Color _popupBackground;
        private Color _popupForeground;

        private bool _isEdit = false;

        public FormulaStyleViewModel(FormulaService formulaService, PositionService positionService, MarkerService markerService, IDialogService dialogService)
        {
            _formulaService = formulaService;
            _positionService = positionService;
            _markerService = markerService;
            _dialogService = dialogService;

            SetDefault();

            AddFormulaCommand = new AsyncRelayCommand<object?>(AddFormulaStyle, CanAddFormulaStyle);
            UpdateFormulaCommand = new AsyncRelayCommand<object?>(UpdateFormulaStyle,CanAddFormulaStyle);
            ResetCommand = new RelayCommand<object?>(Reset);
        }

        public ICommand ResetCommand { get; }
        public ICommand AddFormulaCommand {  get; }
        public ICommand UpdateFormulaCommand { get; }

        public string CardName
        {
            get
            {
                return IsEdit ? "Редактирование стиля формулы" : "Новый стиль формулы";
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
        public string FormulaStyleName
        {
            get => _formulaStyleName;
            set
            {
                _formulaStyleName = value;
                OnPropertyChanged(nameof(FormulaStyleName));
                (AddFormulaCommand as IAsyncRelayCommand)?.NotifyCanExecuteChanged();
                (UpdateFormulaCommand as IAsyncRelayCommand)?.NotifyCanExecuteChanged();
            }
        }
        private string OldStyleName { get; set; }

        public PositionType SelectedPosition
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

        public List<Marker> NumberingFormats
        {
            get => _numberingFormats;
            set
            {
                _numberingFormats = value;
                OnPropertyChanged(nameof(NumberingFormats));
            }
        }

        public Marker SelectedNumberingFormat
        {
            get => _selectedNumberingFormat;
            set
            {
                _selectedNumberingFormat = value;
                OnPropertyChanged(nameof(SelectedNumberingFormat));
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

        private async Task SetDefault()
        {
            IsEdit = false;
            FormulaStyleName = string.Empty;
            SelectedPosition = PositionType.CenterRight;
            InsertBlankLines = false;
            IsNumberingEnabled = true;
            NumberingFormats = await _markerService.GetByType(MarkerTypeEnum.Numberd);
            SelectedNumberingFormat = _numberingFormats.FirstOrDefault();
        }

        private bool CanAddFormulaStyle(object? parameter)
        {
            if (!string.IsNullOrWhiteSpace(FormulaStyleName))
            {
                return true;
            }
            return false;
        }

        private async Task AddFormulaStyle(object? parameter)
        {
            try
            {
                int positionId = await _positionService.GetIdByType(_selectedPosition);

                FormulaStyle formulaStyle = new FormulaStyle
                {
                    Name = FormulaStyleName,
                    EmptyLineAround = InsertBlankLines,
                    Numeration = IsNumberingEnabled,
                    Marker = SelectedNumberingFormat.Id,
                    Position = positionId
                };

                if (await _formulaService.IsUnique(formulaStyle.Name))
                {
                    await _formulaService.Add(formulaStyle);
                    await ShowPopup("Стиль успешно добавлен", PopupType.Good);
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

        private async Task UpdateFormulaStyle(object? parameter)
        {
            try
            {
                int positionId = await _positionService.GetIdByType(_selectedPosition);

                FormulaStyle formulaStyle = new FormulaStyle
                {
                    Id = StyleId,
                    Name = FormulaStyleName,
                    EmptyLineAround = InsertBlankLines,
                    Numeration = IsNumberingEnabled,
                    Marker = SelectedNumberingFormat.Id,
                    Position = positionId
                };

                bool isUnique = true;

                if (OldStyleName != formulaStyle.Name)
                {
                    isUnique = await _formulaService.IsUnique(formulaStyle.Name);
                }

                if (isUnique)
                {
                    await _formulaService.Update(formulaStyle);
                    await ShowPopup("Стиль успешно обновлен", PopupType.Good);
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

        public void Load(object parametr, bool isEdit)
        {
            Reset();
            IsEdit = isEdit;

            if (parametr is FormulaStyle formulaStyle)
            {
                StyleId = formulaStyle.Id;
                FormulaStyleName = formulaStyle.Name;
                OldStyleName = formulaStyle.Name;
                Enum.TryParse(_positionService.GetById(formulaStyle.Position).GetAwaiter().GetResult().Position1, true, out PositionType position);
                SelectedPosition = position;
                InsertBlankLines = formulaStyle.EmptyLineAround;
                IsNumberingEnabled = formulaStyle.Numeration;
                if (IsNumberingEnabled)
                {
                    SelectedNumberingFormat = _numberingFormats.Where(x => x.Id == formulaStyle.Marker).FirstOrDefault();
                }
            }
        }

        public void Reset()
        {
            SetDefault();
        }

        public void Reset(object? parameter)
        {
            Reset();
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
