using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using GothicModComposer.UI.Interfaces;
using GothicModComposer.UI.Services;
using GothicModComposer.UI.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace GothicModComposer.UI
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly NotifyIcon _notifyIcon;
        private GmcVM _gmcVM;
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

            ConfigureTrayIcon();

            _mainWindow.StateChanged += MainWindowOnStateChanged;
        }

        private void ConfigureTrayIcon()
        {
            _notifyIcon.Icon = new Icon("Resources/GMC-logo.ico");
            _notifyIcon.Text = "GMC UI";
            _notifyIcon.Click += NotifyIconOnClick;
            _notifyIcon.ContextMenuStrip = new ContextMenuStrip();
            _gmcVM = _serviceProvider.GetRequiredService<GmcVM>();
            _notifyIcon.ContextMenuStrip.Items.Add("Run mod", null, (s, _) =>
            {
                if (!string.IsNullOrWhiteSpace(_gmcVM.GmcSettings.GmcConfiguration.DefaultWorld))
                {
                    _gmcVM.RunModProfile.Execute(null);   
                }
                else
                {
                    MessageBox.Show("No default world value chosen.");
                }
            });
            _notifyIcon.ContextMenuStrip.Items.Add("Update", null, (s, _) => _gmcVM.RunUpdateProfile.Execute(null));
            _notifyIcon.ContextMenuStrip.Items.Add("Compose", null, (s, _) => _gmcVM.RunComposeProfile.Execute(null));
            _notifyIcon.ContextMenuStrip.Items.Add("Build mod file", null,
                (s, _) => _gmcVM.RunBuildModFileProfile.Execute(null));
            _notifyIcon.ContextMenuStrip.Items.Add("Restore Gothic", null,
                (s, _) => _gmcVM.RunRestoreGothicProfile.Execute(null));
            _notifyIcon.ContextMenuStrip.Items.Add("Exit", null, (s, _) => Current.Shutdown());
            _notifyIcon.Visible = true;
        }

        private void MainWindowOnStateChanged(object sender, EventArgs e)
        {
            if (_mainWindow.WindowState == WindowState.Minimized)
                _mainWindow.Hide();
        }

        private void NotifyIconOnClick(object sender, EventArgs e)
        {
            var @event = e as MouseEventArgs;
            if (@event?.Button == MouseButtons.Left)
            {
                _mainWindow.Show();
                _mainWindow.WindowState = WindowState.Normal;
                _mainWindow.Activate();
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _notifyIcon.Dispose();
            base.OnExit(e);
        }
    }
}