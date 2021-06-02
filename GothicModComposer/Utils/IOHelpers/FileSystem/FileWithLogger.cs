using System.IO;
using System.Text;

namespace GothicModComposer.Utils.IOHelpers.FileSystem
{
    public class FileWithLogger : IFileWithLogger
    {
        public IFileSystemWithLogger FileSystem { get; }

        protected internal FileWithLogger(IFileSystemWithLogger fileSystem) 
            => FileSystem = fileSystem;

        public void Copy(string sourceFileName, string destFileName) => FileHelper.Copy(sourceFileName, destFileName);

        public void CopyWithOverride(string sourceFileName, string destFileName) => FileHelper.CopyWithOverwrite(sourceFileName, destFileName);

        public void Delete(string path) => FileHelper.DeleteIfExists(path);

        public bool Exists(string path) => File.Exists(path);

        public void Move(string sourceFileName, string destFileName) => FileHelper.Move(sourceFileName, destFileName);

        public void MoveWithOverride(string sourceFileName, string destFileName) => FileHelper.MoveWithOverwrite(sourceFileName, destFileName);

        public void Rename(string oldNamePath, string newNamePath) => FileHelper.Rename(oldNamePath, newNamePath);

        public string ReadAllText(string path) => File.ReadAllText(path);

        public string ReadAllText(string path, Encoding encoding) => File.ReadAllText(path, encoding);

        public void WriteAllText(string path, string contents) => File.WriteAllText(path, contents);

        public void WriteAllText(string path, string contents, Encoding encoding) => File.WriteAllText(path, contents, encoding);
    }
}