using Microsoft.Extensions.DependencyInjection;
using QuikFormatDesktop.ViewModels.StylesViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using QuikFormatDesktop.ViewModels.Enums;

namespace QuikFormatDesktop.ViewModels
{
    public class StylesViewModel : ViewModelBase
    {
        private readonly IServiceProvider _serviceProvider;

        private int _selectedTabIndex;

        private TextStyleViewModel _textStyleViewModel;
        private TableStyleViewModel _tableStyleViewModel;
        private PictureStyleViewModel _pictureStyleViewModel;
        private NumberingStyleViewModel _numberingStyleViewModel;
        private FormulaStyleViewModel _formulaStyleViewModel;

        public StylesViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        
        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set
            {
                _selectedTabIndex = value;
                OnPropertyChanged(nameof(SelectedTabIndex));
            }
        }

        public TabItemIndex SelectedTab => (TabItemIndex)SelectedTabIndex;

        public TextStyleViewModel TextStyleViewModel =>
            _textStyleViewModel ??= _serviceProvider.GetRequiredService<TextStyleViewModel>();

        public TableStyleViewModel TableStyleViewModel =>
            _tableStyleViewModel ??= _serviceProvider.GetRequiredService<TableStyleViewModel>();

        public PictureStyleViewModel PictureStyleViewModel =>
            _pictureStyleViewModel ??= _serviceProvider.GetRequiredService<PictureStyleViewModel>();

        public NumberingStyleViewModel NumberingStyleViewModel =>
            _numberingStyleViewModel ??= _serviceProvider.GetRequiredService<NumberingStyleViewModel>();

        public FormulaStyleViewModel FormulaStyleViewModel =>
            _formulaStyleViewModel ??= _serviceProvider.GetRequiredService<FormulaStyleViewModel>();
    }
}
