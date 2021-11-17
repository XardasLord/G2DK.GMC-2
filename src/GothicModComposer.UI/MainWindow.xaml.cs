using System;
using System.Reflection;
using System.Windows;
using GothicModComposer.Core;
using GothicModComposer.UI.Services;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.RichTextBox.Themes;

namespace GothicModComposer.UI
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            //TODO add gmc manager suppport
            var gmcManager = default(GmcCoreManager);

            InitializeComponent();
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.RichTextBox(
                    logsContainer,
                    theme: RichTextBoxConsoleTheme.Grayscale,
                    restrictedToMinimumLevel: LogEventLevel.Information
                )
                .WriteTo.File(
                    $"{gmcManager.GmcFolder.LogsFolderPath}/log_{DateTime.Now:yyyyMMdd_HHmm}.txt",
                    flushToDiskInterval: TimeSpan.FromSeconds(5),
                    shared: true,
                    restrictedToMinimumLevel: LogEventLevel.Debug
                )
                .CreateLogger();
            SetTitle();

            var win32Service = new ExternalWin32Service();

            if (win32Service.IsApplicationAlreadyOpened())
            {
                win32Service.MaximizeAlreadyOpenedApplication();
                Environment.Exit(0);
            }
        }

        private void SetTitle()
        {
            var fullVersion = Assembly.GetExecutingAssembly().GetName().Version;
            Title = $"GMC UI v{fullVersion?.Major}.{fullVersion?.Minor}.{fullVersion?.Build}";
        }

        private void IsConvertAllCheckbox_OnChecked(object sender, RoutedEventArgs e)
        {
            IsConvertTexturesCheckbox.IsEnabled = false;
            IsConvertDataCheckbox.IsEnabled = false;
            IsReparseScriptCheckbox.IsEnabled = false;
        }

        private void IsConvertAllCheckbox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            IsConvertTexturesCheckbox.IsEnabled = true;
            IsConvertDataCheckbox.IsEnabled = true;
            IsReparseScriptCheckbox.IsEnabled = true;
        }
    }
}