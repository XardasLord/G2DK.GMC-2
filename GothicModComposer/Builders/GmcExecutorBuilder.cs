using System.IO;
using GothicModComposer.Loaders;
using GothicModComposer.Models;
using GothicModComposer.Presets;

namespace GothicModComposer.Builders
{
	public static class GmcExecutorBuilder
	{
		public static GmcManager PrepareGmcExecutor(ProfilePresetType profileType, string absolutePathToProject)
		{
			var gmcConfig = UserGmcConfigurationLoader.Load(absolutePathToProject);
			var gmcFolderPath = Path.Combine(gmcConfig.GothicRoot, ".gmc");

			var gothicFolder = GothicFolder.CreateFromPath(gmcConfig.GothicRoot);
			var gmcFolder = GmcFolder.CreateFromPath(gmcFolderPath);
			var profileDefinition = ProfilePresetDefinitionLoader.Load(profileType, gothicFolder, gmcFolder);

			return GmcManager.Create(gothicFolder, gmcFolder, profileDefinition);
		}
	}
}