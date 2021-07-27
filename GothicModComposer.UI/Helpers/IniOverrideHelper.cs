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
                new IniOverride { Key = "camLookaroundInverse", Value = "0", Section = "GAME", DisplayAs = DataGridColumnType.TextBox },
                new IniOverride { Key = "bloodDetail", Value = "2", Section = "GAME", DisplayAs = DataGridColumnType.ComboBox, AvailableValues = new List<string> { "0", "1", "2", "3" } },
                new IniOverride { Key = "extendedVideoKeys", Value = "0", Section = "GAME", DisplayAs = DataGridColumnType.TextBox },
                new IniOverride { Key = "scaleVideos", Value = "0", Section = "GAME", DisplayAs = DataGridColumnType.ComboBox, AvailableValues = new List<string> { "0", "1" } },
                new IniOverride { Key = "cameraLightRange", Value = "0", Section = "GAME", DisplayAs = DataGridColumnType.TextBox },
                new IniOverride { Key = "invShowArrows", Value = "1", Section = "GAME", DisplayAs = DataGridColumnType.TextBox },
                new IniOverride { Key = "invSwitchToFirstCategory", Value = "0", Section = "GAME", DisplayAs = DataGridColumnType.TextBox },
                new IniOverride { Key = "enableMouse", Value = "1", Section = "GAME", DisplayAs = DataGridColumnType.ComboBox, AvailableValues = new List<string> { "0", "1" } },
                new IniOverride { Key = "mouseSensitivity", Value = "0.5", Section = "GAME", DisplayAs = DataGridColumnType.TextBox },
                new IniOverride { Key = "playLogoVideos", Value = "1", Section = "GAME", DisplayAs = DataGridColumnType.ComboBox, AvailableValues = new List<string> { "0", "1" } },
                new IniOverride { Key = "skyEffects", Value = "1", Section = "GAME", DisplayAs = DataGridColumnType.ComboBox, AvailableValues = new List<string> { "0", "1" } },
                new IniOverride { Key = "highlightMeleeFocus", Value = "2", Section = "GAME", DisplayAs = DataGridColumnType.TextBox },
            };
    }
}