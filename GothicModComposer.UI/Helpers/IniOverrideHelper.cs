using System.Collections.Generic;
using GothicModComposer.UI.Models;

namespace GothicModComposer.UI.Helpers
{
    public static class IniOverrideHelper
    {
        public static List<IniOverride> DefaultIniOverrideKeys
            => new()
            {
                new IniOverride { Key = "recalc", Value = "0", Section = "PERFORMANCE", DisplayAs = DataGridColumnType.ComboBox, AvailableValues = new List<string> { "0", "1" } },
                new IniOverride { Key = "sightValue", Value = "14", Section = "PERFORMANCE", DisplayAs = DataGridColumnType.TextBox },
                new IniOverride { Key = "modelDetail", Value = "1", Section = "PERFORMANCE", DisplayAs = DataGridColumnType.TextBox },
                
                new IniOverride { Key = "animatedWindows", Value = "1", Section = "GAME", DisplayAs = DataGridColumnType.ComboBox, AvailableValues = new List<string> { "0", "1" } },
                new IniOverride { Key = "camLookaroundInverse", Value = "1", Section = "GAME", DisplayAs = DataGridColumnType.TextBox },
            };
    }
}