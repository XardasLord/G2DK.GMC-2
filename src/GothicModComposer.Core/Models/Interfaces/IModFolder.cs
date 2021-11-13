using System.Collections.Generic;
using GothicModComposer.Core.Models.ModFiles;

namespace GothicModComposer.Core.Models.Interfaces
{
    public interface IModFolder
    {
        string BasePath { get; }
        string ExtensionsFolderPath { get; }
        List<ModFileEntry> GetAllModFiles();
    }
}