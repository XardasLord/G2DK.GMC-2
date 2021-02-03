using System.Collections.Generic;
using GothicModComposer.Presets;

namespace GothicModComposer.Models.Profiles
{
	public class ProfileDefinition
	{
		public ProfilePresetType ProfileType { get; set; }
		public string DefaultWorld { get; set; }
		public List<string> IniOverrides { get; set; }

		//public List<string> ModDirectories { get; set; }
		public List<string> GothicArguments { get; set; }
		public List<string> ExecutionCommands { get; set; }
	}
}