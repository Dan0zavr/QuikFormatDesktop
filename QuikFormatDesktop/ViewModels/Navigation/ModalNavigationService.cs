using Microsoft.Extensions.DependencyInjection;
using QuikFormatDesktop.ViewModels.StylesViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuikFormatDesktop.ViewModels.Navigation
{
    public class ModalNavigationService<T> where T : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly IServiceProvider _serviceProvider;

        public ModalNavigationService(NavigationStore navigationStore, IServiceProvider serviceProvider)
        {
            _navigationStore = navigationStore;
            _serviceProvider = serviceProvider;
        }

        public void Navigate()
        {
            _navigationStore.CurrentModalViewModel = _serviceProvider.GetRequiredService<T>();

            if (_navigationStore.CurrentModalViewModel is SystemTemplateViewModel systemTemplate)
            {
                systemTemplate.InitializeAsync();
            }
            else if (_navigationStore.CurrentModalViewModel is TemplateViewModel templateViewModel)
            {
                templateViewModel.InitializeAsync();
            }
        }
    }
}
