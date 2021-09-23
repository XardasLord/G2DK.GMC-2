using System;
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
        public void Execute(GmcExecutionProfile profile, GmcSettingsVM gmcSettingsVM)
        {
            if (IsGmcAlreadyRun())
            {
                MessageBox.Show(
                    "GMC is already running. Close the existing GMC-2.exe process if you want to execute a new one.",
                    "GMC is running", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "GMC-2.exe"),
                    ArgumentList =
                    {
                        $"--gothic2Path={gmcSettingsVM.GmcConfiguration.Gothic2RootPath}",
                        $"--modPath={gmcSettingsVM.GmcConfiguration.ModificationRootPath}",
                        $"--profile={profile}",
                        $"--configurationFile={gmcSettingsVM.GmcSettingsJsonFilePath}",
                        gmcSettingsVM.GmcConfiguration.CloseAfterFinish ? "" : "--keepOpenAfterFinish"
                    },
                    Verb = "runas", // Force to run the process as Administrator
                    UseShellExecute = false
                }
            };

            gmcSettingsVM.UnsubscribeOnWorldDirectoryChanges();

            process.Start();
            process.WaitForExit();

            gmcSettingsVM.SubscribeOnWorldDirectoryChanges();
        }

        private static bool IsGmcAlreadyRun() => Process.GetProcessesByName("GMC-2").Length > 0;
    }
}