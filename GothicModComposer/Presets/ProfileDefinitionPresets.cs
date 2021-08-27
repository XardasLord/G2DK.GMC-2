using System.Collections.Generic;
using GothicModComposer.Builders;
using GothicModComposer.Models.Configurations;
using GothicModComposer.Models.Profiles;

namespace GothicModComposer.Presets
{
    public static class ProfileDefinitionPresets
    {
	    private const string DefaultWorldName = "TestWorld.ZEN";

		public static ProfileDefinition GetComposeProfile()
			=> new()
			{
				ProfileType = ProfilePresetType.Compose,
				IniOverrides = new List<IniOverride>(),
				GothicArguments = GothicArgumentsPresets.Compose().ToList(),
				CommandsConditions = new CommandsConditions
                {
					ExecuteGothicStepRequired =  true,
					UpdateDialoguesStepRequired = true
                },
				ExecutionCommands = new List<string>
				{
					CommandBuilderHelper.CreateBackupCommand,
					CommandBuilderHelper.AddDefaultWorldCommand,
					CommandBuilderHelper.EnableVdfFilesCommand,
					CommandBuilderHelper.ClearWorkDataCommand,
					CommandBuilderHelper.CopyEssentialAssetFilesFromBackupCommand,
					CommandBuilderHelper.UpdateModDataFilesCommand,
					CommandBuilderHelper.UpdateModExtensionFilesCommand,
					CommandBuilderHelper.DisableVideoBikFilesCommand,
					CommandBuilderHelper.ExecuteGothicKillOnLoadCommand,
					CommandBuilderHelper.EnableVideoBikFilesCommand,
					CommandBuilderHelper.UpdateDialoguesCommand,
					CommandBuilderHelper.RemoveNotCompiledSourcesCommand,
					CommandBuilderHelper.DisableVdfFilesCommand,
					CommandBuilderHelper.ClearGmcTemporaryFiles
				}
			};

		public static ProfileDefinition GetRestoreGothicProfile()
			=> new()
			{
				ProfileType = ProfilePresetType.RestoreGothic,
				GothicArguments = GothicArgumentsPresets.Default().ToList(),
				DefaultWorld = DefaultWorldName,
				ExecutionCommands = new List<string>
				{
					CommandBuilderHelper.RestoreGothicBackupCommand,
					CommandBuilderHelper.DisableVdfFilesCommand
				}
			};

		public static ProfileDefinition GetRunProfile()
			=> new()
			{
				ProfileType = ProfilePresetType.RunMod,
				GothicArguments = GothicArgumentsPresets.Run().ToList(),
                CommandsConditions = new CommandsConditions
                {
                    ExecuteGothicStepRequired = true
                },
				ExecutionCommands = new List<string>
				{
					CommandBuilderHelper.OverrideIniCommand,
					CommandBuilderHelper.AddDefaultWorldCommand,
					CommandBuilderHelper.EnableVdfFilesCommand,
					CommandBuilderHelper.DisableVideoBikFilesCommand,
					CommandBuilderHelper.ExecuteGothicCommand,
					CommandBuilderHelper.EnableVideoBikFilesCommand,
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
					CommandBuilderHelper.AddDefaultWorldCommand,
					CommandBuilderHelper.EnableVdfFilesCommand,
					CommandBuilderHelper.RemoveNotCompiledSourcesCommand,
					CommandBuilderHelper.UpdateModDataFilesCommand,
					CommandBuilderHelper.UpdateModExtensionFilesCommand,
					CommandBuilderHelper.DisableVideoBikFilesCommand,
					CommandBuilderHelper.ExecuteGothicKillOnLoadCommand,
					CommandBuilderHelper.EnableVideoBikFilesCommand,
					CommandBuilderHelper.UpdateDialoguesCommand,
					CommandBuilderHelper.DisableVdfFilesCommand,
					CommandBuilderHelper.ClearGmcTemporaryFiles
				}
			};

        public static ProfileDefinition GetEnableVDFProfile()
            => new()
            {
                ProfileType = ProfilePresetType.EnableVDF,
                GothicArguments = GothicArgumentsPresets.Default().ToList(),
                DefaultWorld = DefaultWorldName,
				ExecutionCommands = new List<string>
                {
                    CommandBuilderHelper.EnableVdfFilesCommand
                }
            };

		public static ProfileDefinition GetBuildModFileProfile()
            => new()
            {
                ProfileType = ProfilePresetType.BuildModFile,
                GothicArguments = GothicArgumentsPresets.Default().ToList(),
                DefaultWorld = DefaultWorldName,
				ExecutionCommands = new List<string>
                {
                    CommandBuilderHelper.BuildModFileCommand
				}
            };
    }
}