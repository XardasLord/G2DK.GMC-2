using System.Collections.Generic;
using GothicModComposer.Core.Models.Profiles;

namespace GothicModComposer.Core.Models.Interfaces
{
    public interface IGothicArguments
    {
        bool Contains(string parameter);
        void SetArg(string argument, string value = null);
        bool RemoveArg(string argument);
        List<string> ToList();
        GothicArguments Merge(IGothicArgumentsConfiguration profileGothicArgumentsForceConfig);
        string ToString();

        /// <summary>MISSING SUMMARY</summary>
        GothicArguments AddArgument_ZConvertAll();

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        GothicArguments AddArgument_Player(string value = null);

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        GothicArguments AddArgument_ZNoFastExit(string value = null);

        /// <summary>Game skips all the menus and starts the New Game on startup. In game `ESC` button closes the game.</summary>
        GothicArguments AddArgument_NoMenu();

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        GothicArguments AddArgument_Parse(string value = null);

        /// <summary>What world should be loaded after selecting new game. Value eg. "NewWorld.zen"</summary>
        GothicArguments AddArgument_3D(string value = "none");

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        GothicArguments AddArgument_Ini(string value = null);

        /// <summary>Starts the game in "MARVIN" mode.</summary>
        GothicArguments AddArgument_DevMode();

        /// <summary>Sets the active modification. Value eg. "MyMod.ini"</summary>
        GothicArguments AddArgument_Game(string value = "gothic.ini");

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        GothicArguments AddArgument_Vdfs(string value = "physicalfirst");

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        GothicArguments AddArgument_PhysicalFirst(string value = null);

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        GothicArguments AddArgument_CheckRoutines(string value = null);

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        GothicArguments AddArgument_ReparseVis(string value = null);

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        GothicArguments AddArgument_ConvertDialogCams(string value = null);

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        GothicArguments AddArgument_ZSector(string value = null);

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        GothicArguments AddArgument_ZSkipSector(string value = null);

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        GothicArguments AddArgument_ZMemProfiler(string value = null);

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        GothicArguments AddArgument_ZNoResThread(string value = null);

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        GothicArguments AddArgument_ZTexConvert(string value = null);

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        GothicArguments AddArgument_ZLoad3DSoldMethod(string value = null);

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        GothicArguments AddArgument_ZenSaveOldMethod(string value = null);

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        GothicArguments AddArgument_ZNoSharedFeatures(string value = null);

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        GothicArguments AddArgument_ZNoPFX(string value = null);

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        GothicArguments AddArgument_ZTexMaxSize(string value = null);

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        GothicArguments AddArgument_MergeVobsWithLevel(string value = null);

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        GothicArguments AddArgument_ZMaxFrameRate(string value = null);

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        GothicArguments AddArgument_ZNoMemPool(string value = null);

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        GothicArguments AddArgument_ZRnd(string value = null);

        /// <summary>Starts the game with predefined resolution.</summary>
        GothicArguments AddArgument_ZRes(string width = "800", string height = "600", string bpp = "32");

        /// <summary>Starts the game in windowed mode.</summary>
        GothicArguments AddArgument_ZWindow();

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        GothicArguments AddArgument_ZNoTex(string value = null);

        /// <summary>Starts the game with no music enabled.</summary>
        GothicArguments AddArgument_ZNoMusic();

        /// <summary>Starts the game without sound effects.</summary>
        GothicArguments AddArgument_ZNoSound();

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        GothicArguments AddArgument_ZAutoConvertData(string value = null);

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        GothicArguments AddArgument_ZAccount(string value = null);

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        GothicArguments AddArgument_ZAccountFull(string value = null);

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        GothicArguments AddArgument_DebugFightAI(string value = null);

        /// <summary>Sets the startup in game time to this value.</summary>
        GothicArguments AddArgument_Time(string hour = "00", string minute = "00");

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        GothicArguments AddArgument_ClipRange(string value = null);

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        GothicArguments AddArgument_FogRange(string value = null);

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        GothicArguments AddArgument_CamLightRange(string value = null);

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        GothicArguments AddArgument_LightRange(string value = null);

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        GothicArguments AddArgument_OutDoor(string value = null);

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        GothicArguments AddArgument_NoLazyLoad(string value = null);

        /// <summary>MISSING SUMMARY</summary>
        GothicArguments AddArgument_ZReparse();

        /// <summary>MISSING SUMMARY</summary>
        GothicArguments AddArgument_ZLog(int value = 5);
    }
}