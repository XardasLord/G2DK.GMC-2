using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using GothicModComposer.UI.Models;
using GothicModComposer.UI.ViewModels;
using Microsoft.Win32;

namespace GothicModComposer.UI.Views
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class GmcSettings
    {
		private const string WindowsStartupRegistryPath = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";

        public GmcSettings(GmcSettingsVM gmcSettingsVM)
        {
            InitializeComponent();

            DataContext = gmcSettingsVM;

            var collectionView = new ListCollectionView(gmcSettingsVM.GmcConfiguration.IniOverrides);
            collectionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(IniOverride.Section)));

            OverridesIniTable.ItemsSource = collectionView;

            var logsDirectoryExists = Directory.Exists(gmcSettingsVM.LogsDirectoryPath);
            var directoryContainsLogs = logsDirectoryExists && new DirectoryInfo(gmcSettingsVM.LogsDirectoryPath).GetFiles().Any();
            openLogsBtn.IsEnabled = directoryContainsLogs;
            clearLogsBtn.IsEnabled = directoryContainsLogs;
            modBuildBtn.IsEnabled = Directory.Exists(Path.Combine(gmcSettingsVM.GmcConfiguration?.Gothic2RootPath ?? string.Empty, ".gmc", "build"));
        }

        private void WindowsStartup_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
	        var exePath = System.Reflection.Assembly.GetExecutingAssembly().Location.Replace("dll", "exe");

            try
            {
                var registryKey = Registry.CurrentUser.OpenSubKey(WindowsStartupRegistryPath, true);
                registryKey?.SetValue("GMC_2_UI", exePath);
            }
            catch (Exception)
            {
                MessageBox.Show("Error during adding application to Windows startup.");
            }
        }

        private void WindowsStartup_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                var registryKey = Registry.CurrentUser.OpenSubKey(WindowsStartupRegistryPath, true);
                registryKey?.DeleteValue("GMC_2_UI", false);
            }
            catch (Exception)
            {
                MessageBox.Show("Error during removing application from Windows startup");
            }
        }

        private void gothicRoot_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Clipboard.SetText(gothicRoot.Text);
        }

        private void gothicModRoot_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Clipboard.SetText(gothicModRoot.Text); 
        }

    }
}