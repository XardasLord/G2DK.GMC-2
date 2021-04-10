using System;
using System.Collections.Generic;
using System.IO;
using GothicModComposer.Commands;
using GothicModComposer.Models.Profiles;
using GothicModComposer.Presets;
using GothicModComposer.Utils.IOHelpers.FileSystem;
using Moq;
using Xunit;

namespace GothicModComposer.UnitTests.Commands
{
    public class CreateBackupCommandTests
    {
        private readonly Mock<IProfile> _profileMock;
        private readonly Mock<IFileSystemWithLogger> _fileSystemMock;

        public CreateBackupCommandTests()
        {
            _profileMock = new Mock<IProfile>();
            _fileSystemMock = new Mock<IFileSystemWithLogger>();
        }

        private void Act() => new CreateBackupCommand(_profileMock.Object, _fileSystemMock.Object).Execute();

        [Fact]
        public void Execute_WhenBackupFolderExists_ShouldReturn()
        {
            _profileMock.SetupGet(x => x.GmcFolder.DoesBackupFolderExist).Returns(true);

            Act();

            _profileMock.Verify(x => x.GmcFolder.CreateBackupWorkDataFolder(), Times.Never);
        }

        [Fact]
        public void Execute_WhenBackupFolderNotExist_ShouldCreateBackupFolder()
        {
            const string gothicWorkDataFolderPath = "C:/WorkDataFolderPath";
            const string modExtensionsFolderPath = "C:/ExtensionsFolderPath";
            const string gmcBackupWorkDataFolderPath = "C:/BackupWorkDataFolderPath";
            const string assetSourceDirectoryPath = "SourcePath";
            const string assetDestinationDirectoryPath = "DestinationPath";

            _profileMock.SetupGet(x => x.GmcFolder.DoesBackupFolderExist).Returns(false);
            _profileMock.SetupGet(x => x.GmcFolder.BackupWorkDataFolderPath).Returns(gmcBackupWorkDataFolderPath);
            _profileMock.SetupGet(x => x.GothicFolder.WorkDataFolderPath).Returns(gothicWorkDataFolderPath);
            _profileMock.SetupGet(x => x.ModFolder.ExtensionsFolderPath).Returns(modExtensionsFolderPath);
            _fileSystemMock.Setup(x => x.Path.Combine(gothicWorkDataFolderPath, It.IsAny<string>())).Returns(assetSourceDirectoryPath);
            _fileSystemMock.Setup(x => x.Path.Combine(gmcBackupWorkDataFolderPath, It.IsAny<string>())).Returns(assetDestinationDirectoryPath);
            _fileSystemMock.Setup(x => x.Directory.Exists(assetSourceDirectoryPath)).Returns(false);
            _fileSystemMock.Setup(x => x.Directory.Exists(modExtensionsFolderPath)).Returns(false);

            Act();

            _profileMock.Verify(x => x.GmcFolder.CreateBackupWorkDataFolder(), Times.Once);
        }

        [Fact]
        public void Execute_WhenAssetDirectoryInGothicNotExist_ShouldNotMoveAssetFolderToBackup()
        {
            const string gothicWorkDataFolderPath = "C:/WorkDataFolderPath";
            const string modExtensionsFolderPath = "C:/ExtensionsFolderPath";
            const string gmcBackupWorkDataFolderPath = "C:/BackupWorkDataFolderPath";
            const string assetSourceDirectoryPath = "SourcePath";
            const string assetDestinationDirectoryPath = "DestinationPath";

            _profileMock.SetupGet(x => x.GmcFolder.DoesBackupFolderExist).Returns(false);
            _profileMock.SetupGet(x => x.GmcFolder.BackupWorkDataFolderPath).Returns(gmcBackupWorkDataFolderPath);
            _profileMock.SetupGet(x => x.GothicFolder.WorkDataFolderPath).Returns(gothicWorkDataFolderPath);
            _profileMock.SetupGet(x => x.ModFolder.ExtensionsFolderPath).Returns(modExtensionsFolderPath);
            _fileSystemMock.Setup(x => x.Path.Combine(gothicWorkDataFolderPath, It.IsAny<string>())).Returns(assetSourceDirectoryPath);
            _fileSystemMock.Setup(x => x.Path.Combine(gmcBackupWorkDataFolderPath, It.IsAny<string>())).Returns(assetDestinationDirectoryPath);
            _fileSystemMock.Setup(x => x.Directory.Exists(assetSourceDirectoryPath)).Returns(false);

            Act();

            _fileSystemMock.Verify(x => x.Directory.Move(assetSourceDirectoryPath, assetDestinationDirectoryPath), Times.Never);
        }

