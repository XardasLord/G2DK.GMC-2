using System;
using System.Collections.Generic;
using GothicModComposer.Commands;
using GothicModComposer.Models;
using GothicModComposer.Presets;

namespace GothicModComposer.Loaders
{
	public static class ProfilePresetDefinitionLoader
	{
		public static ProfileDefinition Load(ProfilePresetType profileType, GothicFolder gothicFolder, GmcFolder gmcFolder, ModFolder modFolder)
		{
			return profileType switch
			{
				ProfilePresetType.Reset => GetResetProfile(gothicFolder, gmcFolder, modFolder),
				ProfilePresetType.RestoreGothic => GetRestoreGothicProfile(gothicFolder, gmcFolder),
				_ => throw new ArgumentOutOfRangeException(nameof(profileType), profileType, null)
			};
		}

		private static ProfileDefinition GetResetProfile(GothicFolder gothicFolder, GmcFolder gmcFolder, ModFolder modFolder)
			=> new ProfileDefinition
			{
				ProfileType = ProfilePresetType.Reset,
				IniOverrides = new List<string>(),
				ExecutionCommands = new List<ICommand>
				{
					new CreateBackupCommand(gothicFolder, gmcFolder, modFolder)
				}
			};

		private static ProfileDefinition GetRestoreGothicProfile(GothicFolder gothicFolder, GmcFolder gmcFolder)
			=> new ProfileDefinition
			{
				ProfileType = ProfilePresetType.RestoreGothic,
				ExecutionCommands = new List<ICommand>
				{
					new RestoreGothicBackupCommand(gothicFolder, gmcFolder)
				}
			};
	}
}