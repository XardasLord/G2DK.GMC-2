using System;
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
        }

        private void WindowsStartup_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
	        var exePath = System.Reflection.Assembly.GetExecutingAssembly().Location.Replace("dll", "exe");

            try
            {
                var registryKey = Registry.CurrentUser.OpenSubKey(WindowsStartupRegistryPath, true);
                registryKey?.SetValue("GMC_2_UI", exePath);
            }
            catch (Exception ex)
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
            catch (Exception ex)
            {
                MessageBox.Show("Error during removing application from Windows startup");
            }
        }
    }
}