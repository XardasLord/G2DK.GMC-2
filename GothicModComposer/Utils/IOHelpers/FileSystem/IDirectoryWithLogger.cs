using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;

namespace GothicModComposer.Utils.IOHelpers.FileSystem
{
    public interface IDirectoryWithLogger
    {
        IFileSystemWithLogger FileSystem { get; }

        IDirectoryInfo CreateDirectory(string path);

        void Delete(string path);

        bool Exists(string path);

        void Copy(string sourceDirName, string destDirName);

        void Move(string sourceDirName, string destDirName);

        IEnumerable<string> EnumerateFiles(
          string path,
          string searchPattern,
          SearchOption searchOption);
    }
}