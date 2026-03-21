using QuikFormatDesktop.Models;
using QuikFormatDesktop.ViewModels.StylesViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using QuikFormatDesktop.Exceptions;

namespace QuikFormatDesktop.ViewModels.Commands.TextViewModelCommands.TextStyleCommands
{
    public class AddTextStyleCommand : ICommand
    {
        private readonly FontStyleViewModel _viewModel;

        public AddTextStyleCommand(FontStyleViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            if (!string.IsNullOrWhiteSpace(_viewModel.StyleName))
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
                int fontId = await _viewModel.fontService.GetIdByName(_viewModel.SelectedFont.FontName);
                TextStyle textStyle = new TextStyle
                {
                    Name = _viewModel.StyleName,
                    Font = fontId,
                    FontSize = _viewModel.SelectedFontSize
                };

                if (await _viewModel.textStyleService.IsUnique(textStyle.Name))
                {
                    await _viewModel.textStyleService.Add(textStyle);
                    _viewModel.PStatusMessage = "Стиль успешно добавлен";
                }
                else
                {
                    _viewModel.PStatusMessage = "Стиль с таким именем уже существует";
                }
            }
            catch (FontNotFoundException fex)
            {
                _viewModel.dialogService.ShowError(fex.Message);
            }
            catch (Exception ex)
            {
                _viewModel.dialogService.ShowError($"Произошла неизвестная ошибка. Код: {ex.HResult}");
            }
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
