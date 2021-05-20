using System.IO;
using GothicModComposer.Utils.IOHelpers;

namespace GothicModComposer.Models
{
    public class VideoBikFile
    {
        public string FilePath { get; }
        public string FileNameWithoutExtension { get; }
        public bool IsValidVideoBikFile { get; }
        public bool IsDisabled { get; }
        public bool IsEnabled { get; }

        private readonly string _folderPath;

        public VideoBikFile(string filePath)
        {
            FilePath = filePath;
            _folderPath = Path.GetDirectoryName(FilePath);
            
            var fileInfo = new FileInfo(filePath);

            FileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileInfo.Name);
            IsValidVideoBikFile = fileInfo.Exists;
            IsDisabled = fileInfo.Extension == ".disabled";
            IsEnabled = !IsDisabled;
        }

        public void Enable()
        {
            if (IsEnabled)
                return;
            
            var enabledPath = Path.Combine(_folderPath, $"{FileNameWithoutExtension}"); // Because it this case FileNameWithoutExtension has '.bik' extension (cause original extension is '.disabled')
            var disabledPath = $"{enabledPath}.disabled";
            
            FileHelper.Rename(disabledPath, enabledPath);
        }

        public void Disable()
        {
            if (IsDisabled)
                return;
            
            var enabledPath = Path.Combine(_folderPath, $"{FileNameWithoutExtension}.bik");
            var disabledPath = $"{enabledPath}.disabled";
            
            FileHelper.Rename(enabledPath, disabledPath);
        }
    }
}