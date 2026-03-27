using System;
using System.Collections.Generic;
using System.Text;

namespace QuikFormatDesktop.ViewModels.ShortMenuViewModels
{
    public class ShortMenuViewModelBase : ViewModelBase
    {
        public Action ClosePopup { get; set; }
    }
}
