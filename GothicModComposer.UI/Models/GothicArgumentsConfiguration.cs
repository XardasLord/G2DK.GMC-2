using GothicModComposer.UI.Helpers;

namespace GothicModComposer.UI.Models
{
    public class GothicArgumentsConfiguration : ObservableVM
    {
        private bool _isDevMode;
        private bool _isMusicDisabled;
        private bool _isReparseScripts;
        private bool _isConvertTextures;
        private bool _isConvertData;
        private bool _isConvertAll;
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
            set
            {
                if (value && IsConvertAll)
                    return;
                
                SetProperty(ref _isReparseScripts, value);
            }
        }

        public bool IsConvertTextures
        {
            get => _isConvertTextures;
            set
            {
                if (value && IsConvertAll)
                    return;
                
                SetProperty(ref _isConvertTextures, value);
            }
        }

        public bool IsConvertData
        {
            get => _isConvertData;
            set
            {
                if (value && IsConvertAll)
                    return;
                
                SetProperty(ref _isConvertData, value);
            }
        }

        public bool IsConvertAll
        {
            get => _isConvertAll;
            set
            {
                SetProperty(ref _isConvertAll, value);

                IsReparseScript = false;
                IsConvertTextures = false;
                IsConvertData = false;
            }
        }

        public Resolution Resolution
        {
            get => _resolution;
            set => SetProperty(ref _resolution, value);
        }
    }
}