using GothicModComposer.UI.Helpers;

namespace GothicModComposer.UI.Models
{
    public class GothicArgumentsConfiguration : ObservableVM
    {
        private bool _isDevMode;
        private bool _isMusicDisabled;
        private bool _isReparseScripts;
        private bool _isSoundDisabled;
        private bool _isWindowMode;
        private Resolution _resolution;

        public bool IsWindowMode
        {
            get => _isWindowMode;
            set => SetProperty(ref _isWindowMode, value);
        }

        public bool IsDevMode
        {
            get => _isDevMode;
            set => SetProperty(ref _isDevMode, value);
        }

        public bool IsMusicDisabled
        {
            get => _isMusicDisabled;
            set => SetProperty(ref _isMusicDisabled, value);
        }

        public bool IsSoundDisabled
        {
            get => _isSoundDisabled;
            set => SetProperty(ref _isSoundDisabled, value);
        }

        public bool IsReparseScript
        {
            get => _isReparseScripts;
            set => SetProperty(ref _isReparseScripts, value);
        }

        public Resolution Resolution
        {
            get => _resolution;
            set => SetProperty(ref _resolution, value);
        }
    }
}