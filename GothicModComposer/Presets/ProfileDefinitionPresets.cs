using System.Collections.Generic;
using GothicModComposer.Builders;
using GothicModComposer.Models.Profiles;

namespace GothicModComposer.Presets
{
	public static class ProfileDefinitionPresets
	{
		public static ProfileDefinition GetResetProfile()
			=> new()
			{
				ProfileType = ProfilePresetType.Reset,
				IniOverrides = new List<string>(),
				GothicArguments = GothicArgumentsPresets.Build().ToList(),
				ExecutionCommands = new List<string>
				{
					CommandBuilderHelper.CreateBackupCommand,
					CommandBuilderHelper.OverrideIniCommand,
					CommandBuilderHelper.AddDefaultWorldCommand,
					CommandBuilderHelper.EnableVdfFilesCommand,
					CommandBuilderHelper.ClearWorkDataCommand,
					//CommandBuilderHelper.CopyEssentialFilesFromBackupCommand,
					CommandBuilderHelper.UpdateModDataCommand
				}
			};

		public static ProfileDefinition GetRestoreGothicProfile()
			=> new()
			{
				ProfileType = ProfilePresetType.RestoreGothic,
				GothicArguments = GothicArgumentsPresets.Default().ToList(),
				ExecutionCommands = new List<string>
				{
					CommandBuilderHelper.RestoreGothicBackupCommand
				}
			};
	}
}