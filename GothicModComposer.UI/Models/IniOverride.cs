using GothicModComposer.UI.Helpers;

namespace GothicModComposer.UI.Models
{
    public class IniOverride : ObservableVM
    {
        private string _key;
        private string _value;

        public string Key
        {
            get => _key;
            set => SetProperty(ref _key, value);
        }

        public string Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }
    }
}