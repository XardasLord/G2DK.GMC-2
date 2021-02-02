using System.IO;
using GothicModComposer.Loaders;
using GothicModComposer.Models;
using GothicModComposer.Presets;

namespace GothicModComposer.Builders
{
	public static class GmcManagerBuilder
	{
		public static GmcManager PrepareGmcExecutor(ProfilePresetType profileType, string absolutePathToProject)
		{
			var userGmcConfig = UserGmcConfigurationLoader.Load(absolutePathToProject);
			var gmcFolderPath = Path.Combine(userGmcConfig.GothicRoot, ".gmc");

			var gothicFolder = GothicFolder.CreateFromPath(userGmcConfig.GothicRoot);
			var gmcFolder = GmcFolder.CreateFromPath(gmcFolderPath);
			var modFolder = ModFolder.CreateFromPath(absolutePathToProject);
			var profileDefinition = ProfilePresetDefinitionLoader.Load(profileType, gothicFolder, gmcFolder, modFolder, userGmcConfig);

			return GmcManager.Create(gothicFolder, gmcFolder, modFolder, profileDefinition);
		}
	}
}