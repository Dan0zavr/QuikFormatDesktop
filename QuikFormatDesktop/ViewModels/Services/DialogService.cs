using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace QuikFormatDesktop.ViewModels.Services
{
    public class DialogService : IDialogService
    {
        public void ShowError(string message, string title = "Ошибка")
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
