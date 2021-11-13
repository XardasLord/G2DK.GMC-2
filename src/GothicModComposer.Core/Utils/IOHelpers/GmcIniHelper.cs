using System.Collections.Generic;
using System.Reflection;
using GothicModComposer.Core.Models.IniFiles;

namespace GothicModComposer.Core.Utils.IOHelpers
{
    public static class GmcIniHelper
    {
        public static List<IniBlock> GetDefaultGmcIni()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;

            var infoSection = new IniBlock("INFO");
            infoSection.Set("Title", "GMC");
            infoSection.Set("Version", $"{version?.Major}.{version?.Minor}.{version?.Build}");
            infoSection.Set("Authors", "");
            infoSection.Set("Webpage", "");
            infoSection.Set("Description", "!<symlink>GothicGame.rtf");
            infoSection.Set("Icon", "GothicStarter.exe");

            var filesSection = new IniBlock("FILES");
            filesSection.Set("VDF", "");
            filesSection.Set("Game", "Content\\Gothic");
            filesSection.Set("FightAI", "Content\\Fight");
            filesSection.Set("Menu", "System\\Menu");
            filesSection.Set("Camera", "System\\Camera");
            filesSection.Set("Music", "System\\Music");
            filesSection.Set("SoundEffects", "System\\SFX");
            filesSection.Set("ParticleEffects", "System\\ParticleFX");
            filesSection.Set("VisualEffects", "System\\VisualFX");
            filesSection.Set("OutputUnits", "OU");

            var settingsSection = new IniBlock("SETTINGS");
            settingsSection.Set("Player", "PC_HERO");
            settingsSection.Set("World", "");

            var optionsSettings = new IniBlock("OPTIONS");
            optionsSettings.Set("show_Info", "0");
            optionsSettings.Set("show_InfoX", "800");
            optionsSettings.Set("show_InfoY", "7200");
            optionsSettings.Set("show_Version", "1");
            optionsSettings.Set("show_VersionX", "6500");
            optionsSettings.Set("show_VersionY", "7200");
            optionsSettings.Set("show_Focus", "1");
            optionsSettings.Set("show_FocusItm", "1");
            optionsSettings.Set("show_FocusMob", "1");
            optionsSettings.Set("show_FocusNpc", "1");
            optionsSettings.Set("show_FocusBar", "1");
            optionsSettings.Set("force_Subtitles", "0");
            optionsSettings.Set("force_Parameters", "");

            var overridesSection = new IniBlock("OVERRIDES");
            overridesSection.Set("INTERNAL.extendedMenu", "1");

            return new List<IniBlock>
            {
                infoSection, filesSection, settingsSection, optionsSettings, overridesSection
            };
        }
    }
}