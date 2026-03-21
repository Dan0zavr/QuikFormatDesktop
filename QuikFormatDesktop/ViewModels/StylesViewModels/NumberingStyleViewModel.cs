using System;
using System.Collections.Generic;
using System.Text;

namespace QuikFormatDesktop.ViewModels.StylesViewModels
{
    public class NumberingStyleViewModel : ViewModelBase
    {
        public NumberingStyleViewModel(MarkedNumberingStyleViewModel markedNumberingStyle, NumberedNumberingStyleViewModel numberedNumberingStyle)
        {
            MarkedNumberingStyle = markedNumberingStyle;
            NumberedNumberingStyle = numberedNumberingStyle;
        }

        public MarkedNumberingStyleViewModel MarkedNumberingStyle { get; set; }
        public NumberedNumberingStyleViewModel NumberedNumberingStyle { get; set; }
    }
}
