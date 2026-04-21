using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Options;
using QuikFormatDesktop.Exceptions;
using QuikFormatDesktop.Models;
using QuikFormatDesktop.Models.SupportModels;
using QuikFormatDesktop.ViewModels.Enums;
using QuikFormatDesktop.ViewModels.Services;
using System.Windows.Input;
using System.Windows.Media;

namespace QuikFormatDesktop.ViewModels.StylesViewModels
{
    public class FontStyleViewModel : ViewModelBase, IResetable
    {
        public readonly TextService _textStyleService;
        public readonly FontService _fontService;
        public readonly IDialogService _dialogService;

        private string _styleName;
        private Font _selectedFont;
        private int _selectedFontSize;
        private string _popupMessage;

        private bool _isEdit = false;

        private List<Font> _fonts = new List<Font>();
        private List<int> _fontSizes = new List<int>();

        private bool _isPopupOpen = false;
        private Color _popupBackground;
        private Color _popupForeground;

        public ICommand AddTextCommand { get; }
        public ICommand UpdateTextCommand{ get; }
        public ICommand ResetCommand { get; }
        public ICommand CancelCommand { get; }

        public FontStyleViewModel(TextService textStyleService, FontService fontService, IDialogService dialogService, IOptions<FontSettings> options)
        {
            _textStyleService = textStyleService;
            _fontService = fontService;
            _dialogService = dialogService;
            LoadFonts(options);

            AddTextCommand = new AsyncRelayCommand<object?>(AddTextStyleAsync, CanAddTextStyle);
            UpdateTextCommand= new AsyncRelayCommand<object?>(UpdateTextStyleAsync, CanAddTextStyle);
            ResetCommand = new RelayCommand<object?>(Reset);
            CancelCommand = new RelayCommand<object?>( _ => RequestReset?.Invoke());
            
        }

        public event Action RequestReset;

        public string CardName
        {
            get
            {
                return IsEdit ? "Редактирование стиля шрифта" : "Новый стиль шрифта";
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

        public string StyleName
        {
            get => _styleName;
            set
            {
                _styleName = value;
                OnPropertyChanged(nameof(StyleName));
                (AddTextCommand as IAsyncRelayCommand)?.NotifyCanExecuteChanged();
                (UpdateTextCommand as IAsyncRelayCommand)?.NotifyCanExecuteChanged();
            }
        }

        private string OldStyleName { get; set; }

        public Font SelectedFont
        {
            get => _selectedFont;
            set
            {
                _selectedFont = value;
                OnPropertyChanged(nameof(SelectedFont));
            }
        }

        public List<Font> Fonts
        {
            get => _fonts;
            set
            {
                _fonts = value;
                OnPropertyChanged(nameof(Fonts));
            }
        }

        public int SelectedFontSize
        {
            get => _selectedFontSize;
            set
            {
                _selectedFontSize = value;
                OnPropertyChanged(nameof(SelectedFontSize));
            }
        }

        public List<int> FontSizes
        {
            get => _fontSizes;
            set
            {
                _fontSizes = value;
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

        private async Task LoadFonts(IOptions<FontSettings> options)
        {
            _fontSizes = options.Value.AllowedSizes;
            _selectedFontSize = options.Value.DefaultSize;

            _fonts = await _fontService.GetAll();
            _selectedFont = _fonts.FirstOrDefault(f => f.FontName == options.Value.DefaultName);
        }

        private bool CanAddTextStyle(object? parameter)
        {
            return !string.IsNullOrWhiteSpace(StyleName);
        }

        private async Task AddTextStyleAsync(object? parameter)
        {
            try
            {
                int fontId = await _fontService.GetIdByName(SelectedFont.FontName);

                var textStyle = new TextStyle
                {
                    Name = StyleName,
                    Font = fontId,
                    FontSize = SelectedFontSize
                };

                if (await _textStyleService.IsUnique(textStyle.Name))
                {
                    await _textStyleService.Add(textStyle);
                    await ShowPopup("Стиль успешно добавлен", PopupType.Good);
                }
                else
                {
                    await ShowPopup("Стиль с таким именем уже существует", PopupType.Bad);
                }
            }
            catch (FontNotFoundException fex)
            {
                _dialogService.ShowError(fex.Message);
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

        private async Task UpdateTextStyleAsync(object? parametr)
        {
            try
            {
                int fontId = await _fontService.GetIdByName(SelectedFont.FontName);

                var textStyle = new TextStyle
                {
                    Id = StyleId,
                    Name = StyleName,
                    Font = fontId,
                    FontSize = SelectedFontSize
                };

                bool isUnique = true;
                if(OldStyleName != textStyle.Name)
                {
                    isUnique = await _textStyleService.IsUnique(textStyle.Name);
                }

                if (isUnique)
                {
                    await _textStyleService.Update(textStyle);
                    await ShowPopup("Стиль успешно обновлен", PopupType.Good);
                }
                else
                {
                    await ShowPopup("Стиль с таким именем уже существует", PopupType.Bad);
                }
            }
            catch (FontNotFoundException fex)
            {
                _dialogService.ShowError(fex.Message);
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
            StyleName = string.Empty;
            SelectedFont = Fonts.FirstOrDefault();
            SelectedFontSize = 14;
        }

        public void Reset(object? parameter)
        {
            Reset();
        }

        public void Load(TextStyle textStyle, bool isEdit)
        {
            IsEdit = isEdit;

            StyleId = textStyle.Id;
            StyleName = textStyle.Name;
            OldStyleName = textStyle.Name;
            SelectedFont = Fonts.Where(x => x.Id == textStyle.Font).FirstOrDefault();
            SelectedFontSize = textStyle.FontSize;
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
