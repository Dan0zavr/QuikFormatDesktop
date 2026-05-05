using QuikFormatDesktop.ViewModels.Navigation;
using QuikFormatDesktop.ViewModels.StylesViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace QuikFormatDesktop.ViewModels.Commands.ModalCommands
{
    public class OpenTemplateModalCommand<T> : ICommand where T : ViewModelBase
    {
        private readonly ModalNavigationService<T> _navigationService;

        public OpenTemplateModalCommand(ModalNavigationService<T> navigationService)
        {
            _navigationService = navigationService;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            _navigationService.Navigate();
        }
    }
}
