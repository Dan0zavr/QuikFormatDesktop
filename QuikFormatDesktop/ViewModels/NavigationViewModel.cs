using QuikFormatDesktop.Models;
using QuikFormatDesktop.ViewModels.Commands;
using QuikFormatDesktop.ViewModels.Navigation;
using QuikFormatDesktop.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace QuikFormatDesktop.ViewModels
{
    public class NavigationViewModel
    {
        private readonly TemplateService _templateService;
        private ObservableCollection<Template> _templates = new ObservableCollection<Template>();
        public ICommand GoToStyles { get; }
        public ICommand GoToFormat { get; }

        public NavigationViewModel(NavigationService<FormatViewModel> navigationToFormatService, NavigationService<StylesViewModel> navigationToStylesService, 
            TemplateService templateService)
        {
            _templateService = templateService;

            GoToStyles = new NavigateCommand<StylesViewModel>(navigationToStylesService);
            GoToFormat = new NavigateCommand<FormatViewModel>(navigationToFormatService);

           LoadTemplates();
        }

        private async void LoadTemplates()
        {
            var templates =  await _templateService.GetAll();
            _templates = new ObservableCollection<Template>(templates);
        }
    }
}
