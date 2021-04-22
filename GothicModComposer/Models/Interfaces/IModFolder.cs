using System.Collections.Generic;
using GothicModComposer.Models.ModFiles;

namespace GothicModComposer.Models.Interfaces
{
    public interface IModFolder
    {
        string BasePath { get; }
        string ExtensionsFolderPath { get; }
        List<ModFileEntry> GetAllModFiles();
    }
}