using System;
using System.ComponentModel;
using System.Windows;

namespace GothicModComposer.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private System.Windows.Forms.NotifyIcon _notifyIcon;
        private bool _isExit;
 
        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow = new MainWindow();
            MainWindow.Closing += MainWindow_Closing;
 
            _notifyIcon = new System.Windows.Forms.NotifyIcon();
            _notifyIcon.DoubleClick += ShowMainMenu;
            _notifyIcon.Icon = GothicModComposer.UI.Properties.Resources.GMC_logo;
            _notifyIcon.Text = "GMC";
            _notifyIcon.Visible = true;
 
            CreateContextMenu();
            
            base.OnStartup(e);
        }
        
        private void CreateContextMenu()
        {
            _notifyIcon.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            _notifyIcon.ContextMenuStrip.Items.Add("Open GMC UI").Click += ShowMainMenu;
            _notifyIcon.ContextMenuStrip.Items.Add("Quit").Click += ExitApplication;
        }

        private void ShowMainMenu(object sender, EventArgs e)
        {
            if (MainWindow.IsVisible)
            {
                if (MainWindow.WindowState == WindowState.Minimized)
                {
                    MainWindow.WindowState = WindowState.Normal;
                }
                MainWindow.Activate();
            }
            else
            {
                MainWindow.Show();
            }
        }

        private void ExitApplication(object sender, EventArgs e)
        {
            _isExit = true;
            MainWindow?.Close();
            _notifyIcon.Dispose();
            _notifyIcon = null;
        }
 
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (!_isExit)
            {
                e.Cancel = true;
                MainWindow?.Hide(); // A hidden window can be shown again, a closed one not
            }
        }
    }
}
