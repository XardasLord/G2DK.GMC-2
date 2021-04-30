using System.IO;
using GothicModComposer.Loaders;
using GothicModComposer.Models.Folders;
using GothicModComposer.Presets;

namespace GothicModComposer.Builders
{
	public static class GmcManagerBuilder
	{
		public static GmcManager PrepareGmcExecutor(
            ProfilePresetType profileType, string absolutePathToProject, string absolutePathToGothic2Root, string configurationFile)
		{
			var userGmcConfig = UserGmcConfigurationLoader.Load(absolutePathToProject, configurationFile);
			var gmcFolderPath = Path.Combine(absolutePathToGothic2Root, ".gmc");

			var gothicFolder = GothicFolder.CreateFromPath(absolutePathToGothic2Root);
			var gmcFolder = GmcFolder.CreateFromPath(gmcFolderPath);
			var modFolder = ModFolder.CreateFromPath(absolutePathToProject);
			var profileResponse = ProfileDefinitionLoader.Load(profileType, gothicFolder, gmcFolder, modFolder, userGmcConfig);

			return GmcManager.Create(profileResponse);
		}
	}
}