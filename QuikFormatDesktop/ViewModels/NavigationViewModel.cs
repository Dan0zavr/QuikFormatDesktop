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

        public NavigationViewModel(NavigationService navigationToFormatService, NavigationService navigationToStylesService)
        {
            GoToStyles = new NavigateCommand(navigationToStylesService);
            GoToFormat = new NavigateCommand(navigationToFormatService);
        }
    }
}
