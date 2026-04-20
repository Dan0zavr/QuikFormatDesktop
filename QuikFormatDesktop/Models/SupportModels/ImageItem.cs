using QuikFormatDesktop.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;

namespace QuikFormatDesktop.Models.SupportModels
{
    public class ImageItem : ViewModelBase
    {
        public BitmapImage Image { get; set; }
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
    }
}
