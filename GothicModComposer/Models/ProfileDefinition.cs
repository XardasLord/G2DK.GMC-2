using System.Collections.Generic;
using GothicModComposer.Commands;
using GothicModComposer.Presets;

namespace GothicModComposer.Models
{
	public class ProfileDefinition
	{
		public ProfilePresetType ProfileType { get; set; }

		//public string DefaultWorld;
		//public List<string> IniOverrides;
		//public List<string> ModDirectories;
		//public GothicVdfsConfig GothicVdfsConfig;

		//public List<string> GothicArguments;
		public List<ICommand> ExecutionCommands { get; set; }
	}
}