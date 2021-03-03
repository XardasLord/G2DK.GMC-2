using System.Collections.Generic;
using GothicModComposer.Builders;
using GothicModComposer.Models.Profiles;

namespace GothicModComposer.Presets
{
    public static class ProfileDefinitionPresets
	{
		public static ProfileDefinition GetComposeProfile()
			=> new()
			{
				ProfileType = ProfilePresetType.Compose,
				IniOverrides = new List<string>(),
				GothicArguments = GothicArgumentsPresets.Build().ToList(),
				ExecutionCommands = new List<string>
				{
					CommandBuilderHelper.CreateBackupCommand,
					CommandBuilderHelper.OverrideIniCommand,
					CommandBuilderHelper.AddDefaultWorldCommand,
					CommandBuilderHelper.EnableVdfFilesCommand,
					CommandBuilderHelper.ClearWorkDataCommand,
					CommandBuilderHelper.CopyEssentialAssetFilesFromBackupCommand,
					CommandBuilderHelper.UpdateModDataFilesCommand,
					CommandBuilderHelper.UpdateModExtensionFilesCommand,
					CommandBuilderHelper.UpdateDialoguesCommand,
					CommandBuilderHelper.ExecuteGothicKillOnLoadCommand,
					CommandBuilderHelper.DisableVdfFilesCommand,
					CommandBuilderHelper.ClearGmcTemporaryFiles
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

		public static ProfileDefinition GetRunProfile()
			=> new()
			{
				ProfileType = ProfilePresetType.Run,
				GothicArguments = GothicArgumentsPresets.Run().ToList(),
				ExecutionCommands = new List<string>
				{
					CommandBuilderHelper.OverrideIniCommand,
					CommandBuilderHelper.AddDefaultWorldCommand,
					CommandBuilderHelper.EnableVdfFilesCommand,
					CommandBuilderHelper.ExecuteGothicCommand,
					CommandBuilderHelper.DisableVdfFilesCommand,
					CommandBuilderHelper.ClearGmcTemporaryFiles
				}
			};

		public static ProfileDefinition GetUpdateProfile()
			=> new()
			{
				ProfileType = ProfilePresetType.Update,
				GothicArguments = GothicArgumentsPresets.Build().ToList(),
				ExecutionCommands = new List<string>
				{
					CommandBuilderHelper.CreateBackupCommand,
					CommandBuilderHelper.OverrideIniCommand,
					CommandBuilderHelper.AddDefaultWorldCommand,
					CommandBuilderHelper.EnableVdfFilesCommand,
					CommandBuilderHelper.RemoveNotCompiledSourcesCommand,
					CommandBuilderHelper.UpdateModDataFilesCommand,
					CommandBuilderHelper.UpdateModExtensionFilesCommand,
					CommandBuilderHelper.UpdateDialoguesCommand,
					CommandBuilderHelper.ExecuteGothicKillOnLoadCommand,
					CommandBuilderHelper.DisableVdfFilesCommand,
					CommandBuilderHelper.ClearGmcTemporaryFiles
				}
			};

        public static ProfileDefinition GetBuildModFileProfile()
            => new()
            {
                ProfileType = ProfilePresetType.BuildModFile,
                GothicArguments = GothicArgumentsPresets.Default().ToList(),
                ExecutionCommands = new List<string>
                {
                    CommandBuilderHelper.BuildModFileCommand
				}
            };
    }
}