using System.Windows;
using GothicModComposer.UI.Commands;
using GothicModComposer.UI.Enums;
using GothicModComposer.UI.Interfaces;
using GothicModComposer.UI.Services;

namespace GothicModComposer.UI.ViewModels
{
    public class GmcVM : Observable
    {
        public RelayCommand RunUpdateProfile { get; }
        public RelayCommand RunComposeProfile { get; }
        public RelayCommand RunModProfile { get; }
        public RelayCommand RunRestoreGothicProfile { get; }
        public RelayCommand RunBuildModFileProfile { get; }
        public RelayCommand RunEnableVDFProfile { get; }

        private readonly IGmcExecutor _gmcExecutor;

        public GmcVM()
        {
            _gmcExecutor = new GmcExecutor();

            RunUpdateProfile = new RelayCommand(RunUpdateProfileExecute);
            RunComposeProfile = new RelayCommand(RunComposeProfileExecute);
            RunModProfile = new RelayCommand(RunModProfileExecute);
            RunRestoreGothicProfile = new RelayCommand(RunRestoreGothicProfileExecute);
            RunBuildModFileProfile = new RelayCommand(RunBuildModFileProfileProfileExecute);
            RunEnableVDFProfile = new RelayCommand(RunEnableVDFProfileProfileExecute);
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
    }
}
