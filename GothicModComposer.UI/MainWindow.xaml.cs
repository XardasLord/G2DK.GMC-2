using System;
using System.Windows;
using System.Reflection;
using GothicModComposer.UI.Services;
using GothicModComposer.UI.Helpers;
using System.Diagnostics;
using System.IO;

namespace GothicModComposer.UI
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        SubmodsHelper submodsHelper = new SubmodsHelper();
        public MainWindow()
        {
            InitializeComponent();

            SetTitle();

            submodsHelper.Main();

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

        public void SubmodsInitiation()
        {
            string version = "Wersja: ";
            SubmodInfo1.Text = submodsHelper.submods[0].Title;
            SubmodInfo2.Text = version + submodsHelper.submods[0].Version;
            SubmodInfo3.Text = "";
            for (int i = 0; i < submodsHelper.submods[0].Authors.Length; i++)
            {
                if(i < submodsHelper.submods[0].Authors.Length-1 && submodsHelper.submods[0].Authors.Length>1)
                    SubmodInfo3.Text += submodsHelper.submods[0].Authors[i]+", ";
                else 
                    SubmodInfo3.Text += submodsHelper.submods[0].Authors[i];
            }
            
            SubmodInfo4.Text = submodsHelper.submods[0].Webpage;
            SubmodInfo5.Text += submodsHelper.submods[0].Description;
        }

        private void TabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (Worlds.IsSelected)
                SubmodsInfoPanel.Visibility = Visibility.Hidden;
            if (Submods.IsSelected)
            {
                SubmodsInitiation();
                SubmodsInfoPanel.Visibility = Visibility.Visible;
            }
        }

        private void SubmodsView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (SubmodsView.SelectedItems.Count > 0)
            {
                
                //MessageBox.Show(SubmodsView.SelectedIndex.ToString());
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string iniFileName = submodsHelper.submods[SubmodsView.SelectedIndex].iniFileName;
            //MessageBox.Show(SubmodsView.SelectedIndex.ToString());
            MessageBox.Show(submodsHelper.submods[SubmodsView.SelectedIndex].iniFileName);
            Process notePad = new Process();

            notePad.StartInfo.FileName = Path.Combine(submodsHelper.path,"Gothic2.exe");
            notePad.StartInfo.Arguments = "-game:"+iniFileName;

            notePad.Start();
        }
    }
}