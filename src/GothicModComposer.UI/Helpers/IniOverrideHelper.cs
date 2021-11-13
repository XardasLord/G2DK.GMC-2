using System.Collections.Generic;
using GothicModComposer.UI.Models;

namespace GothicModComposer.UI.Helpers
{
    public static class IniOverrideHelper
    {
        public static List<IniOverride> DefaultIniOverrideKeys
            => new()
            {
                new IniOverride
                {
                    Key = "recalc", Value = "0", Section = "PERFORMANCE", DisplayAs = DataGridColumnType.ComboBox,
                    AvailableValues = new List<string> {"0", "1"}
                },
                new IniOverride
                    {Key = "sightValue", Value = "14", Section = "PERFORMANCE", DisplayAs = DataGridColumnType.TextBox},
                new IniOverride
                    {Key = "modelDetail", Value = "1", Section = "PERFORMANCE", DisplayAs = DataGridColumnType.TextBox},

                new IniOverride
                {
                    Key = "animatedWindows", Value = "1", Section = "GAME", DisplayAs = DataGridColumnType.ComboBox,
                    AvailableValues = new List<string> {"0", "1"}
                },
                new IniOverride
                {
                    Key = "camLookaroundInverse", Value = "0", Section = "GAME", DisplayAs = DataGridColumnType.TextBox
                },
                new IniOverride
                {
                    Key = "bloodDetail", Value = "2", Section = "GAME", DisplayAs = DataGridColumnType.ComboBox,
                    AvailableValues = new List<string> {"0", "1", "2", "3"}
                },
                new IniOverride
                    {Key = "extendedVideoKeys", Value = "0", Section = "GAME", DisplayAs = DataGridColumnType.TextBox},
                new IniOverride
                {
                    Key = "scaleVideos", Value = "0", Section = "GAME", DisplayAs = DataGridColumnType.ComboBox,
                    AvailableValues = new List<string> {"0", "1"}
                },
                new IniOverride
                    {Key = "cameraLightRange", Value = "0", Section = "GAME", DisplayAs = DataGridColumnType.TextBox},
                new IniOverride
                    {Key = "invShowArrows", Value = "1", Section = "GAME", DisplayAs = DataGridColumnType.TextBox},
                new IniOverride
                {
                    Key = "invSwitchToFirstCategory", Value = "0", Section = "GAME",
                    DisplayAs = DataGridColumnType.TextBox
                },
                new IniOverride
                {
                    Key = "enableMouse", Value = "1", Section = "GAME", DisplayAs = DataGridColumnType.ComboBox,
                    AvailableValues = new List<string> {"0", "1"}
                },
                new IniOverride
                    {Key = "mouseSensitivity", Value = "0.5", Section = "GAME", DisplayAs = DataGridColumnType.TextBox},
                new IniOverride
                {
                    Key = "playLogoVideos", Value = "1", Section = "GAME", DisplayAs = DataGridColumnType.ComboBox,
                    AvailableValues = new List<string> {"0", "1"}
                },
                new IniOverride
                {
                    Key = "skyEffects", Value = "1", Section = "GAME", DisplayAs = DataGridColumnType.ComboBox,
                    AvailableValues = new List<string> {"0", "1"}
                },
                new IniOverride
                {
                    Key = "highlightMeleeFocus", Value = "2", Section = "GAME", DisplayAs = DataGridColumnType.TextBox
                },
                new IniOverride
                {
                    Key = "highlightInteractFocus", Value = "0", Section = "GAME",
                    DisplayAs = DataGridColumnType.TextBox
                },
                new IniOverride
                {
                    Key = "useGothic1Controls", Value = "1", Section = "GAME", DisplayAs = DataGridColumnType.ComboBox,
                    AvailableValues = new List<string> {"0", "1"}
                },
                new IniOverride
                {
                    Key = "disallowVideoInput", Value = "0", Section = "GAME", DisplayAs = DataGridColumnType.ComboBox,
                    AvailableValues = new List<string> {"0", "1"}
                },
                new IniOverride
                    {Key = "keyDelayRate", Value = "150", Section = "GAME", DisplayAs = DataGridColumnType.TextBox},
                new IniOverride
                {
                    Key = "enableJoystick", Value = "0", Section = "GAME", DisplayAs = DataGridColumnType.ComboBox,
                    AvailableValues = new List<string> {"0", "1"}
                },
                new IniOverride
                {
                    Key = "keyDelayFirst", Value = "0", Section = "GAME", DisplayAs = DataGridColumnType.ComboBox,
                    AvailableValues = new List<string> {"0", "1"}
                },
                new IniOverride
                {
                    Key = "subTitles", Value = "1", Section = "GAME", DisplayAs = DataGridColumnType.ComboBox,
                    AvailableValues = new List<string> {"0", "1"}
                },
                new IniOverride
                {
                    Key = "subTitlesAmbient", Value = "1", Section = "GAME", DisplayAs = DataGridColumnType.ComboBox,
                    AvailableValues = new List<string> {"0", "1"}
                },
                new IniOverride
                {
                    Key = "subTitlesPlayer", Value = "1", Section = "GAME", DisplayAs = DataGridColumnType.ComboBox,
                    AvailableValues = new List<string> {"0", "1"}
                },
                new IniOverride
                {
                    Key = "subTitlesNoise", Value = "0", Section = "GAME", DisplayAs = DataGridColumnType.ComboBox,
                    AvailableValues = new List<string> {"0", "1"}
                },
                new IniOverride
                    {Key = "invMaxColumns", Value = "5", Section = "GAME", DisplayAs = DataGridColumnType.TextBox},
                new IniOverride
                    {Key = "invMaxRows", Value = "0", Section = "GAME", DisplayAs = DataGridColumnType.TextBox},
                new IniOverride
                {
                    Key = "invSplitScreen", Value = "1", Section = "GAME", DisplayAs = DataGridColumnType.ComboBox,
                    AvailableValues = new List<string> {"0", "1"}
                },
                new IniOverride
                {
                    Key = "invCatOrder", Value = "COMBAT,POTION,FOOD,ARMOR,MAGIC,RUNE,DOCS,OTHER,NONE",
                    Section = "GAME", DisplayAs = DataGridColumnType.TextBox
                },
                new IniOverride
                {
                    Key = "gametextAutoScroll", Value = "1000", Section = "GAME", DisplayAs = DataGridColumnType.TextBox
                },
                new IniOverride
                {
                    Key = "usePotionKeys", Value = "0", Section = "GAME", DisplayAs = DataGridColumnType.ComboBox,
                    AvailableValues = new List<string> {"0", "1"}
                },
                new IniOverride
                {
                    Key = "useQuickSaveKeys", Value = "0", Section = "GAME", DisplayAs = DataGridColumnType.ComboBox,
                    AvailableValues = new List<string> {"0", "1"}
                },
                new IniOverride
                {
                    Key = "useSpeechReverbLevel", Value = "1", Section = "GAME",
                    DisplayAs = DataGridColumnType.ComboBox, AvailableValues = new List<string> {"0", "1", "2"}
                },
                new IniOverride
                {
                    Key = "keyboardLayout", Value = "00020409", Section = "GAME",
                    DisplayAs = DataGridColumnType.ComboBox, AvailableValues = new List<string> {"00020409", "00000407"}
                },

                new IniOverride
                    {Key = "zVidDevice", Value = "0", Section = "VIDEO", DisplayAs = DataGridColumnType.TextBox},
                new IniOverride
                {
                    Key = "zVidResFullscreenBPP", Value = "32", Section = "VIDEO",
                    DisplayAs = DataGridColumnType.ComboBox, AvailableValues = new List<string> {"16", "32"}
                },
                new IniOverride
                {
                    Key = "zVidBrightness", Value = "0.5", Section = "VIDEO", DisplayAs = DataGridColumnType.TextBox
                }, // TODO: values from 0.0 to 1.0
                new IniOverride
                {
                    Key = "zVidContrast", Value = "0.5", Section = "VIDEO", DisplayAs = DataGridColumnType.TextBox
                }, // TODO: values from 0.0 to 1.0
                new IniOverride
                {
                    Key = "zVidGamma", Value = "0.5", Section = "VIDEO", DisplayAs = DataGridColumnType.TextBox
                }, // TODO: values from 0.0 to 1.0
                new IniOverride
                {
                    Key = "zTexMaxSize", Value = "16384", Section = "VIDEO", DisplayAs = DataGridColumnType.TextBox
                }, // TODO: values from 0 to 16384

                new IniOverride
                {
                    Key = "musicVolume", Value = "0.800000012", Section = "SOUND",
                    DisplayAs = DataGridColumnType.TextBox
                }, // TODO: values from 0.0 to 1.0
                new IniOverride
                {
                    Key = "soundUseReverb", Value = "1", Section = "SOUND", DisplayAs = DataGridColumnType.ComboBox,
                    AvailableValues = new List<string> {"0", "1"}
                },
                new IniOverride
                {
                    Key = "extendedProviders", Value = "0", Section = "SOUND", DisplayAs = DataGridColumnType.ComboBox,
                    AvailableValues = new List<string> {"0", "1"}
                }
            };
    }
}