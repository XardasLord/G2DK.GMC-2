using System;
using System.IO;
using GothicModComposer.Presets;
using GothicModComposer.Utils.IOHelpers;

namespace GothicModComposer.Models.ModFiles
{
	public class ModFileEntry
	{
		public AssetPresetType AssetType { get; set; }
		public string FilePath { get; set; }
		public string RelativePath { get; set; }
		public long Timestamp { get; set; }

		public ModFileEntry(AssetPresetType assetType, string filePath, string relativePath)
		{
			AssetType = assetType;
			FilePath = filePath;
			RelativePath = relativePath;
			Timestamp = GetFileTimestamp();
		}

		private long GetFileTimestamp()
			=> FileHelper.GetFileTimestamp(FilePath);

		public ModFileEntryOperation GetEntryOperationForFile(string filePath) 
			=> Timestamp < FileHelper.GetFileTimestamp(filePath)
				? ModFileEntryOperation.Update
				: ModFileEntryOperation.None;

		public string GetCompiledFileName()
			=> AssetType switch
			{
				AssetPresetType.Textures => $"{Path.GetFileNameWithoutExtension(FilePath)}-C{Path.GetExtension(FilePath)}",
				AssetPresetType.Meshes => $"{Path.GetFileNameWithoutExtension(FilePath)}.MRM",
				AssetPresetType.Scripts => null, // TODO
				AssetPresetType.Anims => null, // TODO: Parser needed from mark56
				AssetPresetType.Music => null,
				AssetPresetType.Presets => null,
				AssetPresetType.Sound => null,
				AssetPresetType.Video => null,
				AssetPresetType.Worlds => null,
				_ => throw new Exception("Unrecognized asset preset type.")
			};
	}

	public enum ModFileEntryOperation
	{
		None,
		Create,
		Update
	}
}