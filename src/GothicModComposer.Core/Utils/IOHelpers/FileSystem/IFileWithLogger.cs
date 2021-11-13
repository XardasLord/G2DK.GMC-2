using System.Text;

namespace GothicModComposer.Core.Utils.IOHelpers.FileSystem
{
    public interface IFileWithLogger
    {
        IFileSystemWithLogger FileSystem { get; }

        void Copy(string sourceFileName, string destFileName);

        void CopyWithOverride(string sourceFileName, string destFileName);

        void Delete(string path);

        bool Exists(string path);

        void Move(string sourceFileName, string destFileName);

        void MoveWithOverride(string sourceFileName, string destFileName);

        void Rename(string oldNamePath, string newNamePath);

        string ReadAllText(string path);

        string ReadAllText(string path, Encoding encoding);

        void WriteAllText(string path, string contents);

        void WriteAllText(string path, string contents, Encoding encoding);
    }
}