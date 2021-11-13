using System.Collections.Generic;
using GothicModComposer.Core.Models.Configurations;
using GothicModComposer.Core.Models.Interfaces;
using GothicModComposer.Core.Presets;

namespace GothicModComposer.Core.Models.Profiles
{
    public interface IProfile
    {
        ProfilePresetType ProfileType { get; set; }
        IGothicFolder GothicFolder { get; set; }
        IGmcFolder GmcFolder { get; set; }
        IModFolder ModFolder { get; set; }
        string DefaultWorld { get; set; }
        List<IniOverride> IniOverrides { get; set; }
        List<IniOverride> IniOverridesSystemPack { get; set; }
        IGothicArguments GothicArguments { get; set; }
        IGothicVdfsConfig GothicVdfsConfig { get; set; }
        ICommandsConditions CommandsConditions { get; set; }
        IGothicArgumentsConfiguration GothicArgumentsForceConfig { get; set; }
    }
}