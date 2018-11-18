using System;
using System.IO;
using FluentAssertions;
using Moq;
using PureActive.Core.Abstractions.System;
using PureActive.Core.Extensions;
using PureActive.Core.System;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Core.IntegrationTests.System
{
    [Trait("Category", "Integration")]
    public class FileSystemIntegrationTests : TestBaseLoggable<FileSystemIntegrationTests>
    {
        private readonly IFileSystem _fileSystem;

        public FileSystemIntegrationTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _fileSystem = new FileSystem(typeof(FileSystemIntegrationTests));
        }

        [Fact]
        public void FileSystem_CreateNewTempFile()
        {
            Stream tempFile;

            using (tempFile = _fileSystem.CreateNewTempFile())
            {
                tempFile.CanWrite.Should().BeTrue();
                tempFile.WriteByte(10);
                tempFile.Length.Should().Be(1);
            }

            tempFile.CanWrite.Should().BeFalse();
            tempFile.CanRead.Should().BeFalse();
        }

        [Fact]
        public void FileSystem_GetTempFolderPath()
        {
            var tempFolderPath = _fileSystem.GetTempFolderPath();
            tempFolderPath.Should().NotBeNullOrEmpty();
            Directory.Exists(tempFolderPath).Should().BeTrue();

        }

        private const int CBufferSize = 4000;

        [Fact]
        public void FileSystem_GetTempFolderPath_Writable()
        {
            var tempFolderPath = _fileSystem.GetTempFolderPath();

            Stream tempFile;

            using (tempFile = new FileStream
            (
                $"{tempFolderPath}{FileExtensions.GetRandomFileName(null, ".tmp")}",
                FileMode.OpenOrCreate,
                FileAccess.ReadWrite,
                FileShare.ReadWrite,
                CBufferSize,
                FileOptions.DeleteOnClose
            ))
            {
                tempFile.CanWrite.Should().BeTrue();
                tempFile.WriteByte(10);
                tempFile.Length.Should().Be(1);
            }

            tempFile.CanWrite.Should().BeFalse();
            tempFile.CanRead.Should().BeFalse();
        }

        [Fact]
        public void FileSystem_GetTempFileName_Writable()
        {
            Stream tempFile;

            using (tempFile = new FileStream
            (
                _fileSystem.GetTempFileName(),
                FileMode.OpenOrCreate,
                FileAccess.ReadWrite,
                FileShare.ReadWrite,
                CBufferSize,
                FileOptions.DeleteOnClose
            ))
            {
                tempFile.CanWrite.Should().BeTrue();
                tempFile.WriteByte(10);
                tempFile.Length.Should().Be(1);
            }

            tempFile.CanWrite.Should().BeFalse();
            tempFile.CanRead.Should().BeFalse();
        }

        [Fact]
        public void FileSystem_ArpCommandPath_FileExists()
        {
            var arpCommandPath = _fileSystem.ArpCommandPath();
            File.Exists(arpCommandPath).Should().BeTrue();
        }

        [Fact]
        public void FileSystem_ArpCommandPath_FileExists_OSX()
        {
            Mock<IOperatingSystem> operatingSystemMock = new Mock<IOperatingSystem>();

            operatingSystemMock.Setup(osm => osm.IsWindows()).Returns(false);
            operatingSystemMock.Setup(osm => osm.IsOsx()).Returns(true);

            var fileSystem = new FileSystem(typeof(FileSystemIntegrationTests), operatingSystemMock.Object);

            fileSystem.ArpCommandPath().Should().Be("/usr/sbin/arp");
        }

        [Fact]
        public async void FileSystem_ReadWriteContentsFileAsync()
        {
            var tempFileName = _fileSystem.GetTempFileName();
            var fileContents = @"The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.";

            await _fileSystem.WriteFileContentsAsync(tempFileName, fileContents);

            var fileContentsRead = _fileSystem.ReadFileContentsAsync(tempFileName).Result;

            fileContentsRead.Should().Be(fileContents);

            if (File.Exists(tempFileName))
                File.Delete(tempFileName);
        }

        [Fact]
        public void FileSystem_CreateDeleteFolder()
        {
            var tempFolderPath = _fileSystem.GetTempFolderPath() + "FileSystemIntegrationTests";
            
            if (Directory.Exists(tempFolderPath))
                _fileSystem.DeleteFolder(tempFolderPath);

            _fileSystem.CreateFolder(tempFolderPath);
            Directory.Exists(tempFolderPath).Should().BeTrue();

            _fileSystem.DeleteFolder(tempFolderPath);
        }

        [Theory]
        [InlineData(Environment.SpecialFolder.Desktop)]
        [InlineData(Environment.SpecialFolder.Programs)]
        [InlineData(Environment.SpecialFolder.MyDocuments)]
//        [InlineData(Environment.SpecialFolder.Personal)]
        [InlineData(Environment.SpecialFolder.Favorites)]
        [InlineData(Environment.SpecialFolder.Startup)]
        [InlineData(Environment.SpecialFolder.Recent)]
        [InlineData(Environment.SpecialFolder.SendTo)]
        [InlineData(Environment.SpecialFolder.StartMenu)]
        [InlineData(Environment.SpecialFolder.MyMusic)]
        [InlineData(Environment.SpecialFolder.MyVideos)]
        [InlineData(Environment.SpecialFolder.DesktopDirectory)]
        [InlineData(Environment.SpecialFolder.MyComputer)]
        [InlineData(Environment.SpecialFolder.NetworkShortcuts)]
        [InlineData(Environment.SpecialFolder.Fonts)]
        [InlineData(Environment.SpecialFolder.Templates)]
        [InlineData(Environment.SpecialFolder.CommonStartMenu)]
        [InlineData(Environment.SpecialFolder.CommonPrograms)]
        [InlineData(Environment.SpecialFolder.CommonStartup)]
        [InlineData(Environment.SpecialFolder.CommonDesktopDirectory)]
        [InlineData(Environment.SpecialFolder.ApplicationData)]
        [InlineData(Environment.SpecialFolder.PrinterShortcuts)]
        [InlineData(Environment.SpecialFolder.LocalApplicationData)]
        [InlineData(Environment.SpecialFolder.InternetCache)]
        [InlineData(Environment.SpecialFolder.Cookies)]
        [InlineData(Environment.SpecialFolder.History)]
        [InlineData(Environment.SpecialFolder.CommonApplicationData)]
        [InlineData(Environment.SpecialFolder.Windows)]
        [InlineData(Environment.SpecialFolder.System)]
        [InlineData(Environment.SpecialFolder.ProgramFiles)]
        [InlineData(Environment.SpecialFolder.MyPictures)]
        [InlineData(Environment.SpecialFolder.UserProfile)]
        [InlineData(Environment.SpecialFolder.SystemX86)]
        [InlineData(Environment.SpecialFolder.ProgramFilesX86)]
        [InlineData(Environment.SpecialFolder.CommonProgramFiles)]
        [InlineData(Environment.SpecialFolder.CommonProgramFilesX86)]
        [InlineData(Environment.SpecialFolder.CommonTemplates)]
        [InlineData(Environment.SpecialFolder.CommonDocuments)]
        [InlineData(Environment.SpecialFolder.CommonAdminTools)]
        [InlineData(Environment.SpecialFolder.AdminTools)]
        [InlineData(Environment.SpecialFolder.CommonMusic)]
        [InlineData(Environment.SpecialFolder.CommonPictures)]
        [InlineData(Environment.SpecialFolder.CommonVideos)]
        [InlineData(Environment.SpecialFolder.Resources)]
        [InlineData(Environment.SpecialFolder.LocalizedResources)]
        [InlineData(Environment.SpecialFolder.CommonOemLinks)]
        [InlineData(Environment.SpecialFolder.CDBurning)]
        public void FileSystem_SpecialFolder(Environment.SpecialFolder specialFolder)
        {
            var specialFolderPath = _fileSystem.GetSpecialFolderPath(specialFolder);
            specialFolderPath.Should().NotBeNull();

            if (!string.IsNullOrEmpty(specialFolderPath))
            {
                Directory.Exists(specialFolderPath).Should().BeTrue();
            }
        }

        [Theory]
        [InlineData(Environment.SpecialFolder.MyDocuments, Environment.SpecialFolderOption.None)]
        public void FileSystem_SpecialFolder_Option(Environment.SpecialFolder specialFolder, Environment.SpecialFolderOption specialFolderOption)
        {
            var specialFolderPath = _fileSystem.GetSpecialFolderPath(specialFolder, specialFolderOption);
            specialFolderPath.Should().NotBeNull();

            if (!string.IsNullOrEmpty(specialFolderPath))
            {
                Directory.Exists(specialFolderPath).Should().BeTrue();
            }
        }

        [Fact]
        public void FileSystem_GetCommonApplicationDataFolderPath()
        {
            var specialFolderPath = _fileSystem.GetCommonApplicationDataFolderPath(Environment.SpecialFolderOption.None);

            specialFolderPath.Should().NotBeNull();

            if (!string.IsNullOrEmpty(specialFolderPath))
            {
                Directory.Exists(specialFolderPath).Should().BeTrue();
            }
        }

        [Fact]
        public void FileSystem_GetCommonApplicationDataFolderPath_Osx()
        {
            Mock<IOperatingSystem> operatingSystemMock = new Mock<IOperatingSystem>();

            operatingSystemMock.Setup(osm => osm.IsWindows()).Returns(false);
            operatingSystemMock.Setup(osm => osm.IsOsx()).Returns(true);

            var fileSystem = new FileSystem(typeof(FileSystemIntegrationTests), operatingSystemMock.Object);

            var specialFolderPath = fileSystem.GetCommonApplicationDataFolderPath(Environment.SpecialFolderOption.None);

            specialFolderPath.Should().NotBeNull();

            if (!string.IsNullOrEmpty(specialFolderPath))
            {
                Directory.Exists(specialFolderPath).Should().BeTrue();
            }
        }
    }
}