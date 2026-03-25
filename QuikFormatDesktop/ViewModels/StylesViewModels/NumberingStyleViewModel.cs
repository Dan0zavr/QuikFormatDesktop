using QuikFormatDesktop.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuikFormatDesktop.ViewModels.StylesViewModels
{
    public class NumberingStyleViewModel : ViewModelBase, ILoadable, IResetable
    {
        private bool _isNumberedStyleVisible = true;
        private bool _isMarkedStyleVisible = true;

        public NumberingStyleViewModel(MarkedNumberingStyleViewModel markedNumberingStyle, NumberedNumberingStyleViewModel numberedNumberingStyle)
        {
            MarkedNumberingStyle = markedNumberingStyle;
            NumberedNumberingStyle = numberedNumberingStyle;
        }

        public MarkedNumberingStyleViewModel MarkedNumberingStyle { get; set; }
        public NumberedNumberingStyleViewModel NumberedNumberingStyle { get; set; }

        public bool IsNumberedStyleVisible 
        { 
            get 
            {
                return _isNumberedStyleVisible;
            }
            set
            {
                _isNumberedStyleVisible = value;
                OnPropertyChanged(nameof(IsNumberedStyleVisible));
            }
        }

        public bool IsMarkedStyleVisible { 
            get
            {
                return _isMarkedStyleVisible;
            }
            set
            {
                _isMarkedStyleVisible = value;
                OnPropertyChanged(nameof(IsMarkedStyleVisible));
            }
        }

        public void Load(object parametr, bool isEdit = false)
        {
            Reset();

            if (parametr is StyleObject numberingStyle)
            {

                if (numberingStyle.Type == Enums.StyleType.NumberedNumbering)
                {
                    NumberedNumberingStyle.Load((NumberingStyle)numberingStyle, true);
                    _isMarkedStyleVisible = false;

                }
                else
                {
                    MarkedNumberingStyle.Load((NumberingStyle)numberingStyle, true);
                    _isNumberedStyleVisible = false;
                }
            }
        }

        public void Reset()
        {
            IsNumberedStyleVisible = true;
            IsMarkedStyleVisible = true;

            NumberedNumberingStyle.Reset();
            MarkedNumberingStyle.Reset();
        }
    }
}