        [Fact]
        public void Execute_WhenAssetDirectoryInGothicExists_ShouldMoveAllAssetFoldersToBackup()
        {
            const string gothicWorkDataFolderPath = "C:/WorkDataFolderPath";
            const string modExtensionsFolderPath = "C:/ExtensionsFolderPath";
            const string gmcBackupWorkDataFolderPath = "C:/BackupWorkDataFolderPath";
            const string assetSourceDirectoryPath = "SourcePath";
            const string assetDestinationDirectoryPath = "DestinationPath";

            _profileMock.SetupGet(x => x.GmcFolder.DoesBackupFolderExist).Returns(false);
            _profileMock.SetupGet(x => x.GmcFolder.BackupWorkDataFolderPath).Returns(gmcBackupWorkDataFolderPath);
            _profileMock.SetupGet(x => x.GothicFolder.WorkDataFolderPath).Returns(gothicWorkDataFolderPath);
            _profileMock.SetupGet(x => x.ModFolder.ExtensionsFolderPath).Returns(modExtensionsFolderPath);
            _fileSystemMock.Setup(x => x.Path.Combine(gothicWorkDataFolderPath, It.IsAny<string>())).Returns(assetSourceDirectoryPath);
            _fileSystemMock.Setup(x => x.Path.Combine(gmcBackupWorkDataFolderPath, It.IsAny<string>())).Returns(assetDestinationDirectoryPath);
            _fileSystemMock.Setup(x => x.Directory.Exists(assetSourceDirectoryPath)).Returns(true);

            Act();
            
            var numberOfAssets = Enum.GetNames(typeof(AssetPresetType)).Length;
            _fileSystemMock.Verify(x => x.Directory.Move(assetSourceDirectoryPath, assetDestinationDirectoryPath), Times.Exactly(numberOfAssets));
        }
        

