using QuikFormatDesktop.View;
using QuikFormatDesktop.ViewModels;
using QuikFormatDesktop.ViewModels.Commands;
using QuikFormatDesktop.ViewModels.Navigation;
using System.Configuration;
using System.Data;
using System.Windows;

namespace QuikFormatDesktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly NavigationStore _navigationStore;

        public App()
        {
            _navigationStore = new NavigationStore();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var navigateToFormat = new NavigationService(
                _navigationStore,
                () => new FormatViewModel());

            var navigateToStyles = new NavigationService(
                _navigationStore,
                () => new StylesViewModel());

            var navigationViewModel = new NavigationViewModel(
                navigateToFormat,
                navigateToStyles
            );

            var mainViewModel = new MainViewModel(_navigationStore, navigationViewModel);

            navigateToFormat.Navigate();

            MainWindow = new MainWindow()
            {
                DataContext = mainViewModel
            };
            MainWindow.Show();
            base.OnStartup(e);
        }
    }

}
