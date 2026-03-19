using QuikFormatDesktop.Models;
using QuikFormatDesktop.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuikFormatDesktop.ViewModels.StylesViewModels
{
    public class FontStyleViewModel : ViewModelBase
    {
        private readonly TextService _textStyleService;

        private string _styleName;
        private Font _selectedFont;
        private int _selectedFontSize;

        private List<Font> _fonts = new List<Font>();

        public FontStyleViewModel(TextService textStyleService)
        {
            _textStyleService = textStyleService;
        }

        public string StyleName
        {
            get => _styleName;
            set
            {
                _styleName = value;
                OnPropertyChanged(nameof(StyleName));
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
    }
}