        [Fact]
        public void Execute_WhenModExtensionDirectoryNotExist_ShouldNotMoveToBackup()
        {
            const string gothicWorkDataFolderPath = "C:/WorkDataFolderPath";
            const string modExtensionsFolderPath = "C:/ExtensionsFolderPath";
            const string gmcBackupWorkDataFolderPath = "C:/BackupWorkDataFolderPath";
            const string assetSourceDirectoryPath = "SourcePath";
            const string assetDestinationDirectoryPath = "DestinationPath";

            _profileMock.SetupGet(x => x.GmcFolder.DoesBackupFolderExist).Returns(false);
            _profileMock.SetupGet(x => x.GmcFolder.BackupWorkDataFolderPath).Returns(gmcBackupWorkDataFolderPath);
            _profileMock.SetupGet(x => x.GothicFolder.WorkDataFolderPath).Returns(gothicWorkDataFolderPath);
            _profileMock.SetupGet(x => x.ModFolder.ExtensionsFolderPath).Returns(modExtensionsFolderPath);
            _fileSystemMock.Setup(x => x.Path.Combine(gothicWorkDataFolderPath, It.IsAny<string>())).Returns(assetSourceDirectoryPath);
            _fileSystemMock.Setup(x => x.Path.Combine(gmcBackupWorkDataFolderPath, It.IsAny<string>())).Returns(assetDestinationDirectoryPath);
            _fileSystemMock.Setup(x => x.Directory.Exists(assetSourceDirectoryPath)).Returns(false);
            _fileSystemMock.Setup(x => x.Directory.Exists(modExtensionsFolderPath)).Returns(false);

            Act();

            _fileSystemMock.Verify(x => x.Directory.GetAllFilesInDirectory(modExtensionsFolderPath, SearchOption.AllDirectories), Times.Never);
            _fileSystemMock.Verify(x => x.Directory.CreateIfNotExist(It.IsAny<string>()), Times.Never);
            _fileSystemMock.Verify(x => x.File.Copy(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }


        [Fact]
        public void Execute_WhenModExtensionDirectoryExists_ShouldMoveToBackup()
        {
            const string gothicWorkDataFolderPath = "C:/WorkDataFolderPath";
            const string modExtensionsFolderPath = "C:/ExtensionsFolderPath";
            const string gmcBackupWorkDataFolderPath = "C:/BackupWorkDataFolderPath";
            const string assetSourceDirectoryPath = "SourcePath";
            const string assetDestinationDirectoryPath = "DestinationPath";
            const string gothicBasePath = "C:/BasePath";
            const string gmcBackupFolderPath = "C:/BackupFolderPath";

            _profileMock.SetupGet(x => x.GmcFolder.DoesBackupFolderExist).Returns(false);
            _profileMock.SetupGet(x => x.GmcFolder.BackupWorkDataFolderPath).Returns(gmcBackupWorkDataFolderPath);
            _profileMock.SetupGet(x => x.GothicFolder.WorkDataFolderPath).Returns(gothicWorkDataFolderPath);
            _profileMock.SetupGet(x => x.ModFolder.ExtensionsFolderPath).Returns(modExtensionsFolderPath);
            _profileMock.SetupGet(x => x.GothicFolder.BasePath).Returns(gothicBasePath);
            _profileMock.SetupGet(x => x.GmcFolder.BackupFolderPath).Returns(gmcBackupFolderPath);
            _fileSystemMock.Setup(x => x.Path.Combine(gothicWorkDataFolderPath, It.IsAny<string>())).Returns(assetSourceDirectoryPath);
            _fileSystemMock.Setup(x => x.Path.Combine(gmcBackupWorkDataFolderPath, It.IsAny<string>())).Returns(assetDestinationDirectoryPath);
            _fileSystemMock.Setup(x => x.Directory.Exists(assetSourceDirectoryPath)).Returns(false);

            _fileSystemMock.Setup(x => x.Directory.Exists(modExtensionsFolderPath)).Returns(true);
            _fileSystemMock
                .Setup(x => x.Directory.GetAllFilesInDirectory(modExtensionsFolderPath, SearchOption.AllDirectories))
                .Returns(new List<string> { "File1", "File2" });
            _fileSystemMock
                .Setup(x => x.Path.GetRelativePath(modExtensionsFolderPath, It.IsAny<string>()))
                .Returns("RelativePath");
            _fileSystemMock
                .Setup(x => x.Path.Combine(gothicBasePath, "RelativePath"))
                .Returns("ExtensionFileGothicPath");
            _fileSystemMock
                .Setup(x => x.Path.Combine(gmcBackupFolderPath, "RelativePath"))
                .Returns("ExtensionFileGmcBackupPath");
            _fileSystemMock
                .Setup(x => x.File.Exists("ExtensionFileGothicPath"))
                .Returns(true);
            _fileSystemMock
                .Setup(x => x.Path.GetDirectoryName("ExtensionFileGmcBackupPath"))
                .Returns("Dir");
            _fileSystemMock
                .Setup(x => x.Directory.Exists("Dir"))
                .Returns(false);

            Act();

            _fileSystemMock.Verify(x => x.Directory.GetAllFilesInDirectory(modExtensionsFolderPath, SearchOption.AllDirectories), Times.Once);
            _fileSystemMock.Verify(x => x.Directory.CreateIfNotExist(It.IsAny<string>()), Times.Exactly(2));
            _fileSystemMock.Verify(x => x.File.Copy(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
        }
    }
}