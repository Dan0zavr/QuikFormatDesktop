using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuikFormatDesktop.Database;
using QuikFormatDesktop.Models.SupportModels;
using QuikFormatDesktop.ViewModels;
using QuikFormatDesktop.ViewModels.Commands;
using QuikFormatDesktop.ViewModels.FormatViewModels;
using QuikFormatDesktop.ViewModels.Navigation;
using QuikFormatDesktop.ViewModels.Services;
using QuikFormatDesktop.ViewModels.ShortMenuViewModels;
using QuikFormatDesktop.ViewModels.StylesViewModels;
using QuikFormatDesktop.Views;
using System.Configuration;
using System.Data;
using System.Diagnostics;
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
                    services.Configure<FontSettings>(context.Configuration.GetSection("FontSettings"));
                    services.Configure<ParagraphSettings>(context.Configuration.GetSection("ParagraphSettings"));
                    services.Configure<TableSettings>(context.Configuration.GetSection("TableSettings"));
                    services.Configure<GeneralSettings>(context.Configuration.GetSection("GeneralSettings"));
                    services.Configure<SystemStyles>(context.Configuration.GetSection("SystemStyles"));

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
                    services.AddTransient<NumberedNumberingStyleViewModel>();
                    services.AddTransient<MarkedNumberingStyleViewModel>();
                    services.AddTransient<FormulaStyleViewModel>();

                    services.AddTransient<TemplateViewModel>();
                    services.AddTransient<SystemTemplateViewModel>();

                    services.AddTransient<SelectorCardViewModel>();
                    services.AddTransient<PreviewViewModel>();

                    services.AddTransient<TextShortMenuViewModel>();
                    services.AddTransient<ParagraphShortMenuViewModel>();
                    services.AddTransient<TableShortMenuViewModel>();
                    services.AddTransient<PictureShortMenuViewModel>();
                    services.AddTransient<FormulaShortMenuViewModel>();
                    services.AddTransient<MarkedNumberingShortMenuViewModel>();
                    services.AddTransient<NumberedNumberingShortMenuViewModel>();
                    services.AddTransient<TemplateShortMenuViewModel>();
                    services.AddTransient<SystemTemplateShortMenuViewModel>();

                    services.AddTransient<DeleteWarningViewModel>();

                    //сервисы стилей
                    services.AddSingleton<TextService>();
                    services.AddSingleton<ParagraphService>();
                    services.AddSingleton<TableService>();
                    services.AddSingleton<PictureService>();
                    services.AddSingleton<NumberingService>();
                    services.AddSingleton<FormulaService>();

                    services.AddSingleton<TemplateService>();

                    //вспомогательные сервисы
                    services.AddTransient<AlignmentService>();
                    services.AddTransient<FontService>();
                    services.AddTransient<MarkerService>();
                    services.AddTransient<MarkerTypeService>();
                    services.AddTransient<PositionService>();
                    services.AddTransient<TemplateMapper>();

                    services.AddTransient<IDialogService, DialogService>();

                    services.AddSingleton<NavigationService<FormatViewModel>>();
                    services.AddSingleton<NavigationService<StylesViewModel>>();

                    services.AddScoped<ModalNavigationService<TemplateViewModel>>();
                    services.AddScoped<ModalNavigationService<SystemTemplateViewModel>>();
                    services.AddScoped<ModalNavigationService<DeleteWarningViewModel>>();
                });
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();

            using (var scope = _host.Services.CreateScope())
            {
                var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<QfDbContext>>();
                await using var context = await factory.CreateDbContextAsync();
                //await context.Database.MigrateAsync();
                context.Database.EnsureCreated();
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
            var navigationStore = _host.Services.GetRequiredService<NavigationStore>();
            if (navigationStore.CurrentViewModel is FormatViewModel formatVm)
            {
                PreviewViewModel previewViewModel = formatVm.PreviewViewModel;
                await previewViewModel.DisposeAsync();
            }

            await _host.StopAsync();
            _host.Dispose();
            base.OnExit(e);
        }
    }

}
