using System.Collections.Generic;
using GothicModComposer.UI.Models;

namespace GothicModComposer.UI.Helpers
{
    public static class IniOverrideHelper
    {
        public static List<IniOverride> DefaultIniOverrideKeys
            => new()
            {
                new IniOverride { Key = "recalc", Value = "0" },
                new IniOverride { Key = "sightValue", Value = "14" },
                new IniOverride { Key = "modelDetail", Value = "1" },
            };
    }
}