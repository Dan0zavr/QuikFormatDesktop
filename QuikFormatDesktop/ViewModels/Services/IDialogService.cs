using System;
using System.Collections.Generic;
using System.Text;

namespace QuikFormatDesktop.ViewModels.Services
{
    public interface IDialogService
    {
        void ShowError(string message, string title = "Ошибка");
    }
}
