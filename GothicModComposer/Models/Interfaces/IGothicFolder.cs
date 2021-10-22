using System.Collections.Generic;
using GothicModComposer.Models.IniFiles;

namespace GothicModComposer.Models.Interfaces
{
    public interface IGothicFolder
    {
        string BasePath { get; }
        string SystemFolderPath { get; }
        string WorkFolderPath { get; }
        string DataFolderPath { get; }
        string WorkDataFolderPath { get; }
        string CompiledTexturesPath { get; }
        string CompiledMeshesPath { get; }
        string VideoBikFolderPath { get; }
        string GmcIniFilePath { get; }
        string GothicIniFilePath { get; }
        string SystemPackIniFilePath { get; }
        string GothicExeFilePath { get; }
        string GothicSrcFilePath { get; }
        string CutsceneFolderPath { get; }
        string GothicVdfsToolFilePath { get; }
        string GetGothicIniContent(bool removeComments = true);
        string GetSystemPackIniContent(bool removeComments = true);
        void SaveGmcIni(List<IniBlock> iniBlocks);
        void DeleteGmcIni();

        int GetNumberOfTexturesToCompile();
        int GetNumberOfMeshesToCompile();
    }
}