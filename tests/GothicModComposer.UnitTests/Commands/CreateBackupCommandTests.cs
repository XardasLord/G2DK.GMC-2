using System;
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
        public void ExecuteCreateBackupCommand_WhenBackupFolderExists_Returns()
        {
            _profileMock.SetupGet(x => x.GmcFolder.DoesBackupFolderExist).Returns(true);

            Act();

            _profileMock.Verify(x => x.GmcFolder.CreateBackupWorkDataFolder(), Times.Never);
        }

        [Fact]
        public void ExecuteCreateBackupCommand_WhenBackupFolderNotExist_ShouldCreateBackupFolder()
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
        public void ExecuteCreateBackupCommand_WhenAssetDirectoryInGothicNotExist_ShouldNotMoveAssetFolderToBackup()
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

            _fileSystemMock.Verify(x => x.Directory.Move(assetSourceDirectoryPath, assetDestinationDirectoryPath), Times.Never);
        }

        [Fact]
        public void ExecuteCreateBackupCommand_WhenAssetDirectoryInGothicExists_ShouldMoveAssetFolderToBackup()
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
            _fileSystemMock.Setup(x => x.Directory.Exists(modExtensionsFolderPath)).Returns(false);

            Act();
            
            var numberOfAssets = Enum.GetNames(typeof(AssetPresetType)).Length;
            _fileSystemMock.Verify(x => x.Directory.Move(assetSourceDirectoryPath, assetDestinationDirectoryPath), Times.Exactly(numberOfAssets));
        }
    }
}