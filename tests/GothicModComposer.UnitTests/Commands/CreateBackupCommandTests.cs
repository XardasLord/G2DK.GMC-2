using GothicModComposer.Commands;
using GothicModComposer.Models.Profiles;
using Moq;
using Xunit;

namespace GothicModComposer.UnitTests.Commands
{
    public class CreateBackupCommandTests
    {
        private readonly Mock<IProfile> _profileMock;

        public CreateBackupCommandTests() 
            => _profileMock = new Mock<IProfile>();

        private static void Act(IProfile profile) 
            => new CreateBackupCommand(profile).Execute();

        [Fact]
        public void ExecuteCreateBackupCommand_WhenBackupFolderExists_Returns()
        {
            _profileMock.SetupGet(x => x.GmcFolder.DoesBackupFolderExist).Returns(true);

            Act(_profileMock.Object);

            _profileMock.Verify(x => x.GmcFolder.CreateBackupWorkDataFolder(), Times.Never);
        }

        [Fact]
        public void ExecuteCreateBackupCommand_WhenBackupFolderNotExist_ShouldCreateBackupFolder()
        {
            _profileMock.SetupGet(x => x.GmcFolder.DoesBackupFolderExist).Returns(false);
            _profileMock.SetupGet(x => x.GmcFolder.BackupWorkDataFolderPath).Returns("C:/BackupWorkDataFolderPath");
            _profileMock.SetupGet(x => x.GothicFolder.WorkDataFolderPath).Returns("C:/WorkDataFolderPath");
            _profileMock.SetupGet(x => x.ModFolder.ExtensionsFolderPath).Returns("C:/ExtensionsFolderPath");

            Act(_profileMock.Object);

            _profileMock.Verify(x => x.GmcFolder.CreateBackupWorkDataFolder(), Times.Once);
        }
    }
}