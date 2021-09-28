using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using GothicModComposer.UI.Interfaces;
using GothicModComposer.UI.Services;
using GothicModComposer.UI.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Application = System.Windows.Application;

namespace GothicModComposer.UI
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly NotifyIcon _notifyIcon;
        private MainWindow _mainWindow;
        
        public App()
        {
            _notifyIcon = new NotifyIcon();
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
            _mainWindow = _serviceProvider.GetService<MainWindow>();

            if (_mainWindow is null)
                return;

            _mainWindow.DataContext = _serviceProvider.GetService<GmcVM>();
            _mainWindow.Show();
            
            _notifyIcon.Icon = new Icon("Resources/GMC-logo.ico");
            _notifyIcon.Text = "GMC UI";
            _notifyIcon.Click += NotifyIconOnClick;
            _notifyIcon.ContextMenuStrip = new ContextMenuStrip();
            _notifyIcon.ContextMenuStrip.Items.Add("Exit", null, (s, e) => Application.Current.Shutdown());
            _notifyIcon.Visible = true;
            
            _mainWindow.StateChanged += MainWindowOnStateChanged;
        }

        private void MainWindowOnStateChanged(object? sender, EventArgs e)
        {
            if (_mainWindow.WindowState == WindowState.Minimized)
                _mainWindow.Hide();
        }

        private void NotifyIconOnClick(object? sender, EventArgs e)
        {
            _mainWindow.Show();
            _mainWindow.WindowState = WindowState.Normal;
            _mainWindow.Activate();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _notifyIcon.Dispose();
            base.OnExit(e);
        }
    }
}