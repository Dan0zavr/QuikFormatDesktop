using QuikFormatDesktop.Models;
using QuikFormatDesktop.ViewModels.Commands;
using QuikFormatDesktop.ViewModels.Commands.ModalCommands;
using QuikFormatDesktop.ViewModels.Navigation;
using QuikFormatDesktop.ViewModels.Services;
using QuikFormatDesktop.ViewModels.StylesViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace QuikFormatDesktop.ViewModels.ShortMenuViewModels
{
    public class TemplateShortMenuViewModel : ShortMenuViewModelBase
    {
        private ModalNavigationService<DeleteWarningViewModel> _warningService;
        private ModalNavigationService<TemplateViewModel> _navigationService;
        private NavigationStore _navigationStore;

        private readonly TemplateService _templateService;

        private Template _template;

        public TemplateShortMenuViewModel(ModalNavigationService<DeleteWarningViewModel> warningService, ModalNavigationService<TemplateViewModel> navigationService, NavigationStore navigationStore, TemplateService templateService)
        {
            _warningService = warningService;
            _navigationService = navigationService;
            _navigationStore = navigationStore;
            _templateService = templateService;

            DeleteCommand = new RelayCommand<object?>(OpenDeleteWarning);
            DetailCommand = new RelayCommand<object?>(GoToDetails);
        }

        public ICommand DeleteCommand { get; }
        public ICommand DetailCommand { get; }

        public string Name => _template.Name;
        public string Description => _template.Description;

        public void Load(Template template)
        {
            if (template != null)
            {
                _template = template;
            }
        }

        private async Task DeleteTemplate(object? parameter)
        {
            await _templateService.Delete(_template);
            ClosePopup?.Invoke();
        }

        private void GoToDetails(object? parameter)
        {
            new OpenTemplateModalCommand(_navigationService).Execute(parameter);
            if (_navigationStore.CurrentModalViewModel is TemplateViewModel vm)
            {
                vm.InitializeAsync(_template, true);
            }
            ClosePopup?.Invoke();
        }

        private void OpenDeleteWarning(object? parameter)
        {
            new OpenDeleteWarningCommand(_warningService).Execute(parameter);
            if(_navigationStore.CurrentModalViewModel is DeleteWarningViewModel deleteWarning)
            {
                ClosePopup?.Invoke();
                deleteWarning.Load(_template);
                deleteWarning.DeleteCommand = new AsyncRelayCommand<object?>(DeleteTemplate);
            }
        }
    }
}
