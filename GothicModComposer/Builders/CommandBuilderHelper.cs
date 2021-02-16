﻿using System;
using System.Collections.Generic;
using GothicModComposer.Commands;
using GothicModComposer.Models.Profiles;
using GothicModComposer.Utils;

namespace GothicModComposer.Builders
{
	public static class CommandBuilderHelper
	{
		public const string RestoreGothicBackupCommand = nameof(RestoreGothicBackupCommand);
		public const string CreateBackupCommand = nameof(CreateBackupCommand);
		public const string OverrideIniCommand = nameof(OverrideIniCommand);
		public const string AddDefaultWorldCommand = nameof(AddDefaultWorldCommand);
		public const string EnableVdfFilesCommand = nameof(EnableVdfFilesCommand);
		public const string ClearWorkDataCommand = nameof(ClearWorkDataCommand);
		public const string CopyEssentialAssetFilesFromBackupCommand = nameof(CopyEssentialAssetFilesFromBackupCommand);
		public const string UpdateModDataFilesCommand = nameof(UpdateModDataFilesCommand);
		public const string UpdateModExtensionFilesCommand = nameof(UpdateModExtensionFilesCommand);
		public const string UpdateDialoguesCommand = nameof(UpdateDialoguesCommand);
		public const string ClearGmcTemporaryFiles = nameof(ClearGmcTemporaryFiles);
		public const string ExecuteGothicKillOnLoad = nameof(ExecuteGothicKillOnLoad);

		private static readonly Dictionary<string, Func<IProfile, ICommand>> Commands = new()
		{
			{ RestoreGothicBackupCommand, profile => new RestoreGothicBackupCommand(profile) },
			{ CreateBackupCommand, profile => new CreateBackupCommand(profile) },
			{ OverrideIniCommand, profile => new OverrideIniCommand(profile) },
			{ AddDefaultWorldCommand, profile => new AddDefaultWorldCommand(profile) },
			{ EnableVdfFilesCommand, profile => new EnableVdfFilesCommand(profile) },
			{ ClearWorkDataCommand, profile => new ClearWorkDataCommand(profile) },
			{ CopyEssentialAssetFilesFromBackupCommand, profile => new CopyEssentialAssetFilesFromBackupCommand(profile) },
			{ UpdateModDataFilesCommand, profile => new UpdateModDataFilesCommand(profile) },
			{ UpdateModExtensionFilesCommand, profile => new UpdateModExtensionFilesCommand(profile) },
			{ UpdateDialoguesCommand, profile => new UpdateDialoguesCommand(profile) },
			{ ClearGmcTemporaryFiles, profile => new ClearGmcTemporaryFilesCommand(profile) },
			{ ExecuteGothicKillOnLoad, profile => new ExecuteGothicCommand(profile, ExecuteGothicCommand.WorldLoadedMessage) },
		};


		public static ICommand FromString(IProfile profile, string commandName)
		{
			if (Commands.TryGetValue(commandName, out var command)) 
				return command(profile);

			Logger.Warn($"Unknown command with name: {commandName}");
			return new UnknownCommand();
		}

		public static List<ICommand> ParseCommands(List<string> stringCommands, IProfile profile)
		{
			var commands = new List<ICommand>();
			stringCommands.ForEach(commandName =>
			{
				commands.Add(FromString(profile, commandName));
			});

			return commands;
		}
	}
}