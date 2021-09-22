using System;
using System.IO;
using GothicModComposer.Presets;
using GothicModComposer.Utils.IOHelpers;

namespace GothicModComposer.Models.ModFiles
{
    public class ModFileEntry
    {
        public ModFileEntry(AssetPresetType assetType, string filePath, string relativePath)
        {
            AssetType = assetType;
            FilePath = filePath;
            RelativePath = relativePath;
            Timestamp = GetFileTimestamp();
        }

        public AssetPresetType AssetType { get; set; }
        public string FilePath { get; set; }
        public string RelativePath { get; set; }
        public long Timestamp { get; set; }

        private long GetFileTimestamp()
            => FileHelper.GetFileTimestamp(FilePath);

        public ModFileEntryOperation GetEntryOperationForFile(string filePath)
            => Timestamp < FileHelper.GetFileTimestamp(filePath)
                ? ModFileEntryOperation.Update
                : ModFileEntryOperation.None;

        public string GetCompiledFileName()
            => AssetType switch
            {
                AssetPresetType.Textures => $"{Path.GetFileNameWithoutExtension(FilePath)}-C.TEX",
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

        public bool ExistsInModFiles() => FileHelper.Exists(FilePath);

        public bool DoesNeedGothicCompilation() => AssetType != AssetPresetType.Sound
                                                   && AssetType != AssetPresetType.Music
                                                   && AssetType != AssetPresetType.Video
                                                   && AssetType != AssetPresetType.Worlds
                                                   && !FilePath.Contains(@"Meshes\Level");

        public bool DoesNeedDialoguesUpdate() => AssetType == AssetPresetType.Scripts
                                                 && FilePath.Contains(@"Content\Story\Dialoge");
    }
}