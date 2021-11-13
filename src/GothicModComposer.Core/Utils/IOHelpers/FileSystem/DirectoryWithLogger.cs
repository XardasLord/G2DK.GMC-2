using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GothicModComposer.Core.Utils.IOHelpers.FileSystem
{
    public class DirectoryWithLogger : IDirectoryWithLogger
    {
        protected internal DirectoryWithLogger(IFileSystemWithLogger fileSystem)
            => FileSystem = fileSystem;

        public IFileSystemWithLogger FileSystem { get; }

        public bool CreateIfNotExist(string path) => DirectoryHelper.CreateIfDoesNotExist(path);

        public void Delete(string path) => DirectoryHelper.DeleteIfExists(path);

        public bool Exists(string path) => Directory.Exists(path);

        public void Copy(string sourceDirName, string destDirName) => DirectoryHelper.Copy(sourceDirName, destDirName);

        public void Move(string sourceDirName, string destDirName) => DirectoryHelper.Move(sourceDirName, destDirName);

        public List<string> GetDirectories(string path) => Directory.GetDirectories(path).ToList();

        public List<string> GetAllFilesInDirectory(string directoryPath,
            SearchOption searchOption = SearchOption.AllDirectories)
            => Directory.Exists(directoryPath)
                ? Directory.EnumerateFiles(directoryPath, "*", searchOption).ToList()
                : new List<string>();
    }
}