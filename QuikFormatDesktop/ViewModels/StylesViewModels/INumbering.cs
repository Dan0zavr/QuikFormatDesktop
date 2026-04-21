using QuikFormatDesktop.Models;
using QuikFormatDesktop.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace QuikFormatDesktop.ViewModels.StylesViewModels
{
    public interface INumbering
    {
        public int StyleId { get; set; }
        public string NumberingStyleName { get; set; }
        public string OldStyleName { get; set; }
        public List<Marker> Markers { get; set; }
        public Marker SelectedMarker { get; set; }
        public bool IsPopupOpen { get; set; }
        public string PopupMessage { get; set; }
        public Color PopupBackground {  get; set; }
        public Color PopupForeground { get; set; }
        public event Action RequestReset;
        void RaiseRequestReset();
    }
}
