﻿using System.IO;
using GothicModComposer.Loaders;
using GothicModComposer.Models;
using GothicModComposer.Presets;

namespace GothicModComposer.Builders
{
	public static class GmcManagerBuilder
	{
		public static GmcManager PrepareGmcExecutor(ProfilePresetType profileType, string absolutePathToProject)
		{
			var gmcConfig = UserGmcConfigurationLoader.Load(absolutePathToProject);
			var gmcFolderPath = Path.Combine(gmcConfig.GothicRoot, ".gmc");

			var gothicFolder = GothicFolder.CreateFromPath(gmcConfig.GothicRoot);
			var gmcFolder = GmcFolder.CreateFromPath(gmcFolderPath);
			var modFolder = ModFolder.CreateFromPath(absolutePathToProject);
			var profileDefinition = ProfilePresetDefinitionLoader.Load(profileType, gothicFolder, gmcFolder, modFolder);

			return GmcManager.Create(gothicFolder, gmcFolder, modFolder, profileDefinition);
		}
	}
}