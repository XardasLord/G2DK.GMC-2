using System;
using GothicModComposer.Models.Profiles;
using GothicModComposer.Utils.IOHelpers.FileSystem;
using Moq;

namespace GothicModComposer.UnitTests.Commands
{
    public abstract class TestsFixture : IDisposable
    {
        public readonly Mock<IProfile> ProfileMock;
        public readonly Mock<IFileSystemWithLogger> FileSystemMock;

        public const string GothicBasePath = "C:/BasePath";
        public const string GothicWorkDataFolderPath = "C:/WorkDataFolderPath";
        public const string GmcBackupFolderPath = "C:/BackupFolderPath";
        public const string GmcBackupWorkDataFolderPath = "C:/BackupWorkDataFolderPath";
        public const string ModExtensionsFolderPath = "C:/ExtensionsFolderPath";

        protected TestsFixture()
        {
            // Do "global" initialization here; Called before every test method.
            ProfileMock = new Mock<IProfile>();
            FileSystemMock = new Mock<IFileSystemWithLogger>();
         
            MockGetPropertiesInProfile();
        }

        private void MockGetPropertiesInProfile()
        {
            ProfileMock.SetupGet(x => x.GothicFolder.BasePath).Returns(GothicBasePath);
            ProfileMock.SetupGet(x => x.GothicFolder.WorkDataFolderPath).Returns(GothicWorkDataFolderPath);
            ProfileMock.SetupGet(x => x.GmcFolder.BackupFolderPath).Returns(GmcBackupFolderPath);
            ProfileMock.SetupGet(x => x.GmcFolder.BackupWorkDataFolderPath).Returns(GmcBackupWorkDataFolderPath);
            ProfileMock.SetupGet(x => x.ModFolder.ExtensionsFolderPath).Returns(ModExtensionsFolderPath);
        }

        public void Dispose()
        {
            // Do "global" teardown here; Called after every test method.
        }
    }
}
