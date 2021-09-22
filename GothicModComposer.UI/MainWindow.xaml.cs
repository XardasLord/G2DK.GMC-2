using System;
using System.Reflection;
using GothicModComposer.UI.Services;

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
    }
}