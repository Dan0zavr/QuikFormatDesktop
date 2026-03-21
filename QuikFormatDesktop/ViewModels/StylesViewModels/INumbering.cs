using QuikFormatDesktop.Models;
using QuikFormatDesktop.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuikFormatDesktop.ViewModels.StylesViewModels
{
    public interface INumbering
    {
        public string NumberingStyleName { get; set; }
        public List<Marker> Markers { get; set; }
        public Marker SelectedMarker { get; set; }
        public string PStatusMessage { get; set; }
    }
}
