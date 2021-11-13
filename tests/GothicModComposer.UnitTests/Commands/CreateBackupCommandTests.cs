using System;
using System.Collections.Generic;
using System.IO;
using GothicModComposer.Core.Commands;
using GothicModComposer.Core.Models.Profiles;
using GothicModComposer.Core.Presets;
using GothicModComposer.Core.Utils.IOHelpers.FileSystem;
using Moq;
using Xunit;

namespace GothicModComposer.UnitTests.Commands
{
    public class CreateBackupCommandTests : TestsFixture
    {
        private readonly Mock<IFileSystemWithLogger> _fileSystemMock;
        private readonly Mock<IProfile> _profileMock;

        public CreateBackupCommandTests()
        {
            _profileMock = ProfileMock;
            _fileSystemMock = FileSystemMock;
        }

        private void Act()
            => new CreateBackupCommand(_profileMock.Object, _fileSystemMock.Object).Execute();

        [Fact]
        public void Execute_WhenBackupFolderExists_ShouldReturn()
        {
            _profileMock.SetupGet(x => x.GmcFolder.DoesBackupFolderExist).Returns(true);

            Act();

            _profileMock.Verify(x => x.GmcFolder.CreateBackupWorkDataFolder(), Times.Never);
            _fileSystemMock.Verify(x => x.Directory.Move(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _fileSystemMock.Verify(x => x.Directory.Copy(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void Execute_WhenBackupFolderNotExist_ShouldCreateBackupFolder()
        {
            const string assetSourceDirectoryPath = "SourcePath";
            const string assetDestinationDirectoryPath = "DestinationPath";

            _profileMock.SetupGet(x => x.GmcFolder.DoesBackupFolderExist).Returns(false);
            _fileSystemMock.Setup(x => x.Path.Combine(GothicWorkDataFolderPath, It.IsAny<string>()))
                .Returns(assetSourceDirectoryPath);
            _fileSystemMock.Setup(x => x.Path.Combine(GmcBackupWorkDataFolderPath, It.IsAny<string>()))
                .Returns(assetDestinationDirectoryPath);
            _fileSystemMock.Setup(x => x.Directory.Exists(assetSourceDirectoryPath)).Returns(false);
            _fileSystemMock.Setup(x => x.Directory.Exists(ModExtensionsFolderPath)).Returns(false);

            Act();

            _profileMock.Verify(x => x.GmcFolder.CreateBackupWorkDataFolder(), Times.Once);
        }

        [Fact]
        public void Execute_WhenAssetDirectoryInGothicNotExist_ShouldNotMoveAssetFolderToBackup()
        {
            const string assetSourceDirectoryPath = "SourcePath";
            const string assetDestinationDirectoryPath = "DestinationPath";

            _profileMock.SetupGet(x => x.GmcFolder.DoesBackupFolderExist).Returns(false);
            _fileSystemMock.Setup(x => x.Path.Combine(GothicWorkDataFolderPath, It.IsAny<string>()))
                .Returns(assetSourceDirectoryPath);
            _fileSystemMock.Setup(x => x.Path.Combine(GmcBackupWorkDataFolderPath, It.IsAny<string>()))
                .Returns(assetDestinationDirectoryPath);
            _fileSystemMock.Setup(x => x.Directory.Exists(assetSourceDirectoryPath)).Returns(false);

            Act();

            _fileSystemMock.Verify(x => x.Directory.Move(assetSourceDirectoryPath, assetDestinationDirectoryPath),
                Times.Never);
        }

        [Fact]
        public void Execute_WhenAssetDirectoryInGothicExists_ShouldMoveAllAssetFoldersToBackup()
        {
            const string assetSourceDirectoryPath = "SourcePath";
            const string assetDestinationDirectoryPath = "DestinationPath";

            _profileMock.SetupGet(x => x.GmcFolder.DoesBackupFolderExist).Returns(false);
            _fileSystemMock.Setup(x => x.Path.Combine(GothicWorkDataFolderPath, It.IsAny<string>()))
                .Returns(assetSourceDirectoryPath);
            _fileSystemMock.Setup(x => x.Path.Combine(GmcBackupWorkDataFolderPath, It.IsAny<string>()))
                .Returns(assetDestinationDirectoryPath);
            _fileSystemMock.Setup(x => x.Directory.Exists(assetSourceDirectoryPath)).Returns(true);

            Act();

            var numberOfAssets = Enum.GetNames(typeof(AssetPresetType)).Length;
            _fileSystemMock.Verify(x => x.Directory.Move(assetSourceDirectoryPath, assetDestinationDirectoryPath),
                Times.Exactly(numberOfAssets));
        }


        [Fact]
        public void Execute_WhenModExtensionDirectoryNotExist_ShouldNotMoveToBackup()
        {
            const string assetSourceDirectoryPath = "SourcePath";
            const string assetDestinationDirectoryPath = "DestinationPath";

            _profileMock.SetupGet(x => x.GmcFolder.DoesBackupFolderExist).Returns(false);
            _fileSystemMock.Setup(x => x.Path.Combine(GothicWorkDataFolderPath, It.IsAny<string>()))
                .Returns(assetSourceDirectoryPath);
            _fileSystemMock.Setup(x => x.Path.Combine(GmcBackupWorkDataFolderPath, It.IsAny<string>()))
                .Returns(assetDestinationDirectoryPath);
            _fileSystemMock.Setup(x => x.Directory.Exists(assetSourceDirectoryPath)).Returns(false);
            _fileSystemMock.Setup(x => x.Directory.Exists(ModExtensionsFolderPath)).Returns(false);

            Act();

            _fileSystemMock.Verify(
                x => x.Directory.GetAllFilesInDirectory(ModExtensionsFolderPath, SearchOption.AllDirectories),
                Times.Never);
            _fileSystemMock.Verify(x => x.Directory.CreateIfNotExist(It.IsAny<string>()), Times.Never);
            _fileSystemMock.Verify(x => x.File.Copy(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }


        [Fact]
        public void Execute_WhenModExtensionDirectoryExists_ShouldMoveToBackup()
        {
            const string assetSourceDirectoryPath = "SourcePath";
            const string assetDestinationDirectoryPath = "DestinationPath";

            _profileMock.SetupGet(x => x.GmcFolder.DoesBackupFolderExist).Returns(false);
            _fileSystemMock.Setup(x => x.Path.Combine(GothicWorkDataFolderPath, It.IsAny<string>()))
                .Returns(assetSourceDirectoryPath);
            _fileSystemMock.Setup(x => x.Path.Combine(GmcBackupWorkDataFolderPath, It.IsAny<string>()))
                .Returns(assetDestinationDirectoryPath);
            _fileSystemMock.Setup(x => x.Directory.Exists(assetSourceDirectoryPath)).Returns(false);

            _fileSystemMock.Setup(x => x.Directory.Exists(ModExtensionsFolderPath)).Returns(true);
            _fileSystemMock
                .Setup(x => x.Directory.GetAllFilesInDirectory(ModExtensionsFolderPath, SearchOption.AllDirectories))
                .Returns(new List<string> { "File1", "File2" });
            _fileSystemMock
                .Setup(x => x.Path.GetRelativePath(ModExtensionsFolderPath, It.IsAny<string>()))
                .Returns("RelativePath");
            _fileSystemMock
                .Setup(x => x.Path.Combine(GothicBasePath, "RelativePath"))
                .Returns("ExtensionFileGothicPath");
            _fileSystemMock
                .Setup(x => x.Path.Combine(GmcBackupFolderPath, "RelativePath"))
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

            _fileSystemMock.Verify(
                x => x.Directory.GetAllFilesInDirectory(ModExtensionsFolderPath, SearchOption.AllDirectories),
                Times.Once);
            _fileSystemMock.Verify(x => x.Directory.CreateIfNotExist(It.IsAny<string>()), Times.Exactly(2));
            _fileSystemMock.Verify(x => x.File.Copy(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
        }
    }
}