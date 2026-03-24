using QuikFormatDesktop.Models;
using QuikFormatDesktop.ViewModels.Commands;
using QuikFormatDesktop.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace QuikFormatDesktop.ViewModels.ShortMenuViewModels
{
    public class FormulaShortMenuViewModel : ViewModelBase
    {
        private readonly FormulaStyle _formulaStyle;
        private readonly FormulaService _formulaService;
        private readonly PositionService _positionService;
        private readonly MarkerService _markerService;

        public FormulaShortMenuViewModel(FormulaStyle formulaStyle, FormulaService formulaService, PositionService positionService, MarkerService markerService)
        {
            _formulaStyle = formulaStyle;
            _formulaService = formulaService;
            _positionService = positionService;
            _markerService = markerService;
            DeleteFormulaStyleCommand = new AsyncRelayCommand(DeleteFormulaStyle);
        }

        public ICommand DeleteFormulaStyleCommand { get; }

        public string Name => _formulaStyle.Name;
        public bool Numeration => _formulaStyle.Numeration;
        public bool EmptyLineAround => _formulaStyle.EmptyLineAround;
        public string Marker
        {
            get 
            {
                if (Numeration)
                {
                    return _markerService.GetById((int)_formulaStyle.Marker).GetAwaiter().GetResult().Marker1;
                }
                return "";
            }
        }
        public string Position => PositionToView(_positionService.GetById(_formulaStyle.Position).GetAwaiter().GetResult().Position1);
        private string PositionToView(string position)
        {
            switch (position.ToLower())
            {
                case "centerright":
                    return "Формула по центру, номер справа";
                case "centerleft":
                    return "Формула по центру, номер слева";
                case "leftright":
                    return "Формула слева, номер справа";
                case "rightleft":
                    return "Формула справа, номер слева";
                default:
                    return "";
            }
        }

        private async Task DeleteFormulaStyle(object? parametr)
        {
            await _formulaService.Delete(_formulaStyle);
        }
    }
}
