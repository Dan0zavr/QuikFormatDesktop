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
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace QuikFormatDesktop.ViewModels.StylesViewModels
{
    public class TableStyleViewModel : ViewModelBase, ILoadable, IResetable
    {
        private readonly TextService _textService;
        private readonly ParagraphService _paragraphService;
        private readonly TableService _tableService;
        private readonly AlignmentService _alignmentService;
        private readonly IDialogService _dialogService;

        private string _tableStyleName;
        private ParagraphStyle _selectedParagraphStyle;
        private TextStyle _selectedTextStyle;
        private AlignmentType _selectedAlignment;
        private double _padding;
        private int _borderThikness;
        private string _borderColor;
        private string _popupMessage;
        private IOptions<TableSettings> _options;

        private bool _isEdit = false;

        private bool _isPopupOpen = false;
        private Color _popupBackground;
        private Color _popupForeground;

        private ObservableCollection<ParagraphStyle> _paragraphStyles = new ObservableCollection<ParagraphStyle>();
        private ObservableCollection<TextStyle> _textStyles = new ObservableCollection<TextStyle>();
        public TableStyleViewModel(TableService tableService, AlignmentService alignmentService, IDialogService dialogService, 
            TextService textService, ParagraphService paragraphService, IOptions<TableSettings> options)
        {
            _textService = textService;
            _paragraphService = paragraphService;
            _tableService = tableService;
            _alignmentService = alignmentService;
            _dialogService = dialogService;
            _options = options;

            LoadTextStyles();
            LoadParagraphStyles();

            SetDefault(_options);

            AddTableCommand = new AsyncRelayCommand<object?>(AddTableStyleAsync, CanAddTableStyle);
            UpdateTableCommand = new AsyncRelayCommand<object?>(UpdateTableStyleAsync, CanAddTableStyle);
            ResetCommand = new RelayCommand<object?>(Reset);
        }

        public ICommand ResetCommand { get; }
        public ICommand AddTableCommand { get; }
        public ICommand UpdateTableCommand { get; }

        public string CardName
        {
            get
            {
                return IsEdit ? "Редактирование стиля таблицы" : "Новый стиль таблицы";
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
        public string TableStyleName
        {
            get => _tableStyleName;
            set
            {
                _tableStyleName = value;
                OnPropertyChanged(nameof(TableStyleName));
                (AddTableCommand as IAsyncRelayCommand)?.NotifyCanExecuteChanged();
                (UpdateTableCommand as IAsyncRelayCommand)?.NotifyCanExecuteChanged();
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

        public AlignmentType SelectedAlignment
        {
            get => _selectedAlignment;
            set
            {
                _selectedAlignment = value;
                OnPropertyChanged(nameof(SelectedAlignment));
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

        public string BorderColor
        {
            get => _borderColor;
            set
            {
                _borderColor = value;
                OnPropertyChanged(nameof(BorderColor));
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

        private async Task LoadTextStyles()
        {
            List<TextStyle> textStyles = await _textService.GetAll();

            _textStyles.Clear();
            foreach (var t in textStyles)
            {
                _textStyles.Add(t);
            }
        }

        private async Task LoadParagraphStyles()
        {
            List<ParagraphStyle> paragraphStyles = await _paragraphService.GetAll();

            _paragraphStyles.Clear();
            foreach (var p in paragraphStyles)
            {
                _paragraphStyles.Add(p);
            }
        }

        private bool CanAddTableStyle(object? parameter)
        {
            return !string.IsNullOrWhiteSpace(TableStyleName);
        }

        private async Task AddTableStyleAsync(object? parameter)
        {
            try
            {
                int alignmentId = await _alignmentService.GetIdByType(_selectedAlignment);

                TableStyle tableStyle = new TableStyle
                {
                    Name = TableStyleName,
                    TextStyle = SelectedTextStyle.Id,
                    ParagraphStyle = SelectedParagraphStyle.Id,
                    Alignment = alignmentId,
                    BorderThikness = this.BorderThikness,
                    BorderColor = this.BorderColor,
                    CellPadding = Padding
                };

                if (await _tableService.IsUnique(tableStyle.Name))
                {
                    await _tableService.Add(tableStyle);
                    await ShowPopup("Стиль успешно добавлен", PopupType.Good);
                }
                else
                {
                    await ShowPopup("Стиль с таким именем уже существует", PopupType.Bad);
                }
            }
            catch (AlignmentNotFoundException fex)
            {
                _dialogService.ShowError(fex.Message);
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

        private async Task UpdateTableStyleAsync(object? parameter)
        {
            try
            {
                int alignmentId = await _alignmentService.GetIdByType(_selectedAlignment);

                TableStyle tableStyle = new TableStyle
                {
                    Id = StyleId,
                    Name = TableStyleName,
                    TextStyle = SelectedTextStyle.Id,
                    ParagraphStyle = SelectedParagraphStyle.Id,
                    Alignment = alignmentId,
                    BorderThikness = this.BorderThikness,
                    BorderColor = this.BorderColor,
                    CellPadding = Padding
                };

                if (await _tableService.IsUnique(tableStyle.Name))
                {
                    await _tableService.Update(tableStyle);
                    await ShowPopup("Стиль успешно обновлен", PopupType.Good);
                }
                else
                {
                    await ShowPopup("Стиль с таким именем уже существует", PopupType.Bad);
                }
            }
            catch (AlignmentNotFoundException fex)
            {
                _dialogService.ShowError(fex.Message);
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

        private void SetDefault(IOptions<TableSettings> options)
        {
            IsEdit = false;

            TableStyleName = string.Empty;
            SelectedParagraphStyle = ParagraphStyles.FirstOrDefault();
            SelectedTextStyle = TextStyles.FirstOrDefault();

            BorderThikness = options.Value.DefaultBorderThikness;
            BorderColor = options.Value.DefaultBorderColor;
            Padding = options.Value.CellPadding;

            SelectedAlignment = AlignmentType.Center;
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

            if (parametr is TableStyle tableStyle)
            {
                StyleId = tableStyle.Id;
                TableStyleName = tableStyle.Name;
                Enum.TryParse(_alignmentService.GetById(tableStyle.Alignment).GetAwaiter().GetResult().Alignment1, true, out AlignmentType alignment);

                SelectedAlignment = alignment;
                Padding = tableStyle.CellPadding;
                BorderThikness = tableStyle.BorderThikness;
                BorderColor = tableStyle.BorderColor;

                SelectedTextStyle = _textStyles.Where(x => x.Id == tableStyle.TextStyle).FirstOrDefault();
                SelectedParagraphStyle = _paragraphStyles.Where(x => x.Id == tableStyle.ParagraphStyle).FirstOrDefault();
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
