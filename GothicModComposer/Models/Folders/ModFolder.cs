using System.Collections.Generic;
using System.IO;
using System.Linq;
using GothicModComposer.Models.Interfaces;
using GothicModComposer.Models.ModFiles;
using GothicModComposer.Presets;
using GothicModComposer.Utils.IOHelpers;

namespace GothicModComposer.Models.Folders
{
    public class ModFolder : IModFolder
    {
		public string BasePath { get; }
		public string ExtensionsFolderPath => Path.Combine(BasePath, "Extensions");

		private ModFolder(string modFolderPath)
		{
			BasePath = modFolderPath;
		}

		public static ModFolder CreateFromPath(string modFolderPath)
		{
			var instance = new ModFolder(modFolderPath);

			instance.Verify();

			return instance;
		}

		public List<ModFileEntry> GetAllModFiles()
		{
			return AssetPresetFolders.FoldersWithAssets
				.SelectMany(assetType =>
				{
					var absolutePath = Path.Combine(BasePath, assetType.ToString());
					var files = DirectoryHelper.GetAllFilesInDirectory(absolutePath);

					var modFiles = new List<ModFileEntry>();
					files.ForEach(file => modFiles.Add(new ModFileEntry(assetType, file,
						DirectoryHelper.ToRelativePath(file, BasePath))));

					return modFiles;
				})
				.ToList();
		}

		private void Verify()
		{
			// TODO: Verify if the folder exists and is correct
		}
	}
}