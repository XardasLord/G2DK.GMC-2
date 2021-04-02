using GothicModComposer.UI.ViewModels.Commands;

namespace GothicModComposer.UI.ViewModels.ViewModels
{
    public class GmcVM : Observable
    {
        public RelayCommand RunUpdateProfile { get; }
        public RelayCommand RunComposeProfile { get; }
        public RelayCommand RunDKProfile { get; }
        public RelayCommand RunRestoreGothicProfile { get; }
        public RelayCommand RunBuildModFileProfile { get; }
        public RelayCommand RunEnableVDFProfile { get; }

        public GmcVM()
        {
            RunUpdateProfile = new RelayCommand(RunUpdateProfileExecute, x => true);
            RunComposeProfile = new RelayCommand(RunComposeProfileExecute, x => true);
            RunDKProfile = new RelayCommand(RunDKProfileExecute, x => true);
            RunRestoreGothicProfile = new RelayCommand(RunRestoreGothicProfileExecute, x => true);
            RunBuildModFileProfile = new RelayCommand(RunBuildModFileProfileProfileExecute, x => true);
            RunEnableVDFProfile = new RelayCommand(RunEnableVDFProfileProfileExecute, x => true);
        }


        private void RunUpdateProfileExecute(object obj)
        {
        }

        private void RunComposeProfileExecute(object obj)
        {
        }

        private void RunDKProfileExecute(object obj)
        {
        }

        private void RunRestoreGothicProfileExecute(object obj)
        {
        }

        private void RunBuildModFileProfileProfileExecute(object obj)
        {
        }

        private void RunEnableVDFProfileProfileExecute(object obj)
        {
        }
    }
}
