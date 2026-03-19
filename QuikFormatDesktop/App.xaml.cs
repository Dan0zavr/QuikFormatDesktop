using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QuikFormatDesktop.Database;
using QuikFormatDesktop.View;
using QuikFormatDesktop.ViewModels;
using QuikFormatDesktop.ViewModels.Commands;
using QuikFormatDesktop.ViewModels.Navigation;
using QuikFormatDesktop.ViewModels.Services;
using QuikFormatDesktop.ViewModels.StylesViewModels;
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
        private ServiceProvider _serviceProvider;

        public App()
        {
            _navigationStore = new NavigationStore();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection();

            services.AddDbContext<QfDbContext>(options => options.UseSqlite(@"Data Source=C:\Users\Temp\source\repos\QuikFormatDesktop\QuikFormatDesktop\TemplatesDataBase.db"));

            services.AddSingleton<NavigationStore>();

            services.AddSingleton<MainViewModel>();
            services.AddSingleton<NavigationViewModel>();

            services.AddTransient<FormatViewModel>();
            services.AddTransient<StylesViewModel>();

            services.AddTransient<TextStyleViewModel>();
            services.AddTransient<FontStyleViewModel>();
            services.AddTransient<ParagraphStyleViewModel>();
            services.AddTransient<TableStyleViewModel>();
            services.AddTransient<PictureStyleViewModel>();
            services.AddTransient<NumberingStyleViewModel>();
            services.AddTransient<FormulaStyleViewModel>();

            //сервисы стилей
            services.AddTransient<TextService>();
            services.AddTransient<ParagraphService>();
            services.AddTransient<TableService>();
            services.AddTransient<PictureService>();
            services.AddTransient<NumberingService>();
            services.AddTransient<FormulaService>();

            services.AddTransient<TemplateService>();

            //вспомогательные сервисы
            services.AddTransient<AlignmentService>();
            services.AddTransient<FontService>();
            services.AddTransient<MarkerService>();
            services.AddTransient<MarkerTypeService>();
            services.AddTransient<PositionService>();

            services.AddSingleton<NavigationService<FormatViewModel>>();
            services.AddSingleton<NavigationService<StylesViewModel>>();

            _serviceProvider = services.BuildServiceProvider();

            var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
            var nav = _serviceProvider.GetRequiredService<NavigationService<FormatViewModel>>();

            nav.Navigate();

            MainWindow = new MainWindow()
            {
                DataContext = mainViewModel
            };
            MainWindow.Show();
            base.OnStartup(e);
        }
    }

}
