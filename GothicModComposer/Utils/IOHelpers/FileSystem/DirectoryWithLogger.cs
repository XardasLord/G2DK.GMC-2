using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;

namespace GothicModComposer.Utils.IOHelpers.FileSystem
{
    public class DirectoryWithLogger : IDirectoryWithLogger
    {
        protected internal DirectoryWithLogger(IFileSystemWithLogger fileSystem)
            => FileSystem = fileSystem;

        public IDirectoryInfo CreateDirectory(string path)
        {
            DirectoryHelper.CreateIfDoesNotExist(path);
            return null;
        }

        public void Delete(string path) => DirectoryHelper.DeleteIfExists(path);

        public bool Exists(string path) => Directory.Exists(path);

        public void Copy(string sourceDirName, string destDirName) => DirectoryHelper.Copy(sourceDirName, destDirName);

        public List<string> GetDirectories(string path) => Directory.GetDirectories(path).ToList();

        public void Move(string sourceDirName, string destDirName) => DirectoryHelper.Move(sourceDirName, destDirName);

        public IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption) 
            => Directory.Exists(path) ? Directory.EnumerateFiles(path, searchPattern, searchOption) : new List<string>();

        public IFileSystemWithLogger FileSystem { get; }
    }
}