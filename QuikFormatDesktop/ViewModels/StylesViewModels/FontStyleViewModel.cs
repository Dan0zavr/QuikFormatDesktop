using Microsoft.Extensions.Options;
using QuikFormatDesktop.Exceptions;
using QuikFormatDesktop.Models;
using QuikFormatDesktop.Models.SupportModels;
using QuikFormatDesktop.ViewModels.Commands;
using QuikFormatDesktop.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace QuikFormatDesktop.ViewModels.StylesViewModels
{
    public class FontStyleViewModel : ViewModelBase, IResetable
    {
        public readonly TextService textStyleService;
        public readonly FontService fontService;
        public readonly IDialogService dialogService;

        private string _styleName;
        private Font _selectedFont;
        private int _selectedFontSize;
        private string _pStatusMessage;

        private List<Font> _fonts = new List<Font>();
        private List<int> _fontSizes = new List<int>();

        public ICommand AddTextStyle { get; }

        public FontStyleViewModel(TextService DiTextStyleService, FontService DiFontService, IDialogService DiDialogService, IOptions<FontSettings> options)
        {
            textStyleService = DiTextStyleService;
            fontService = DiFontService;
            dialogService = DiDialogService;
            LoadFonts(options);

            AddTextStyle = new AsyncRelayCommand(AddTextStyleAsync, CanAddTextStyle);
        }

        public bool IsEdit { get; set; } = false;

        public string StyleName
        {
            get => _styleName;
            set
            {
                _styleName = value;
                OnPropertyChanged(nameof(StyleName));
                (AddTextStyle as AsyncRelayCommand)?.RaiseCanExecuteChanged();
            }
        }

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

        public string PStatusMessage
        {
            get => _pStatusMessage;
            set
            {
                _pStatusMessage = value;
                OnPropertyChanged(nameof(PStatusMessage));
            }
        }

        private async Task LoadFonts(IOptions<FontSettings> options)
        {
            _fontSizes = options.Value.AllowedSizes;
            _selectedFontSize = options.Value.DefaultSize;

            _fonts = await fontService.GetAll();
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
                int fontId = await fontService.GetIdByName(SelectedFont.FontName);

                var textStyle = new TextStyle
                {
                    Name = StyleName,
                    Font = fontId,
                    FontSize = SelectedFontSize
                };

                if (await textStyleService.IsUnique(textStyle.Name))
                {
                    await textStyleService.Add(textStyle);
                    PStatusMessage = "Стиль успешно добавлен";
                }
                else
                {
                    PStatusMessage = "Стиль с таким именем уже существует";
                }
            }
            catch (FontNotFoundException fex)
            {
                dialogService.ShowError(fex.Message);
            }
            catch (Exception ex)
            {
                dialogService.ShowError($"Ошибка. Код: {ex.HResult}");
            }
        }

        public void Reset()
        {
            StyleName = null;
            SelectedFont = Fonts.FirstOrDefault();
            SelectedFontSize = 14;
        }

        public void Load(TextStyle textStyle, bool isEdit)
        {
            IsEdit = isEdit;

            StyleName = textStyle.Name;
            SelectedFont = textStyle.FontNavigation;
            SelectedFontSize = textStyle.FontSize;
        }
    }
}
