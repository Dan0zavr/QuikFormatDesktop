using QuikFormatDesktop.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuikFormatDesktop.ViewModels.StylesViewModels
{
    public class TextStyleViewModel : ViewModelBase
    {
        public FontStyleViewModel FontStyleViewModel { get; }
        public ParagraphStyleViewModel ParagraphStyleViewModel { get; }

        public TextStyleViewModel(TextService textStyleService, ParagraphService paragraphService)
        {
            FontStyleViewModel = new FontStyleViewModel(textStyleService);
            ParagraphStyleViewModel = new ParagraphStyleViewModel(paragraphService);
        }
    }
}
