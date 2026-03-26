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
        private string _pStatusMessage;


        public FormulaStyleViewModel(FormulaService formulaService, PositionService positionService, MarkerService markerService, IDialogService dialogService)
        {
            _formulaService = formulaService;
            _positionService = positionService;
            _markerService = markerService;
            _dialogService = dialogService;

            SetDefault();

            AddFormulaCommand = new AsyncRelayCommand(AddFormulaStyle, CanAddFormulaStyle);
        }

        public ICommand FormulaDeleteCommand { get; }
        public ICommand AddFormulaCommand {  get; }

        public bool IsEdit { get; set; } = false;

        public string FormulaStyleName
        {
            get => _formulaStyleName;
            set
            {
                _formulaStyleName = value;
                OnPropertyChanged(nameof(FormulaStyleName));
                (AddFormulaCommand as AsyncRelayCommand)?.RaiseCanExecuteChanged();
            }
        }

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

        public string PStatusMessage
        {
            get => _pStatusMessage;
            set
            {
                _pStatusMessage = value;
                OnPropertyChanged(nameof(PStatusMessage));
            }
        }

        private async Task SetDefault()
        {
            _formulaStyleName = null;
            _selectedPosition = PositionType.CenterRight;
            _insertBlankLines = false;
            _isNumberingEnabled = true;
            _numberingFormats = await _markerService.GetByType(MarkerTypeEnum.Numberd);
            _selectedNumberingFormat = _numberingFormats.FirstOrDefault();
        }

        private bool CanAddFormulaStyle(object? parametr)
        {
            if (!string.IsNullOrWhiteSpace(FormulaStyleName))
            {
                return true;
            }
            return false;
        }

        private async Task AddFormulaStyle(object? parametr)
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
                    PStatusMessage = "Стиль успешно добавлен";
                }
                else
                {
                    PStatusMessage = "Стиль с таким именем уже существует";
                }
            }
            catch (Exception ex)
            {
                _dialogService.ShowError($"Ошибка. Код: {ex.HResult}");
            }
            
        }

        public void Load(object parametr, bool isEdit)
        {
            IsEdit = isEdit;

            Reset();

            if (parametr is FormulaStyle formulaStyle)
            {
                FormulaStyleName = formulaStyle.Name;
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
    }
}
