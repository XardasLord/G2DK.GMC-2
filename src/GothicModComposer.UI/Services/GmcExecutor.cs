using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using GothicModComposer.Core.Builders;
using GothicModComposer.Core.Presets;
using GothicModComposer.UI.Enums;
using GothicModComposer.UI.Interfaces;
using GothicModComposer.UI.ViewModels;

namespace GothicModComposer.UI.Services
{
    public class GmcExecutor : IGmcExecutor
    {
        private readonly GmcSettingsVM _gmcSettingsVM;
        private const string PathToGothic2Exe = "System/Gothic2.exe";
        private const string PathToGothicVdfsExe = "_Work/Tools/VDFS/GothicVDFS.exe";

        public GmcExecutor(GmcSettingsVM gmcSettingsVM)
        {
            _gmcSettingsVM = gmcSettingsVM;
        }
        
        public bool GothicExecutableExists(string gothicRootPath)
            => File.Exists(Path.Combine(gothicRootPath, PathToGothic2Exe));

        public bool GothicVdfsExecutableExists(string gothicRootPath)
            => File.Exists(Path.Combine(gothicRootPath, PathToGothicVdfsExe));

        public async Task ExecuteAsync(GmcExecutionProfile profile)
        {
            if (IsGmcAlreadyRun())
            {
                MessageBox.Show("GMC is already running.", "GMC is running", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (ProfileCanTouchWorldFiles())
            {
                _gmcSettingsVM.UnsubscribeOnWorldDirectoryChanges();
            }
            
            var gmcManager = GmcCoreManagerBuilder.PrepareGmcExecutor((ProfilePresetType)profile, _gmcSettingsVM.GmcConfiguration.ModificationRootPath,
                _gmcSettingsVM.GmcConfiguration.Gothic2RootPath, _gmcSettingsVM.GmcSettingsJsonFilePath);
            
            await gmcManager.RunAsync();

            if (ProfileCanTouchWorldFiles())
            {
                _gmcSettingsVM.SubscribeOnWorldDirectoryChanges();
                _gmcSettingsVM.LoadZen3DWorlds();
            }

            bool ProfileCanTouchWorldFiles() => profile is GmcExecutionProfile.Compose or GmcExecutionProfile.Update or GmcExecutionProfile.RestoreGothic;
        }

        private static bool IsGmcAlreadyRun() => Process.GetProcessesByName("GMC-2").Length > 0;
    }
}