using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuikFormatDesktop.Database;
using QuikFormatDesktop.View;
using QuikFormatDesktop.ViewModels;
using QuikFormatDesktop.ViewModels.Commands;
using QuikFormatDesktop.ViewModels.Navigation;
using QuikFormatDesktop.ViewModels.Services;
using QuikFormatDesktop.ViewModels.StylesViewModels;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;

namespace QuikFormatDesktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = CreateHostBuilder().Build();
        }

        private static IHostBuilder CreateHostBuilder(string[] args = null)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddDbContextFactory<QfDbContext>(options => options.UseSqlite(context.Configuration.GetConnectionString("DefaultConnection")));

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
                });
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();

            using (var scope = _host.Services.CreateScope())
            {
                var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<QfDbContext>>();
                await using var context = await factory.CreateDbContextAsync();
                await context.Database.EnsureCreatedAsync();
            }

            var mainViewModel = _host.Services.GetRequiredService<MainViewModel>();
            var nav = _host.Services.GetRequiredService<NavigationService<FormatViewModel>>();

            nav.Navigate();

            MainWindow = new MainWindow()
            {
                DataContext = mainViewModel
            };
            MainWindow.Show();
            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();
            base.OnExit(e);
        }
    }

}
