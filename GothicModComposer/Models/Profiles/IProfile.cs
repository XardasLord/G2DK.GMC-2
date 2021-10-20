using System.Collections.Generic;
using GothicModComposer.Models.Configurations;
using GothicModComposer.Models.Interfaces;

namespace GothicModComposer.Models.Profiles
{
    public interface IProfile
    {
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