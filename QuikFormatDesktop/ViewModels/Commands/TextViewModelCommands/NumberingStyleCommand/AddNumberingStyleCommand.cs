using QuikFormatDesktop.Models;
using QuikFormatDesktop.ViewModels.Services;
using QuikFormatDesktop.ViewModels.StylesViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace QuikFormatDesktop.ViewModels.Commands.TextViewModelCommands.NumberingStyleCommand
{
    public class AddNumberingStyleCommand : ICommand
    {
        private readonly INumbering _viewModel;
        private readonly NumberingService _numberingService;
        private readonly IDialogService _dialogService;

        public AddNumberingStyleCommand(INumbering viewModel, NumberingService numberingService, IDialogService dialogService)
        {
            _viewModel = viewModel;
            _numberingService = numberingService;
            _dialogService = dialogService;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            if (!string.IsNullOrWhiteSpace(_viewModel.NumberingStyleName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async void Execute(object? parameter)
        {
            try
            {
                NumberingStyle numberingStyle = new NumberingStyle
                {
                    Name = _viewModel.NumberingStyleName,
                    Marker = _viewModel.SelectedMarker.Id
                };

                if (await _numberingService.IsUnique(numberingStyle.Name))
                {
                    await _numberingService.Add(numberingStyle);
                    _viewModel.PStatusMessage = "Стиль успешно добавлен";
                }
                else
                {
                    _viewModel.PStatusMessage = "Стиль с таким именем уже существует";
                }
            }
            catch (Exception ex)
            {
                _dialogService.ShowError($"Произошла неизвестная ошибка. Код: {ex.HResult}");
            }
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
