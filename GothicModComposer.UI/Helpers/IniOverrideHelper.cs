using System.Collections.Generic;
using GothicModComposer.UI.Models;

namespace GothicModComposer.UI.Helpers
{
    public static class IniOverrideHelper
    {
        private const string TextBoxColumnName = "TextBox";
        private const string CheckBoxColumnName = "CheckBox";
        private const string ComboBoxColumnName = "ComboBox";
        
        public static List<IniOverride> DefaultIniOverrideKeys
            => new()
            {
                new IniOverride { Key = "recalc", Value = "0", Section = "PERFORMANCE", DisplayAs = ComboBoxColumnName, AvailableValues = new List<string> { "0", "1" } },
                new IniOverride { Key = "sightValue", Value = "14", Section = "PERFORMANCE", DisplayAs = TextBoxColumnName },
                new IniOverride { Key = "modelDetail", Value = "1", Section = "PERFORMANCE", DisplayAs = TextBoxColumnName },
            };
    }
}