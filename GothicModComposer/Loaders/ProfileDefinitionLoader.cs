using System.Collections.Generic;
using System.Linq;
using GothicModComposer.Builders;
using GothicModComposer.Models.Configurations;
using GothicModComposer.Models.Folders;
using GothicModComposer.Models.Profiles;
using GothicModComposer.Presets;
using GothicModComposer.Utils.Exceptions;
using GothicModComposer.Utils.IOHelpers;

namespace GothicModComposer.Loaders
{
	public static class ProfileDefinitionLoader
	{
		private static readonly List<ProfileDefinition> ProfileDefinitions = new()
		{
			ProfileDefinitionPresets.GetComposeProfile(),
			ProfileDefinitionPresets.GetRestoreGothicProfile(),
			ProfileDefinitionPresets.GetRunProfile(),
			ProfileDefinitionPresets.GetUpdateProfile(),
			ProfileDefinitionPresets.GetBuildModFileProfile()
		};

		public static ProfileLoaderResponse Load(ProfilePresetType profileType, GothicFolder gothicFolder, GmcFolder gmcFolder, ModFolder modFolder, UserGmcConfiguration userGmcConfiguration)
		{
			var profileDefinition = ProfileDefinitions.Single(x => x.ProfileType == profileType);

			var profile = new Profile
			{
				GothicFolder = gothicFolder,
				GmcFolder = gmcFolder,
				ModFolder = modFolder,
				DefaultWorld = userGmcConfiguration.DefaultWorld
							   ?? profileDefinition.DefaultWorld 
                               ?? throw new DefaultWorldNotFoundException(),
				IniOverrides = userGmcConfiguration.IniOverrides 
                               ?? profileDefinition.IniOverrides
							   ?? new List<string>(),
				GothicVdfsConfig = userGmcConfiguration.GothicVdfsConfig 
								   ?? throw new VdfsGothicConfigurationNotFoundException(),
				GothicArguments = GothicArgumentsHelper.ParseGothicArguments(profileDefinition.GothicArguments)
			};

			return new ProfileLoaderResponse
			{
				Profile = profile,
				Commands = CommandBuilderHelper.ParseCommands(profileDefinition.ExecutionCommands, profile)
			};
		}
	}
}