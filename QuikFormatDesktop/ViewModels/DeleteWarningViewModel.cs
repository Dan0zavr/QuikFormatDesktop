using QuikFormatDesktop.Models;
using QuikFormatDesktop.ViewModels.Commands;
using QuikFormatDesktop.ViewModels.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;


namespace QuikFormatDesktop.ViewModels
{
    public class DeleteWarningViewModel : ViewModelBase, ILoadable
    {
        private NavigationStore _navigationStore;
        private ICommand _originalDeleteCommand;
        private ICommand _wrappedDeleteCommand;

        public DeleteWarningViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            CloseWindow = new RelayCommand<object?>(Close);
        }
        public ICommand CloseWindow { get; }
        public ICommand DeleteCommand
        {
            get => _wrappedDeleteCommand;
            set
            {
                _originalDeleteCommand = value;
                _wrappedDeleteCommand = WrapCommand(value);
                OnPropertyChanged(nameof(DeleteCommand));
            }
        }

        private ICommand WrapCommand(ICommand original)
        {
            if (original is IAsyncRelayCommand asyncCmd)
            {
                return new AsyncRelayCommand<object?>(
                    async (param) =>
                    {
                        if (asyncCmd.CanExecute(param))
                        {
                            await asyncCmd.ExecuteAsync(param);
                            Close(param);
                        }
                    },
                    (param) => asyncCmd.CanExecute(param)
                );
            }
            else
            {
                return new RelayCommand<object?>(
                    (param) =>
                    {
                        if (original.CanExecute(param))
                        {
                            original.Execute(param);
                            Close(param);
                        }
                    },
                    (param) => original.CanExecute(param)
                );
            }
        }

        private StyleObject _style;

        public bool IsTemplate => _style is Template ? true : false;
        public string StyleName => _style.Name;
        public string Message => IsTemplate ? "Вы уверены, что хотите удалить шаблон:" : "Вы уверены, что хотите удалить стиль:";

        public void Load(object parametr, bool isEdit = false)
        {
            _style = (StyleObject)parametr;
        }

        private void Close(object? parameter)
        {
            if(_navigationStore.CurrentModalViewModel is DeleteWarningViewModel)
            {
                _navigationStore.CurrentModalViewModel = null;
            }
        }
    }
}
