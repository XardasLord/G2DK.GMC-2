using System.Windows;
using GothicModComposer.UI.Interfaces;
using GothicModComposer.UI.Services;
using GothicModComposer.UI.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace GothicModComposer.UI
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            var services = new ServiceCollection();

            ConfigureServices(services);

            _serviceProvider = services.BuildServiceProvider();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IGmcExecutor, GmcExecutor>();
            services.AddTransient<ISpacerService, SpacerService>();
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<IGmcDirectoryService, GmcDirectoryService>();
            services.AddTransient<IZenWorldsFileWatcherService, ZenWorldsFileWatcherService>();

            services.AddSingleton<GmcVM>();
            services.AddSingleton<GmcSettingsVM>();
            
            services.AddSingleton<MainWindow>();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetService<MainWindow>();

            if (mainWindow is null)
                return;

            mainWindow.DataContext = _serviceProvider.GetService<GmcVM>();

            mainWindow.Show();
        }
    }
}