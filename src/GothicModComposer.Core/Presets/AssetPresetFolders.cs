using System;
using System.Collections.Generic;
using System.Linq;

namespace GothicModComposer.Core.Presets
{
    public static class AssetPresetFolders
    {
        public static List<AssetPresetType> FoldersWithAssets => Enum.GetValues<AssetPresetType>().ToList();
    }
}