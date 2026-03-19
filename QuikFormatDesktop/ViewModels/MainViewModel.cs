using QuikFormatDesktop.ViewModels.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuikFormatDesktop.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private NavigationStore _navigationStore;

        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

        public NavigationViewModel NavigationViewModel { get; }

        public MainViewModel(NavigationStore navigationStore, NavigationViewModel navigationViewModel)
        {
            _navigationStore = navigationStore;
            NavigationViewModel = navigationViewModel;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;

        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
