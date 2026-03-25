using QuikFormatDesktop.Models;
using QuikFormatDesktop.ViewModels.Services;
using QuikFormatDesktop.Views.Pages;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuikFormatDesktop.ViewModels.StylesViewModels
{
    public class TextStyleViewModel : ViewModelBase, ILoadable, IResetable
    {
        private bool _isFontStyleVisible = true;
        private bool _isParagraphStyleVisible = true;

        public FontStyleViewModel FontStyleViewModel { get; }
        public ParagraphStyleViewModel ParagraphStyleViewModel { get; }

        public TextStyleViewModel(FontStyleViewModel fontStyleViewModel, ParagraphStyleViewModel paragraphStyleViewModel)
        {
            FontStyleViewModel = fontStyleViewModel;
            ParagraphStyleViewModel = paragraphStyleViewModel;
        }

        public bool IsFontStyleVisible 
        { 
            get => _isFontStyleVisible;
            set 
            { 
                _isFontStyleVisible = value; 
                OnPropertyChanged(nameof(IsFontStyleVisible));
            }
        }
        public bool IsParagraphStyleVisible 
        {
            get => _isParagraphStyleVisible;
            set
            {
                _isParagraphStyleVisible = value;
                OnPropertyChanged(nameof(IsParagraphStyleVisible));
            }
        }

        public void Reset()
        {
            IsParagraphStyleVisible = true;
            IsFontStyleVisible = true;

            FontStyleViewModel.Reset();
            ParagraphStyleViewModel.Reset();
        }

        public void Load(object parametr, bool isEdit = false)
        {
            Reset();

            if (parametr is TextStyle textStyle)
            {
                FontStyleViewModel.Load(textStyle, true);
                IsParagraphStyleVisible = false;
            }
            else if (parametr is ParagraphStyle paragraphStyle)
            {
                ParagraphStyleViewModel.Load(paragraphStyle, true);
                IsFontStyleVisible = false;
            }
        }
    }
}
