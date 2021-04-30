using System;
using System.Collections.Generic;
using FluentAssertions;
using GothicModComposer.Commands;
using GothicModComposer.Models.Profiles;
using GothicModComposer.Utils.IOHelpers.FileSystem;
using Moq;
using Xunit;

namespace GothicModComposer.UnitTests.Commands
{
    public class OverrideIniCommandTests : TestsFixture
    {
        private readonly Mock<IProfile> _profileMock;
        private readonly Mock<IFileSystemWithLogger> _fileSystemMock;

        public OverrideIniCommandTests()
        {
            _profileMock = ProfileMock;
            _fileSystemMock = FileSystemMock;
        }
        private void Act()
            => new OverrideIniCommand(_profileMock.Object, _fileSystemMock.Object).Execute();

        [Fact]
        public void Execute_WhenIniOverridesAreNotDefined_ShouldReturn()
        {
            _profileMock.SetupGet(x => x.IniOverrides).Returns(new List<string>());

            Act();

            _profileMock.Verify(x => x.GmcFolder.CreateGmcFolder(), Times.Never);
            _fileSystemMock.Verify(x => x.File.Exists(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void Execute_WhenIniOverridesAreDefined_ThrowsException()
        {
            _profileMock.SetupGet(x => x.IniOverrides).Returns(new List<string> {"Test1=x"});
            _fileSystemMock.Setup(x => x.File.Exists(GothicIniFilePath)).Returns(false);

            var result = Record.Exception(Act);

            result.Should().NotBeNull();
            result.Should().BeOfType<Exception>();
            result?.Message.Should().Be("Gothic.ini file was not found.");
        }
    }
}