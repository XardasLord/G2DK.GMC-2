using System.Diagnostics;
using System.Windows;
using GothicModComposer.UI.Commands;
using GothicModComposer.UI.Enums;
using GothicModComposer.UI.Helpers;
using GothicModComposer.UI.Interfaces;
using GothicModComposer.UI.Views;

namespace GothicModComposer.UI.ViewModels
{
    public class GmcVM : ObservableVM
    {
        private readonly IGmcExecutor _gmcExecutor;
        private readonly ISpacerService _spacerService;

        public GmcVM(IGmcExecutor gmcExecutor, ISpacerService spacerService, GmcSettingsVM gmcSettingsVM)
        {
            _gmcExecutor = gmcExecutor;
            _spacerService = spacerService;

            GmcSettings = gmcSettingsVM;
            GmcSettings.PropertyChanged += (_, _) => GmcSettings.SaveSettings.Execute(null);
            GmcSettings.GmcConfiguration.GothicArguments.PropertyChanged +=
                (_, _) => GmcSettings.SaveSettings.Execute(null);

            RunUpdateProfile = new RelayCommand(RunUpdateProfileExecute);
            RunComposeProfile = new RelayCommand(RunComposeProfileExecute);
            RunModProfile = new RelayCommand(RunModProfileExecute);
            RunRestoreGothicProfile = new RelayCommand(RunRestoreGothicProfileExecute);
            RunBuildModFileProfile = new RelayCommand(RunBuildModFileProfileProfileExecute);
            RunEnableVDFProfile = new RelayCommand(RunEnableVDFProfileProfileExecute);
            OpenSettings = new RelayCommand(OpenSettingsExecute);
            OpenChangeLog = new RelayCommand(OpenChangeLogExecute);
            OpenTrelloProjectBoard = new RelayCommand(OpenTrelloProjectBoardExecute);
            RunSpacer = new RelayCommand(RunSpacerExecute);
        }

        public GmcSettingsVM GmcSettings { get; }
        public RelayCommand RunUpdateProfile { get; }
        public RelayCommand RunComposeProfile { get; }
        public RelayCommand RunModProfile { get; }
        public RelayCommand RunRestoreGothicProfile { get; }
        public RelayCommand RunBuildModFileProfile { get; }
        public RelayCommand RunEnableVDFProfile { get; }
        public RelayCommand OpenSettings { get; }
        public RelayCommand OpenChangeLog { get; }
        public RelayCommand OpenTrelloProjectBoard { get; }
        public RelayCommand RunSpacer { get; }

        private void RunUpdateProfileExecute(object obj)
        {
            if (!_gmcExecutor.GothicExecutableExists(GmcSettings.GmcConfiguration.Gothic2RootPath))
            {
                ShowGothicExeNotFoundMessage();
                return;
            }
            
            _gmcExecutor.Execute(GmcExecutionProfile.Update, GmcSettings);
        }

        private void RunComposeProfileExecute(object obj)
        {
            if (!_gmcExecutor.GothicExecutableExists(GmcSettings.GmcConfiguration.Gothic2RootPath))
            {
                ShowGothicExeNotFoundMessage();
                return;
            }
            
            var messageBoxResult = MessageBox.Show("Are you sure you want to execute 'Compose' profile?",
                "Execute Confirmation", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
                _gmcExecutor.Execute(GmcExecutionProfile.Compose, GmcSettings);
        }

        private void RunModProfileExecute(object obj)
        {
            if (!_gmcExecutor.GothicExecutableExists(GmcSettings.GmcConfiguration.Gothic2RootPath))
            {
                ShowGothicExeNotFoundMessage();
                return;
            }
            
            _gmcExecutor.Execute(GmcExecutionProfile.RunMod, GmcSettings);
        }

        private void RunRestoreGothicProfileExecute(object obj)
        {
            var messageBoxResult = MessageBox.Show("Are you sure to you want to execute 'RestoreGothic' profile?",
                "Execute Confirmation", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
                _gmcExecutor.Execute(GmcExecutionProfile.RestoreGothic, GmcSettings);
        }

        private void RunBuildModFileProfileProfileExecute(object obj)
        {
            if (!_gmcExecutor.GothicVdfsExecutableExists(GmcSettings.GmcConfiguration.Gothic2RootPath))
            {
                ShowGothicVdfsExeNotFoundMessage();
                return;
            }
            
            _gmcExecutor.Execute(GmcExecutionProfile.BuildModFile, GmcSettings);
        }

        private void RunEnableVDFProfileProfileExecute(object obj)
            => _gmcExecutor.Execute(GmcExecutionProfile.EnableVDF, GmcSettings);

        private void OpenSettingsExecute(object obj)
            => new GmcSettings(GmcSettings).ShowDialog();

        private static void OpenChangeLogExecute(object obj)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "https://gitlab.com/dzieje-khorinis/gmc-2/-/blob/master/CHANGELOG.md",
                UseShellExecute = true
            };

            Process.Start(processStartInfo);
        }

        private static void OpenTrelloProjectBoardExecute(object obj)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "https://trello.com/b/ndyTLtzA/gmc-2",
                UseShellExecute = true
            };

            Process.Start(processStartInfo);
        }

        private void RunSpacerExecute(object obj)
        {
            if (!_spacerService.SpacerExists(GmcSettings.GmcConfiguration.Gothic2RootPath))
            {
                MessageBox.Show("Spacer.exe editor does not exist in 'System' directory.", "Spacer does not exist",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _gmcExecutor.Execute(GmcExecutionProfile.EnableVDF, GmcSettings);
            
            var spacerProcess = _spacerService.RunSpacer(GmcSettings.GmcConfiguration.Gothic2RootPath);

            spacerProcess.WaitForExit();
            
            _gmcExecutor.Execute(GmcExecutionProfile.DisableVDF, GmcSettings);
        }

        private static void ShowGothicExeNotFoundMessage() =>
            MessageBox.Show("Gothic2.exe does not exist in 'System' directory.", "Gothic2.exe does not exist",
                MessageBoxButton.OK, MessageBoxImage.Warning);

        private static void ShowGothicVdfsExeNotFoundMessage() =>
            MessageBox.Show("GothicVDFS.exe does not exist in '_Work/Tools/VDFS' directory.", "GothicVDFS.exe does not exist",
                MessageBoxButton.OK, MessageBoxImage.Warning);
    }
}