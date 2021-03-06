using System.Diagnostics;
using System.Windows;
using GothicModComposer.UI.Commands;
using GothicModComposer.UI.Enums;
using GothicModComposer.UI.Helpers;
using GothicModComposer.UI.Interfaces;
using GothicModComposer.UI.Services;
using GothicModComposer.UI.Views;


namespace GothicModComposer.UI.ViewModels
{
    public class GmcVM : ObservableVM
    {
        public GmcSettingsVM GmcSettings { get; private set; }
        public RelayCommand RunUpdateProfile { get; }
        public RelayCommand RunComposeProfile { get; }
        public RelayCommand RunModProfile { get; }
        public RelayCommand RunRestoreGothicProfile { get; }
        public RelayCommand RunBuildModFileProfile { get; }
        public RelayCommand RunEnableVDFProfile { get; }
        public RelayCommand OpenSettings { get; }
        public RelayCommand OpenChangeLog { get; }
        public RelayCommand OpenTrelloProjectBoard { get; }

        private readonly IGmcExecutor _gmcExecutor;

        public GmcVM()
        {
            _gmcExecutor = new GmcExecutor();

            GmcSettings = new GmcSettingsVM();
            GmcSettings.PropertyChanged += (_, _) => GmcSettings.SaveSettings.Execute(null);
            GmcSettings.GmcConfiguration.GothicArguments.PropertyChanged += (_, _) => GmcSettings.SaveSettings.Execute(null);

            RunUpdateProfile = new RelayCommand(RunUpdateProfileExecute);
            RunComposeProfile = new RelayCommand(RunComposeProfileExecute);
            RunModProfile = new RelayCommand(RunModProfileExecute);
            RunRestoreGothicProfile = new RelayCommand(RunRestoreGothicProfileExecute);
            RunBuildModFileProfile = new RelayCommand(RunBuildModFileProfileProfileExecute);
            RunEnableVDFProfile = new RelayCommand(RunEnableVDFProfileProfileExecute);
            OpenSettings = new RelayCommand(OpenSettingsExecute);
            OpenChangeLog = new RelayCommand(OpenChangeLogExecute);
            OpenTrelloProjectBoard = new RelayCommand(OpenTrelloProjectBoardExecute);
        }

        private void RunUpdateProfileExecute(object obj)
            => _gmcExecutor.Execute(GmcExecutionProfile.Update);

        private void RunComposeProfileExecute(object obj)
        {
            var messageBoxResult = MessageBox.Show("Are you sure you want to execute 'Compose' profile?", "Execute Confirmation", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                _gmcExecutor.Execute(GmcExecutionProfile.Compose);
            }
        }

        private void RunModProfileExecute(object obj)
            => _gmcExecutor.Execute(GmcExecutionProfile.RunMod);

        private void RunRestoreGothicProfileExecute(object obj)
        {
            var messageBoxResult = MessageBox.Show("Are you sure to you want to execute 'RestoreGothic' profile?", "Execute Confirmation", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                _gmcExecutor.Execute(GmcExecutionProfile.RestoreGothic);
            }
        }

        private void RunBuildModFileProfileProfileExecute(object obj)
            => _gmcExecutor.Execute(GmcExecutionProfile.BuildModFile);

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
    }
}
