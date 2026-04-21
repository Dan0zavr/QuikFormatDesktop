using System;
using System.Collections.Generic;
using System.Text;

namespace QuikFormatDesktop.ViewModels.Navigation
{
    public class NavigationStore
    {
        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                if (_currentViewModel is IDisposable disposableOld)
                {
                    disposableOld.Dispose();
                }

                _currentViewModel = value;
                OnCurrentViewModelChanged();
            }
        }

        private ViewModelBase? _currentModalViewModel;
        public ViewModelBase? CurrentModalViewModel
        {
            get => _currentModalViewModel;
            set
            {
                _currentModalViewModel = value;
                CurrentModalViewModelChanged?.Invoke();
            }
        }

        public event Action? CurrentModalViewModelChanged;

        public void CloseModal()
        {
            CurrentModalViewModel = null;
        }

        public event Action CurrentViewModelChanged;

        private void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }
    }
}
