using System.Collections.Generic;
using GothicModComposer.Models.Configurations;
using GothicModComposer.Presets;

namespace GothicModComposer.Models.Profiles
{
    public class ProfileDefinition
    {
        public ProfilePresetType ProfileType { get; set; }
        public string DefaultWorld { get; set; }
        public List<IniOverride> IniOverrides { get; set; }
        public List<IniOverride> IniOverridesSystemPack { get; set; }
        public List<string> GothicArguments { get; set; }
        public List<string> ExecutionCommands { get; set; }
        public CommandsConditions CommandsConditions { get; set; }
    }
}