using System;
using System.Collections.Generic;
using System.IO;
using GothicModComposer.Presets;
using GothicModComposer.Utils.IOHelpers;

namespace GothicModComposer.Models.Folders
{
	public class AssetFolder
	{
		public string BasePath { get; }
		public string AssetFolderName { get; }
		public string CompiledFolderPath => Path.Combine(BasePath, "_compiled");
		public AssetPresetType AssetType { get; }

		public List<string> SubDirectories => DirectoryHelper.GetDirectories(BasePath);

		public AssetFolder(string assetFolderPath)
		{
			BasePath = assetFolderPath;
			AssetFolderName = Path.GetFileName(BasePath);
			AssetType = Enum.Parse<AssetPresetType>(AssetFolderName);
		}

		public AssetFolder(string assetFolderPath, AssetPresetType assetType)
		{
			BasePath = assetFolderPath;
			AssetFolderName = Path.GetFileName(BasePath);
			AssetType = assetType;
		}

		public bool IsCompilable() 
			=> AssetType switch
			{
				AssetPresetType.Textures => true,
				AssetPresetType.Meshes => true,
				AssetPresetType.Scripts => true,
				AssetPresetType.Anims => true,
				AssetPresetType.Music => false,
				AssetPresetType.Presets => false,
				AssetPresetType.Sound => false,
				AssetPresetType.Video => false,
				AssetPresetType.Worlds => false,
				_ => throw new Exception("Unrecognized asset preset type.")
			};

		public bool Exists() => DirectoryHelper.Exists(BasePath);

		public void Delete() => DirectoryHelper.DeleteIfExists(BasePath);

		public void CreateCompiledFolder() => DirectoryHelper.CreateIfDoesNotExist(CompiledFolderPath);
	}
}
