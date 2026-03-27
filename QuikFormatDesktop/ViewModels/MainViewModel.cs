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
        public ViewModelBase? CurrentModalViewModel => _navigationStore.CurrentModalViewModel;

        public bool IsModalOpen => _navigationStore.CurrentModalViewModel != null;

        public NavigationViewModel NavigationViewModel { get; }

        public MainViewModel(NavigationStore navigationStore, NavigationViewModel navigationViewModel)
        {
            _navigationStore = navigationStore;
            NavigationViewModel = navigationViewModel;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
            _navigationStore.CurrentModalViewModelChanged += OnCurrentModalViewModelChanged;
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        private void OnCurrentModalViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentModalViewModel));
            OnPropertyChanged(nameof(IsModalOpen));
        }
    }
}
