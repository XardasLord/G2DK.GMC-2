using System.Diagnostics;
using System.IO;
using System.Windows;
using GothicModComposer.UI.Enums;
using GothicModComposer.UI.Interfaces;
using GothicModComposer.UI.ViewModels;

namespace GothicModComposer.UI.Services
{
    public class GmcExecutor : IGmcExecutor
    {
        public void Execute(GmcExecutionProfile profile)
        {
            if (IsGmcAlreadyRun())
            {
                MessageBox.Show("GMC is already running. Close the existing GMC-2.exe process if you want to execute a new one.", "GMC is running", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
#if DEBUG
            var gmcLocation = @"C:\localRepository\Gothic 2 Dzieje Khorinis\GMC-2\GothicModComposer\bin\Debug\net5.0-windows";
#else
            var gmcLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "Release");
#endif

            var settingsVM = new GmcSettingsVM();

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = Path.Combine(gmcLocation, "GMC-2.exe"),
                    ArgumentList =
                    {
                        $"--gothic2Path={settingsVM.GmcConfiguration.Gothic2RootPath}",
                        $"--modPath={settingsVM.GmcConfiguration.ModificationRootPath}",
                        $"--profile={profile}",
                        $"--configurationFile={settingsVM.GmcSettingsJsonFilePath}"

                    },
                    UseShellExecute = false
                }
            };

            process.Start();
        }

        private static bool IsGmcAlreadyRun() => Process.GetProcessesByName("GMC-2").Length > 0;
    }
}