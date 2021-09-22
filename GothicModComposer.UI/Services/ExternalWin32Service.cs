using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace GothicModComposer.UI.Services
{
    public class ExternalWin32Service
    {
        private const int SW_SHOW = 5;

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public bool IsApplicationAlreadyOpened()
        {
            var thisProcessName = Process.GetCurrentProcess().ProcessName;

            return Process.GetProcesses().Count(p => p.ProcessName == thisProcessName) > 1;
        }

        public void MaximizeAlreadyOpenedApplication()
        {
            var openedApplicationProcess =
                Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).FirstOrDefault();

            if (openedApplicationProcess != null)
                ShowWindow(openedApplicationProcess.MainWindowHandle, SW_SHOW);
        }
    }
}