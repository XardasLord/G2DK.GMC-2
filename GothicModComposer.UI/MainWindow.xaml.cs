using System;
using System.Reflection;
using GothicModComposer.UI.Services;
using GothicModComposer.UI.Helpers;

namespace GothicModComposer.UI
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

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

        private void Test_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SubmodsHelper submodsHelper = new SubmodsHelper();
            submodsHelper.Main();
            
            TestText.Content+= submodsHelper.submods[0].Title
            + "\n" + submodsHelper.submods[0].Version 
            + "\n" + submodsHelper.submods[0].Authors[0]
            + "\n" + submodsHelper.submods[0].Webpage
            + "\n" + submodsHelper.submods[0].Description;
        }
    }
}