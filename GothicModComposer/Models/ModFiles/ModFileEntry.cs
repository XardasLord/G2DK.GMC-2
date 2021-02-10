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

		public ModFileEntryOperation GetEntryOperation(string filePath) 
			=> Timestamp < FileHelper.GetFileTimestamp(filePath)
				? ModFileEntryOperation.Update
				: ModFileEntryOperation.None;
	}

	public enum ModFileEntryOperation
	{
		None,
		Create,
		Update
	}
}