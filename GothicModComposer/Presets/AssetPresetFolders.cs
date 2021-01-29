using System.Collections.Generic;

namespace GothicModComposer.Presets
{
	public static class AssetPresetFolders
	{
		public static List<AssetPresetType> FoldersWithAssets => new List<AssetPresetType>
		{
			AssetPresetType.Anims,
			AssetPresetType.Meshes,
			AssetPresetType.Music,
			AssetPresetType.Presets,
			AssetPresetType.Scripts,
			AssetPresetType.Sound,
			AssetPresetType.Textures,
			AssetPresetType.Video,
			AssetPresetType.Worlds
		};
	}
}
