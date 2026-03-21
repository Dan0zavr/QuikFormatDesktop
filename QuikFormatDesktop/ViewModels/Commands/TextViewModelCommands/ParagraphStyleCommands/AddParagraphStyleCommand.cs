using QuikFormatDesktop.Exceptions;
using QuikFormatDesktop.Models;
using QuikFormatDesktop.ViewModels.StylesViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace QuikFormatDesktop.ViewModels.Commands.TextViewModelCommands.ParagraphStyleCommands
{
    public class AddParagraphStyleCommand : ICommand
    {
        private readonly ParagraphStyleViewModel _viewModel;

        public AddParagraphStyleCommand(ParagraphStyleViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            if (!string.IsNullOrWhiteSpace(_viewModel.ParagraphStyleName) && 
                _viewModel.FirstLineIndent != null &&
                _viewModel.LeftIndent != null &&
                _viewModel.RightIndent != null &&
                _viewModel.AfterInterval != null &&
                _viewModel.BeforeInterval != null)
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
                int alignmentId = await _viewModel.alignmentService.GetIdByType(_viewModel.SelectedAlignment);

                ParagraphStyle paragraphStyle = new ParagraphStyle
                {
                    Name = _viewModel.ParagraphStyleName,
                    Alignment = alignmentId,
                    LeftIndent = _viewModel.LeftIndent,
                    RightIndent = _viewModel.RightIndent,
                    FirstLineIndent = _viewModel.FirstLineIndent,
                    IntervalInText = _viewModel.SelectedInterval,
                    BeforeInterval = _viewModel.BeforeInterval,
                    AfterInterval = _viewModel.AfterInterval,
                    ContextualSpacing = _viewModel.ContextualSpacing,
                };

                if (await _viewModel.paragraphService.IsUnique(paragraphStyle.Name))
                {
                    await _viewModel.paragraphService.Add(paragraphStyle);
                    _viewModel.PStatusMessage = "Стиль Успешно добавлен";
                }
                else
                {
                    _viewModel.PStatusMessage = "Стиль с таким именем уже существует";
                }
            }
            catch (AlignmentNotFoundException aex)
            {
                _viewModel.dialogService.ShowError(aex.Message);
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
