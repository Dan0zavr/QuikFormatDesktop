using QuikFormatDesktop.ViewModels.Navigation;
using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;
using System.Windows.Input;

namespace QuikFormatDesktop.ViewModels.Commands
{
    public class NavigateCommand : ICommand
    {
        private NavigationService _navigationService;

        public NavigateCommand(NavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            _navigationService.Navigate();
        }


        public event EventHandler? CanExecuteChanged;
    }
}
