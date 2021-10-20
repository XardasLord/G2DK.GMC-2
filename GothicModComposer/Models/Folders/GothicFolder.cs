using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GothicModComposer.Models.IniFiles;
using GothicModComposer.Models.Interfaces;
using GothicModComposer.Models.ModFiles;
using GothicModComposer.Presets;
using GothicModComposer.Utils.IOHelpers;

namespace GothicModComposer.Models.Folders
{
    public class GothicFolder : IGothicFolder
    {
        private GothicFolder(string gothicFolderPath)
            => BasePath = gothicFolderPath;

        public string BasePath { get; }
        public string SystemFolderPath => Path.Combine(BasePath, "System");
        public string WorkFolderPath => Path.Combine(BasePath, "_Work");
        public string DataFolderPath => Path.Combine(BasePath, "Data");
        public string WorkDataFolderPath => Path.Combine(WorkFolderPath, "Data");
        public string CompiledTexturesPath => Path.Combine(WorkDataFolderPath, AssetPresetType.Textures.ToString(), "_compiled");
        public string VideoBikFolderPath => Path.Combine(WorkDataFolderPath, "Video");
        public string GmcIniFilePath => Path.Combine(SystemFolderPath, "GMC.ini");
        public string GothicIniFilePath => Path.Combine(SystemFolderPath, "Gothic.ini");
        public string SystemPackIniFilePath => Path.Combine(SystemFolderPath, "SystemPack.ini");
        public string GothicExeFilePath => Path.Combine(SystemFolderPath, "Gothic2.exe");
        public string GothicSrcFilePath => Path.Combine(WorkDataFolderPath, "Scripts", "Content", "Gothic.src");
        public string CutsceneFolderPath => Path.Combine(WorkDataFolderPath, "Scripts", "Content", "Cutscene");
        public string GothicVdfsToolFilePath => Path.Combine(WorkFolderPath, "Tools", "VDFS", "GothicVDFS.exe");

        public static GothicFolder CreateFromPath(string gothicFolderPath)
        {
            var instance = new GothicFolder(gothicFolderPath);

            instance.Verify();

            return instance;
        }

        public string GetGothicIniContent(bool removeComments = true)
        {
            var gothicIni = File.ReadAllText(GothicIniFilePath);

            if (removeComments)
            {
                var commentRegex = new Regex(IniFileHelper.CommentRegex, RegexOptions.Multiline);
                gothicIni = commentRegex.Replace(gothicIni, "");
            }

            return gothicIni.Replace("\r", "");
        }

        public string GetSystemPackIniContent(bool removeComments = true)
        {
            var systemPackIni = File.ReadAllText(SystemPackIniFilePath);

            if (removeComments)
            {
                var commentRegex = new Regex(IniFileHelper.CommentRegex, RegexOptions.Multiline);
                systemPackIni = commentRegex.Replace(systemPackIni, "");
            }

            return systemPackIni.Replace("\r", "");
        }

        public void SaveGmcIni(List<IniBlock> iniBlocks)
        {
            var iniContent = GothicIniWriter.GenerateContent(iniBlocks);
            FileHelper.SaveContent(GmcIniFilePath, iniContent, Encoding.Default);
        }

        public void DeleteGmcIni()
        {
            FileHelper.DeleteIfExists(GmcIniFilePath);
        }

        public int GetNumberOfTexturesToCompile()
        {
            var texturesDirectory = Path.Combine(WorkDataFolderPath, AssetPresetType.Textures.ToString());
            var compiledTexturesDirectory = Path.Combine(WorkDataFolderPath, AssetPresetType.Textures.ToString(), "_compiled");

            var textureFiles = new List<ModFileEntry>();

            DirectoryHelper.GetAllFilesInDirectory(texturesDirectory)
                .ForEach(file =>
                {
                    if (file.Contains("_compiled"))
                        return;

                    textureFiles.Add(new ModFileEntry(AssetPresetType.Textures, file,
                        DirectoryHelper.ToRelativePath(file, BasePath)));
                });

            var compiledTextureFiles = DirectoryHelper.GetAllFilesInDirectory(compiledTexturesDirectory);

            return textureFiles.Count(tex => compiledTextureFiles.All(compiled => Path.GetFileName(compiled) != tex.GetCompiledFileName()));
        }

        private void Verify()
        {
            // TODO: Verify if the folder exists and is correct
        }
    }
}