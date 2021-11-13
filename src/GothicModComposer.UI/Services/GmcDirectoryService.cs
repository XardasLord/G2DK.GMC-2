using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using GothicModComposer.UI.Interfaces;

namespace GothicModComposer.UI.Services
{
    public class GmcDirectoryService : IGmcDirectoryService
    {
        public void OpenModBuildDirectoryExecute(string modBuildDirectoryPath)
        {
            if (Directory.Exists(modBuildDirectoryPath))
            {
                Process.Start("explorer.exe", modBuildDirectoryPath);
            }
            else
            {
                MessageBox.Show("Mod build directory does not exists.");
            }
        }
        
        public void OpenLogsDirectoryExecute(string gmcLogsPath)
        {
            if (Directory.Exists(gmcLogsPath))
                Process.Start("explorer.exe", gmcLogsPath);
        }
        
        public void ClearLogsDirectoryExecute(string gmcLogsPath)
        {
            if (!Directory.Exists(gmcLogsPath))
                return;

            var directoryInfo = new DirectoryInfo(gmcLogsPath);

            try
            {
                foreach (var file in directoryInfo.GetFiles())
                    file.Delete();

                foreach (var dir in directoryInfo.GetDirectories())
                    dir.Delete(true);

                MessageBox.Show("Cleared logs directory.", "Clear logs", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (IOException ex)
            {
                MessageBox.Show($"Cannot clear Logs directory. Try to run GMC application again with Administrator privileges.{Environment.NewLine}Reason: {ex.Message}",
                    "Cannot clear Logs directory", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public bool HasFiles(string gmcLogsPath)
        {
            if (!Directory.Exists(gmcLogsPath))
                return false;
            
            var directoryInfo = new DirectoryInfo(gmcLogsPath);

            return directoryInfo.GetFiles().Any();
        }
    }
}