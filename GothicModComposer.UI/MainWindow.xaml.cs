using System;
using System.Windows;
using System.Reflection;
using GothicModComposer.UI.Services;
using GothicModComposer.UI.Helpers;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace GothicModComposer.UI
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        SubmodsLoaderService submodsLoaderService = new SubmodsLoaderService();
        public MainWindow()
        {
            InitializeComponent();

            SetTitle();

            submodsLoaderService.FindSubmodDatafiles();

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

        public void SubmodsInfoInitiation()
        {
            StringBuilder sb = new StringBuilder(100);

            string version = "Wersja: ";
            SubmodInfo1.Text = submodsLoaderService.submods[1].Title;
            SubmodInfo2.Text = sb.Append(version).Append(submodsLoaderService.submods[1].Version).ToString();
            sb.Clear();
            SubmodInfo3.Text = "";
            for (int i = 0; i < submodsLoaderService.submods[1].Authors.Length; i++)
            {
                if (i < submodsLoaderService.submods[1].Authors.Length - 1 && submodsLoaderService.submods[1].Authors.Length > 1)
                {
                    SubmodInfo3.Text += sb.Append(submodsLoaderService.submods[1].Authors[i]).Append(", ");
                    sb.Clear();
                }
                else
                    SubmodInfo3.Text += submodsLoaderService.submods[1].Authors[i];
            }
            
            SubmodInfo4.Text = submodsLoaderService.submods[1].Webpage;
            SubmodInfo5.Text = submodsLoaderService.submods[1].Description;
        }

        private void tcWorldsAndSubmods_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (Worlds.IsSelected)
                SubmodsInfoPanel.Visibility = Visibility.Hidden;
            if (Submods.IsSelected)
            {
                SubmodsInfoInitiation();
                SubmodsInfoPanel.Visibility = Visibility.Visible;
            }
        }
        private void RunSubmod_Click(object sender, RoutedEventArgs e)
        {
            string iniFileName = submodsLoaderService.submods[SubmodsView.SelectedIndex].iniFileName;
            Process gothic = new Process();

            gothic.StartInfo.FileName = Path.Combine(submodsLoaderService.path,"Gothic2.exe");
            gothic.StartInfo.Arguments = "-game:"+iniFileName;

            gothic.Start();
        }
    }
}