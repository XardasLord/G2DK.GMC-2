using System.Collections.Generic;
using System.IO;

namespace GothicModComposer.Core.Utils.IOHelpers.FileSystem
{
    public interface IDirectoryWithLogger
    {
        IFileSystemWithLogger FileSystem { get; }

        bool CreateIfNotExist(string path);

        void Delete(string path);

        bool Exists(string path);

        void Copy(string sourceDirName, string destDirName);

        void Move(string sourceDirName, string destDirName);

        List<string> GetDirectories(string path);

        List<string> GetAllFilesInDirectory(string directoryPath,
            SearchOption searchOption = SearchOption.AllDirectories);
    }
}