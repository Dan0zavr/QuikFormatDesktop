using Microsoft.Extensions.DependencyInjection;
using QuikFormatDesktop.Models;
using QuikFormatDesktop.ViewModels.Enums;
using QuikFormatDesktop.ViewModels.Navigation;
using QuikFormatDesktop.ViewModels.ShortMenuViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace QuikFormatDesktop.ViewModels.Commands
{
    public class GoToDetailsCommand<TViewModel> : ICommand where TViewModel : ViewModelBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly NavigationStore _navigationStore;

        public GoToDetailsCommand(IServiceProvider serviceProvider, NavigationStore navigationStore)
        {
            _serviceProvider = serviceProvider;
            _navigationStore = navigationStore;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            if (parameter is not StyleObject targetStyle)
                return;

            var stylesVm = _serviceProvider.GetRequiredService<StylesViewModel>();

            switch (targetStyle.Type)
            {
                case Enums.StyleType.MarkedNumbering:
                case Enums.StyleType.NumberedNumbering:
                    stylesVm.SelectedTabIndex = (int)TabItemIndex.Numbering;
                    stylesVm.NumberingStyleViewModel.Load(targetStyle, true);
                    break;

                case Enums.StyleType.Paragraph:
                case Enums.StyleType.Text:
                    stylesVm.SelectedTabIndex = (int)TabItemIndex.Text;
                    stylesVm.TextStyleViewModel.Load(targetStyle, true);
                    break;

                case Enums.StyleType.Table:
                    stylesVm.SelectedTabIndex = (int)TabItemIndex.Table;
                    stylesVm.TableStyleViewModel.Load(targetStyle, true);
                    break;
                case Enums.StyleType.Picture:
                    stylesVm.SelectedTabIndex = (int)TabItemIndex.Picture;
                    stylesVm.PictureStyleViewModel.Load(targetStyle, true);
                    break;
                case Enums.StyleType.Formula:
                    stylesVm.SelectedTabIndex = (int)TabItemIndex.Formula;
                    stylesVm.FormulaStyleViewModel.Load(targetStyle, true);
                    break;
                case Enums.StyleType.Global:
                    stylesVm.SelectedTabIndex = (int)TabItemIndex.Global;
                    stylesVm.GlobalStyleViewModel.Load(targetStyle, true);
                    break;

            }

            _navigationStore.CurrentViewModel = stylesVm;
        }
    }
}
