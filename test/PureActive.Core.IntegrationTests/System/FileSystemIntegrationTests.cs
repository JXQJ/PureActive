using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using FluentAssertions;
using Moq;
using PureActive.Core.Abstractions.System;
using PureActive.Core.Extensions;
using PureActive.Core.System;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;
using OperatingSystem = PureActive.Core.System.OperatingSystem;

namespace PureActive.Core.IntegrationTests.System
{
    [Trait("Category", "Integration")]
    public class FileSystemIntegrationTests : TestBaseLoggable<FileSystemIntegrationTests>
    {
        public FileSystemIntegrationTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _fileSystem = new FileSystem(typeof(FileSystemIntegrationTests));
        }

        private readonly IFileSystem _fileSystem;

        private const int CBufferSize = 4000;

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
                _fileSystem.FolderExists(specialFolderPath).Should().BeTrue();
            }
        }

        [Theory]
        [InlineData(Environment.SpecialFolder.MyDocuments, Environment.SpecialFolderOption.None)]
        public void FileSystem_SpecialFolder_Option(Environment.SpecialFolder specialFolder,
            Environment.SpecialFolderOption specialFolderOption)
        {
            var specialFolderPath = _fileSystem.GetSpecialFolderPath(specialFolder, specialFolderOption);
            specialFolderPath.Should().NotBeNull();

            if (!string.IsNullOrEmpty(specialFolderPath))
            {
                _fileSystem.FolderExists(specialFolderPath).Should().BeTrue();
            }
        }

        [Fact]
        public void FileSystem_ArpCommandPath_FileExists()
        {
            var arpCommandPath = _fileSystem.ArpCommandPath();
            _fileSystem.FileExists(arpCommandPath).Should().BeTrue();
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
        public void FileSystem_AssemblyFolder()
        {
            var assemblyFolder = _fileSystem.AssemblyFolder;
            assemblyFolder.Should().NotBeNullOrEmpty();
            assemblyFolder.Should().Contain("PureActive.Core.IntegrationTests");
        }

        [Fact]
        public void FileSystem_CreateDeleteFolder()
        {
            var tempFolderPath = _fileSystem.GetTempFolderPath() + Guid.NewGuid().ToStringNoDashes();

            _fileSystem.CreateFolder(tempFolderPath);
            _fileSystem.FolderExists(tempFolderPath).Should().BeTrue();

            _fileSystem.DeleteFolder(tempFolderPath);
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
        public void FileSystem_DataFolderPath()
        {
            var dataFolderPath = _fileSystem.DataFolderPath();
            dataFolderPath.Should().NotBeNullOrEmpty();
            dataFolderPath.Should().EndWith($"/{nameof(FileSystemIntegrationTests)}/Data/");
        }


        [Fact]
        public void FileSystem_DeleteFile_NoFound()
        {
            _fileSystem.Invoking(fs => fs.DeleteFile("ZZ:\\")).Should().Throw<IOException>();
        }

        [Fact]
        public void FileSystem_DeleteFile_Null()
        {
            _fileSystem.Invoking(fs => fs.DeleteFile(null)).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void FileSystem_GetCommonApplicationDataFolderPath()
        {
            var settingFolderPath = _fileSystem.GetCommonApplicationDataFolderPath();
            settingFolderPath.Should().NotBeNullOrEmpty();
            _fileSystem.FolderExists(settingFolderPath).Should().BeTrue();
        }

        [Fact]
        [ExcludeFromCodeCoverage]
        public void FileSystem_GetCommonApplicationDataFolderPath_Osx()
        {
            var operatingSystem = new OperatingSystem();
            Mock<IOperatingSystem> operatingSystemMock = new Mock<IOperatingSystem>();

            operatingSystemMock.Setup(osm => osm.IsWindows()).Returns(false);
            operatingSystemMock.Setup(osm => osm.IsOsx()).Returns(true);

            var fileSystem = new FileSystem(typeof(FileSystemIntegrationTests), operatingSystemMock.Object);

            var specialFolderPath = fileSystem.GetCommonApplicationDataFolderPath(Environment.SpecialFolderOption.None);

            specialFolderPath.Should().NotBeNull();

            if (operatingSystem.IsOsx())
                _fileSystem.FolderExists(specialFolderPath).Should().BeTrue();
        }


        [Fact]
        public void FileSystem_GetFileNameWithExtension()
        {
            var tempFileNameWithOutExtension = FileExtensions.GetRandomFileName("Test_", "");
            _fileSystem.GetFileNameWithoutExtension($"{tempFileNameWithOutExtension}.log").Should()
                .Be(tempFileNameWithOutExtension);
        }

        [Fact]
        public void FileSystem_GetFileNameWithExtension_Empty()
        {
            _fileSystem.GetFileNameWithoutExtension("").Should().BeEmpty();
        }

        [Fact]
        public void FileSystem_GetFileNameWithExtension_Null()
        {
            _fileSystem.GetFileNameWithoutExtension(null).Should().BeNull();
        }

        [Fact]
        public void FileSystem_GetFolderName_DriveLetter()
        {
            _fileSystem.GetFolderName("C:").Should().BeNull();
        }


        [Fact]
        public void FileSystem_GetFolderName_DriveLetterPath()
        {
            _fileSystem.GetFolderName("C:\\").Should().BeNull();
        }

        [Fact]
        public void FileSystem_GetFolderName_DriveLetterRootFolder()
        {
            _fileSystem.GetFolderName("C:\\Temp").Should().Be("C:\\");
        }

        [Fact]
        public void FileSystem_GetFolderName_DriveLetterRootFolderPath()
        {
            _fileSystem.GetFolderName("C:\\Temp\\").Should().Be("C:\\Temp");
        }

        [Fact]
        public void FileSystem_GetFolderName_Empty()
        {
            _fileSystem.GetFolderName("").Should().BeNull();
        }


        [Fact]
        public void FileSystem_GetFolderName_Null()
        {
            _fileSystem.GetFolderName(null).Should().BeNull();
        }

        [Fact]
        public void FileSystem_GetLocalApplicationDataFolderPath()
        {
            var settingFolderPath = _fileSystem.GetLocalApplicationDataFolderPath();
            settingFolderPath.Should().NotBeNullOrEmpty();
            _fileSystem.FolderExists(settingFolderPath).Should().BeTrue();
        }

        [Fact]
        public void FileSystem_GetLocalApplicationDataFolderPath_DoNotVerify()
        {
            var settingFolderPath =
                _fileSystem.GetLocalApplicationDataFolderPath(Environment.SpecialFolderOption.DoNotVerify);
            settingFolderPath.Should().NotBeNullOrEmpty();
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
        public void FileSystem_GetTempFolderPath()
        {
            var tempFolderPath = _fileSystem.GetTempFolderPath();
            tempFolderPath.Should().NotBeNullOrEmpty();
            _fileSystem.FolderExists(tempFolderPath).Should().BeTrue();
        }

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
        public void FileSystem_LogFolderPath()
        {
            var settingFolderPath = _fileSystem.LogFolderPath();
            settingFolderPath.Should().NotBeNullOrEmpty();
            settingFolderPath.Should().EndWith($"/{nameof(FileSystemIntegrationTests)}/Logs/");
        }

        [Fact]
        public void FileSystem_ProcessSpecialFolder_Private_Create()
        {
            var tempFolderName =
                $"{_fileSystem.GetFolderName(_fileSystem.GetTempFileName())}\\{Guid.NewGuid().ToStringNoDashes()}";
            _fileSystem.FolderExists(tempFolderName).Should().BeFalse();

            MethodInfo methodInfo = typeof(FileSystem).GetMethod("ProcessSpecialFolder",
                BindingFlags.NonPublic | BindingFlags.Instance);
            object[] parameters = {Environment.SpecialFolderOption.Create, tempFolderName};

            methodInfo.Invoke(_fileSystem, parameters).Should().Be(tempFolderName);
            _fileSystem.FolderExists(tempFolderName).Should().BeTrue();
            _fileSystem.DeleteFolder(tempFolderName);
            _fileSystem.FolderExists(tempFolderName).Should().BeFalse();
        }

        [Fact]
        public void FileSystem_ProcessSpecialFolder_Private_DoNotVerify()
        {
            var tempFolderName =
                $"{_fileSystem.GetFolderName(_fileSystem.GetTempFileName())}\\{Guid.NewGuid().ToStringNoDashes()}";
            _fileSystem.FolderExists(tempFolderName).Should().BeFalse();

            MethodInfo methodInfo = typeof(FileSystem).GetMethod("ProcessSpecialFolder",
                BindingFlags.NonPublic | BindingFlags.Instance);
            object[] parameters = {Environment.SpecialFolderOption.DoNotVerify, tempFolderName};

            methodInfo.Invoke(_fileSystem, parameters).Should().Be(tempFolderName);
            _fileSystem.FolderExists(tempFolderName).Should().BeFalse();
        }

        [Fact]
        public void FileSystem_ProcessSpecialFolder_Private_None_Exists()
        {
            var tempFolderName =
                $"{_fileSystem.GetFolderName(_fileSystem.GetTempFileName())}\\{Guid.NewGuid().ToStringNoDashes()}";
            _fileSystem.CreateFolder(tempFolderName);

            MethodInfo methodInfo = typeof(FileSystem).GetMethod("ProcessSpecialFolder",
                BindingFlags.NonPublic | BindingFlags.Instance);
            object[] parameters = {Environment.SpecialFolderOption.None, tempFolderName};

            methodInfo.Invoke(_fileSystem, parameters).Should().Be(tempFolderName);
            _fileSystem.FolderExists(tempFolderName).Should().BeTrue();
            _fileSystem.DeleteFolder(tempFolderName);
            _fileSystem.FolderExists(tempFolderName).Should().BeFalse();
        }

        [Fact]
        public void FileSystem_ProcessSpecialFolder_Private_None_Not_Exists()
        {
            var tempFolderName =
                $"{_fileSystem.GetFolderName(_fileSystem.GetTempFileName())}\\{Guid.NewGuid().ToStringNoDashes()}";
            _fileSystem.FolderExists(tempFolderName).Should().BeFalse();

            MethodInfo methodInfo = typeof(FileSystem).GetMethod("ProcessSpecialFolder",
                BindingFlags.NonPublic | BindingFlags.Instance);
            object[] parameters = {Environment.SpecialFolderOption.None, tempFolderName};

            methodInfo.Invoke(_fileSystem, parameters).Should().Be(string.Empty);
            _fileSystem.FolderExists(tempFolderName).Should().BeFalse();
        }

        [Fact]
        public async void FileSystem_ReadWriteContentsFileAsync()
        {
            var tempFileName = _fileSystem.GetTempFileName();
            var fileContents =
                @"The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.";

            await _fileSystem.WriteFileContentsAsync(tempFileName, fileContents);

            var fileContentsRead = _fileSystem.ReadFileContentsAsync(tempFileName).Result;

            fileContentsRead.Should().Be(fileContents);

            if (_fileSystem.FileExists(tempFileName))
                _fileSystem.DeleteFile(tempFileName);
        }


        [Fact]
        public void FileSystem_SettingsFolder()
        {
            var settingFolderPath = _fileSystem.SettingsFolder;
            settingFolderPath.Should().NotBeNullOrEmpty();
            settingFolderPath.Should().EndWith("/Settings/");
        }


        [Fact]
        public void FileSystem_TempFile_Create_Exists_Delete()
        {
            var tempFileName = _fileSystem.GetTempFileName();

            _fileSystem.FileExists(tempFileName).Should().BeTrue();
            _fileSystem.DeleteFile(tempFileName);
            _fileSystem.FileExists(tempFileName).Should().BeFalse();
        }


        [Fact]
        public void FileSystem_TempFolder_Create_Exists_Delete()
        {
            var tempFolderName =
                $"{_fileSystem.GetFolderName(_fileSystem.GetTempFileName())}\\{Guid.NewGuid().ToStringNoDashes()}";
            _fileSystem.CreateFolder(tempFolderName);
            _fileSystem.FolderExists(tempFolderName).Should().BeTrue();
            _fileSystem.DeleteFolder(tempFolderName);
            _fileSystem.FolderExists(tempFolderName).Should().BeFalse();
        }

        [Fact]
        public void FileSystem_TestLogFolderPath()
        {
            var settingFolderPath = _fileSystem.TestLogFolderPath();
            settingFolderPath.Should().NotBeNullOrEmpty();
            settingFolderPath.Should().EndWith($"/{nameof(FileSystemIntegrationTests)}/Logs/Test/");
        }
    }
}