using QuikFormatDesktop.Models;
using QuikFormatDesktop.ViewModels.Enums;
using QuikFormatDesktop.ViewModels.Services;
using QuikFormatDesktop.ViewModels.StylesViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;

namespace QuikFormatDesktop.ViewModels.Commands.NumberingStyleCommand
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
                    Id = _viewModel.StyleId,
                    Name = _viewModel.NumberingStyleName,
                    Marker = _viewModel.SelectedMarker.Id
                };

                if (await _numberingService.IsUnique(numberingStyle.Name))
                {
                    await _numberingService.Add(numberingStyle);
                    await ShowPopup("Стиль успешно обновлен", PopupType.Good);
                }
                else
                {
                    await ShowPopup("Стиль с таким именем уже существует", PopupType.Bad);
                }
            }
            catch (Exception ex)
            {
                _dialogService.ShowError($"Произошла неизвестная ошибка. Код: {ex.HResult}");
            }
            finally
            {
                _viewModel.RaiseRequestReset();
            }
        }

        private async Task ShowPopup(string message, PopupType type)
        {
            switch (type)
            {
                case PopupType.Bad:
                    _viewModel.PopupBackground = (Color)ColorConverter.ConvertFromString("#fc9d9d");
                    _viewModel.PopupForeground = (Color)ColorConverter.ConvertFromString("#570000");
                    break;
                case PopupType.Good:
                    _viewModel.PopupBackground = (Color)ColorConverter.ConvertFromString("#b1ffa8");
                    _viewModel.PopupForeground = (Color)ColorConverter.ConvertFromString("#085200");
                    break;
                default:
                    _viewModel.PopupBackground = Colors.White;
                    _viewModel.PopupForeground = Colors.Black;
                    break;
            }

            _viewModel.PopupMessage = message;
            _viewModel.IsPopupOpen = true;
            await Task.Delay(2000);
            _viewModel.IsPopupOpen = false;
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
