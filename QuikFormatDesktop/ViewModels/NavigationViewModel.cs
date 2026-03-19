using QuikFormatDesktop.ViewModels.Commands;
using QuikFormatDesktop.ViewModels.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace QuikFormatDesktop.ViewModels
{
    public class NavigationViewModel
    {

        public ICommand GoToStyles { get; }
        public ICommand GoToFormat { get; }

        public NavigationViewModel(NavigationService<FormatViewModel> navigationToFormatService, NavigationService<StylesViewModel> navigationToStylesService)
        {
            GoToStyles = new NavigateCommand<StylesViewModel>(navigationToStylesService);
            GoToFormat = new NavigateCommand<FormatViewModel>(navigationToFormatService);
        }
    }
}
