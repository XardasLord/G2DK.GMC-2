using System;
using System.Collections.Generic;
using FluentAssertions;
using GothicModComposer.Core.Commands;
using GothicModComposer.Core.Models.Configurations;
using GothicModComposer.Core.Models.Profiles;
using GothicModComposer.Core.Utils.IOHelpers.FileSystem;
using Moq;
using Xunit;

namespace GothicModComposer.UnitTests.Commands
{
    public class OverrideIniCommandTests : TestsFixture
    {
        private readonly Mock<IFileSystemWithLogger> _fileSystemMock;
        private readonly Mock<IProfile> _profileMock;

        public OverrideIniCommandTests()
        {
            _profileMock = ProfileMock;
            _fileSystemMock = FileSystemMock;
        }

        private void Act()
            => new OverrideIniCommand(_profileMock.Object, _fileSystemMock.Object).ExecuteAsync();

        [Fact]
        public void Execute_WhenIniOverridesAreNotDefined_ShouldReturn()
        {
            _profileMock.SetupGet(x => x.IniOverrides).Returns(new List<IniOverride>());
            _profileMock.SetupGet(x => x.IniOverridesSystemPack).Returns(new List<IniOverride>());

            Act();

            _profileMock.Verify(x => x.GmcFolder.CreateGmcFolder(), Times.Never);
            _fileSystemMock.Verify(x => x.File.Exists(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void Execute_WhenIniOverridesAreDefined_ThrowsException()
        {
            _profileMock.SetupGet(x => x.IniOverrides).Returns(new List<IniOverride>
            {
                new() { Key = "Test1", Value = "2" }
            });
            _profileMock.SetupGet(x => x.IniOverridesSystemPack).Returns(new List<IniOverride>());
            _fileSystemMock.Setup(x => x.File.Exists(GothicIniFilePath)).Returns(false);

            var result = Record.Exception(Act);

            result.Should().NotBeNull();
            result.Should().BeOfType<Exception>();
            result?.Message.Should().Be("Gothic.ini file was not found.");
        }
    }
}