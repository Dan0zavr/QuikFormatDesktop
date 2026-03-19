using QuikFormatDesktop.ViewModels.Navigation;
using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;
using System.Windows.Input;

namespace QuikFormatDesktop.ViewModels.Commands
{
    public class NavigateCommand<T> : ICommand where T : ViewModelBase
    {
        private NavigationService<T> _navigationService;

        public NavigateCommand(NavigationService<T> navigationService)
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
