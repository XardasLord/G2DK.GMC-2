using System.Collections.Generic;
using System.IO;
using System.Linq;
using GothicModComposer.Core.Models.Interfaces;
using GothicModComposer.Core.Models.ModFiles;
using GothicModComposer.Core.Presets;
using GothicModComposer.Core.Utils.IOHelpers;

namespace GothicModComposer.Core.Models.Folders
{
    public class ModFolder : IModFolder
    {
        private ModFolder(string modFolderPath) => BasePath = modFolderPath;

        public string BasePath { get; }
        public string ExtensionsFolderPath => Path.Combine(BasePath, "Extensions");

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

        public static ModFolder CreateFromPath(string modFolderPath)
        {
            var instance = new ModFolder(modFolderPath);

            instance.Verify();

            return instance;
        }

        private void Verify()
        {
            // TODO: Verify if the folder exists and is correct
        }
    }
}