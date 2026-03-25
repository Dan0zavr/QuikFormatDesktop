using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuikFormatDesktop.ViewModels.Navigation
{
    public class NavigationService<T> where T : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly IServiceProvider _serviceProvider;

        public NavigationService(NavigationStore navigationStore, IServiceProvider serviceProvider)
        {
            _navigationStore = navigationStore;
            _serviceProvider = serviceProvider;

        }

        public void Navigate()
        {
            _navigationStore.CurrentViewModel = _serviceProvider.GetRequiredService<T>();
        }
    }
}
