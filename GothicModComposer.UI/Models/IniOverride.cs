using System.Collections.Generic;
using System.Text.Json.Serialization;
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

        [JsonIgnore]
        public string Section { get; set; }
        
        [JsonIgnore]
        public string DisplayAs { get; set; }
        
        [JsonIgnore]
        public List<string> AvailableValues { get; set; }
    }
}