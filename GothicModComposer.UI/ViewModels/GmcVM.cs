using System.Diagnostics;
using System.IO;
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
        private readonly MainWindow _mainWindow;

        public GmcVM(IGmcExecutor gmcExecutor, ISpacerService spacerService, GmcSettingsVM gmcSettingsVM, MainWindow mainWindow)
        {
            _gmcExecutor = gmcExecutor;
            _spacerService = spacerService;
            _mainWindow = mainWindow;

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
            DeleteZenWorld = new RelayCommand(DeleteZenWorldExecute);
            RenameZenWorld = new RelayCommand(RenameZenWorldExecute);
            ManualZenWorldListRefresh = new RelayCommand(ManualZenWorldListRefreshExecute);
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
        public RelayCommand DeleteZenWorld { get; }
        public RelayCommand RenameZenWorld { get; }
        public RelayCommand ManualZenWorldListRefresh { get; }

        private void RunUpdateProfileExecute(object obj)
        {
            if (!_gmcExecutor.GothicExecutableExists(GmcSettings.GmcConfiguration.Gothic2RootPath))
            {
                ShowGothicExeNotFoundMessage();
                return;
            }

            _mainWindow.Hide();
            
            _gmcExecutor.Execute(GmcExecutionProfile.Update);

            _mainWindow.Show();
            _mainWindow.Activate();
        }

        private void RunComposeProfileExecute(object obj)
        {
            if (!_gmcExecutor.GothicExecutableExists(GmcSettings.GmcConfiguration.Gothic2RootPath))
            {
                ShowGothicExeNotFoundMessage();
                return;
            }

            var messageBoxResult = MessageBox.Show("Are you sure you want to execute 'Compose' profile?",
                "Execute Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                _mainWindow.Hide();
            
                _gmcExecutor.Execute(GmcExecutionProfile.Compose);

                _mainWindow.Show();
                _mainWindow.Activate();
            }
        }

        private void RunModProfileExecute(object obj)
        {
            if (!_gmcExecutor.GothicExecutableExists(GmcSettings.GmcConfiguration.Gothic2RootPath))
            {
                ShowGothicExeNotFoundMessage();
                return;
            }

            _mainWindow.Hide();
            
            _gmcExecutor.Execute(GmcExecutionProfile.RunMod);

            _mainWindow.Show();
            _mainWindow.Activate();
        }

        private void RunRestoreGothicProfileExecute(object obj)
        {
            var messageBoxResult = MessageBox.Show("Are you sure to you want to execute 'RestoreGothic' profile?",
                "Execute Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                _mainWindow.Hide();
            
                _gmcExecutor.Execute(GmcExecutionProfile.RestoreGothic);

                _mainWindow.Show();
                _mainWindow.Activate();
            }
        }

        private void RunBuildModFileProfileProfileExecute(object obj)
        {
            if (!_gmcExecutor.GothicVdfsExecutableExists(GmcSettings.GmcConfiguration.Gothic2RootPath))
            {
                ShowGothicVdfsExeNotFoundMessage();
                return;
            }

            _mainWindow.Hide();
            
            _gmcExecutor.Execute(GmcExecutionProfile.BuildModFile);

            _mainWindow.Show();
            _mainWindow.Activate();
        }

        private void RunEnableVDFProfileProfileExecute(object obj)
            => _gmcExecutor.Execute(GmcExecutionProfile.EnableVDF);

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

            _gmcExecutor.Execute(GmcExecutionProfile.EnableVDF);

            var spacerProcess = _spacerService.RunSpacer(GmcSettings.GmcConfiguration.Gothic2RootPath);

            spacerProcess.WaitForExit();

            _gmcExecutor.Execute(GmcExecutionProfile.DisableVDF);
        }

        private void DeleteZenWorldExecute(object obj)
        {
            var fullWorldPath = obj?.ToString();

            if (fullWorldPath is null)
                return;

            if (File.Exists(fullWorldPath))
                File.Delete(fullWorldPath);
        }

        private void RenameZenWorldExecute(object obj)
        {
            var fullWorldPath = obj?.ToString();

            if (fullWorldPath is null)
                return;

            var fileName = Path.GetFileNameWithoutExtension(fullWorldPath);

            var inputDialog = new InputDialog("Rename", "Rename:", fileName);
            if (inputDialog.ShowDialog() == true)
            {
                var fileDirectoryPath = Path.GetDirectoryName(fullWorldPath);
                var newFileName = $"{inputDialog.Answer}{Path.GetExtension(fullWorldPath)}";
                var newFileNamePath = Path.Combine(fileDirectoryPath, newFileName);

                File.Move(fullWorldPath, newFileNamePath);
            }
        }

        private void ManualZenWorldListRefreshExecute(object obj)
        {
            GmcSettings.LoadZen3DWorlds(true);
        }

        private static void ShowGothicExeNotFoundMessage() =>
            MessageBox.Show("Gothic2.exe does not exist in 'System' directory.", "Gothic2.exe does not exist",
                MessageBoxButton.OK, MessageBoxImage.Warning);

        private static void ShowGothicVdfsExeNotFoundMessage() =>
            MessageBox.Show("GothicVDFS.exe does not exist in '_Work/Tools/VDFS' directory.",
                "GothicVDFS.exe does not exist",
                MessageBoxButton.OK, MessageBoxImage.Warning);
    }
}