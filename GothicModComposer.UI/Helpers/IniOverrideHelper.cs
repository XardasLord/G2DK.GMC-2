using System.Collections.Generic;
using GothicModComposer.UI.Models;

namespace GothicModComposer.UI.Helpers
{
    public static class IniOverrideHelper
    {
        public static List<IniOverride> DefaultIniOverrideKeys
            => new()
            {
                new IniOverride { Key = "recalc", Value = "0", Section = "[PERFORMANCE]" },
                new IniOverride { Key = "sightValue", Value = "14", Section = "[PERFORMANCE]" },
                new IniOverride { Key = "modelDetail", Value = "1", Section = "[PERFORMANCE]" },
            };
    }
}