using CommunityToolkit.Mvvm.Input;
using QuikFormatDesktop.Models;
using QuikFormatDesktop.ViewModels.Commands.ModalCommands;
using QuikFormatDesktop.ViewModels.FormatViewModels;
using QuikFormatDesktop.ViewModels.Navigation;
using QuikFormatDesktop.ViewModels.Services;
using QuikFormatDesktop.ViewModels.StylesViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace QuikFormatDesktop.ViewModels.ShortMenuViewModels
{
    public class SystemTemplateShortMenuViewModel : ShortMenuViewModelBase
    {
        private ModalNavigationService<SystemTemplateViewModel> _navigationService;
        private NavigationStore _navigationStore;

        private Template _template;

        public SystemTemplateShortMenuViewModel(ModalNavigationService<SystemTemplateViewModel> navigationService, NavigationStore navigationStore)
        {
            _navigationService = navigationService;
            _navigationStore = navigationStore;

            DetailCommand = new RelayCommand<object?>(GoToDetails);
            SelectCommand = new AsyncRelayCommand(TransferTemplate);
        }

        public ICommand DetailCommand { get; }
        public ICommand SelectCommand { get; }

        public string Name => _template.Name;
        public string Description => _template.Description;

        public void Load(Template template)
        {
            if (template != null)
            {
                _template = template;
            }
        }

        private void GoToDetails(object? parameter)
        {
            new OpenTemplateModalCommand<SystemTemplateViewModel>(_navigationService).Execute(parameter);
            if (_navigationStore.CurrentModalViewModel is SystemTemplateViewModel vm)
            {
                vm.InitializeAsync(_template, true);
            }
            ClosePopup?.Invoke();
        }

        private async Task TransferTemplate()
        {
            if (_navigationStore.CurrentViewModel is FormatViewModel format)
            {
                await format.SelectorCardViewModel.LoadSystemTemplate(_template);
                ClosePopup?.Invoke();
            }
        }
    }
}
