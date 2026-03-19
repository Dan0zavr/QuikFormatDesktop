using QuikFormatDesktop.ViewModels.StylesViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuikFormatDesktop.ViewModels
{
    public class StylesViewModel : ViewModelBase
    {
        private readonly Lazy<TextStyleViewModel> _textStyleViewModel = new Lazy<TextStyleViewModel>(() => new TextStyleViewModel());
        private readonly Lazy<TableStyleViewModel> _tableStyleViewModel = new Lazy<TableStyleViewModel>(() => new TableStyleViewModel());
        private readonly Lazy<PictureStyleViewModel> _pictureStyleViewModel = new Lazy<PictureStyleViewModel>(() => new PictureStyleViewModel());
        private readonly Lazy<NumberingStyleViewModel> _numberingStyleViewModel = new Lazy<NumberingStyleViewModel>(() => new NumberingStyleViewModel());
        private readonly Lazy<FormulaStyleViewModel> _formulaStyleViewModel = new Lazy<FormulaStyleViewModel>(() => new FormulaStyleViewModel());

        public TextStyleViewModel TextStyleViewModel => _textStyleViewModel.Value;
        public TableStyleViewModel TableStyleViewModel => _tableStyleViewModel.Value;
        public PictureStyleViewModel PictureStyleViewModel => _pictureStyleViewModel.Value;
        public NumberingStyleViewModel NumberingStyleViewModel => _numberingStyleViewModel.Value;
        public FormulaStyleViewModel FormulaStyleViewModel => _formulaStyleViewModel.Value;
    }
}
