using System.Collections.Generic;
using GothicModComposer.Models.Vdfs;

namespace GothicModComposer.Models.Configurations
{
    public class UserGmcConfiguration
	{
		public string DefaultWorld { get; set; }
		public List<IniOverride> IniOverrides { get; set; }
		public List<IniOverride> IniOverridesSystemPack { get; set; }
        public GothicVdfsConfig GothicVdfsConfig { get; set; }
		public GothicArgumentsConfiguration GothicArguments { get; set; }
    }
}