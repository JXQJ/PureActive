using System;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions.Internal;
using Moq;
using PureActive.Core.Abstractions.System;
using PureActive.Core.Extensions;
using PureActive.Core.System;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Core.UnitTests.System
{
    public class FileSystemUnitTests : LoggingUnitTestBase<FileSystemUnitTests>
    {
        private static readonly string AppFolderName = "FileSystemUnitTests";
        private readonly IFileSystem _fileSystem;

        public FileSystemUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _fileSystem = new FileSystem(typeof(FileSystemUnitTests));
        }

        private static IConfigurationRoot FileSystemConfigurationRoot(string appFolderName)
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables()
                .AddInMemoryCollection(
                    new Dictionary<string, string>()
                    {
                        ["AppSettings:AppFolderName"] = appFolderName,
                    }
                )
                .Build();
        }


        [Fact]
        public void FileSystem_Constructor_Configuration()
        {
            var fileSystem = new FileSystem(FileSystemConfigurationRoot(AppFolderName));
            fileSystem.Should().NotBeNull().And.Subject.As<IFileSystem>().AppFolderName.Should().Be(AppFolderName);
        }


        [Fact]
        public void FileSystem_Constructor_Configuration_Null()
        {
            var fileSystemConfigurationRoot = FileSystemConfigurationRoot(null);

            Action act = () => new FileSystem(fileSystemConfigurationRoot);

            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("appFolderName");
        }

        [Fact]
        public void FileSystem_Constructor_Configuration_OperatingSystem_Null()
        {
            var fileSystemConfigurationRoot = FileSystemConfigurationRoot(AppFolderName);

            Action act = () => new FileSystem(fileSystemConfigurationRoot, null);

            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("operatingSystem");
        }

        [Fact]
        public void FileSystem_Constructor_AppFolderName()
        {
            var fileSystem = new FileSystem(AppFolderName);

            fileSystem.Should().NotBeNull().And.Subject.As<IFileSystem>().AppFolderName.Should().Be(AppFolderName);
        }


        [Fact]
        public void FileSystem_Constructor_Type()
        {
            var fileSystem = new FileSystem(typeof(FileSystemUnitTests));

            fileSystem.Should().NotBeNull().And.Subject.As<IFileSystem>().AppFolderName.Should()
                .Be(TypeNameHelper.GetTypeDisplayName(typeof(FileSystemUnitTests)).Replace(".", "/"));
        }


        [Fact]
        public void FileSystem_Constructor_Type_Null()
        {
            Action act = () => new FileSystem((Type) null);

            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("type");
        }

        [Fact]
        public void FileSystem_Constructor_Type_OperatingSystem_Null()
        {
            Action act = () => new FileSystem(typeof(FileSystemUnitTests), null);

            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("operatingSystem");
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
            var tempFolderPath = _fileSystem.GetTempFolderPath();

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

            var fileSystem = new FileSystem(typeof(FileSystemUnitTests), operatingSystemMock.Object);

            fileSystem.ArpCommandPath().Should().Be("/usr/sbin/arp");
        }
    }
}