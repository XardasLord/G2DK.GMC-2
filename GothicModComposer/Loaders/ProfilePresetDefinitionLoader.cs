using System;
using System.Collections.Generic;
using GothicModComposer.Commands;
using GothicModComposer.Models;
using GothicModComposer.Presets;

namespace GothicModComposer.Loaders
{
	public static class ProfilePresetDefinitionLoader
	{
		public static ProfileDefinition Load(ProfilePresetType profileType, GothicFolder gothicFolder, GmcFolder gmcFolder, ModFolder modFolder, UserGmcConfiguration userGmcConfiguration)
		{
			var profile = profileType switch
			{
				ProfilePresetType.Reset => GetResetProfile(gothicFolder, gmcFolder, modFolder, userGmcConfiguration),
				ProfilePresetType.RestoreGothic => GetRestoreGothicProfile(gothicFolder, gmcFolder),
				_ => throw new ArgumentOutOfRangeException(nameof(profileType), profileType, null)
			};

			ReplaceProfileValuesWithUserConfig(profile, userGmcConfiguration);

			return profile;
		}

		private static void ReplaceProfileValuesWithUserConfig(ProfileDefinition profileDefinition, UserGmcConfiguration userGmcConfig)
		{
			profileDefinition.DefaultWorld = userGmcConfig.DefaultWorld ?? profileDefinition.DefaultWorld;
			profileDefinition.IniOverrides = userGmcConfig.IniOverrides ?? profileDefinition.IniOverrides ?? new List<string>();
		}

		private static ProfileDefinition GetResetProfile(GothicFolder gothicFolder, GmcFolder gmcFolder, ModFolder modFolder, UserGmcConfiguration userGmcConfiguration)
			=> new ProfileDefinition
			{
				ProfileType = ProfilePresetType.Reset,
				ExecutionCommands = new List<ICommand>
				{
					new CreateBackupCommand(gothicFolder, gmcFolder, modFolder),
					new OverrideIniCommand(gothicFolder, gmcFolder, userGmcConfiguration.IniOverrides ?? new List<string>())
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