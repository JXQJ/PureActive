using System;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions.Internal;
using PureActive.Core.Abstractions.System;
using PureActive.Core.System;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Core.UnitTests.System
{
    [Trait("Category", "Unit")]
    public class FileSystemUnitTests : TestBaseLoggable<FileSystemUnitTests>
    {
        public FileSystemUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        private static readonly string AppFolderName = "FileSystemUnitTests";

        private static IConfigurationRoot FileSystemConfigurationRoot(string appFolderName)
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables()
                .AddInMemoryCollection(
                    new Dictionary<string, string>
                    {
                        ["AppSettings:AppFolderName"] = appFolderName
                    }
                )
                .Build();
        }

        [Fact]
        public void FileSystem_Constructor_AppFolderName()
        {
            var fileSystem = new FileSystem(AppFolderName);

            fileSystem.Should().NotBeNull().And.Subject.As<IFileSystem>().AppFolderName.Should().Be(AppFolderName);
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

            // ReSharper disable once ObjectCreationAsStatement
            Action act = () => new FileSystem(fileSystemConfigurationRoot);

            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("appFolderName");
        }

        [Fact]
        public void FileSystem_Constructor_Configuration_OperatingSystem_Null()
        {
            var fileSystemConfigurationRoot = FileSystemConfigurationRoot(AppFolderName);

            // ReSharper disable once ObjectCreationAsStatement
            Action act = () => new FileSystem(fileSystemConfigurationRoot, null);

            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("operatingSystem");
        }


        [Fact]
        public void FileSystem_Constructor_Default()
        {
            var fileSystem = new FileSystem();
            fileSystem.Should().NotBeNull().And.Subject.As<IFileSystem>().AppFolderName.Should().Be("PureActive/Core");
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
            // ReSharper disable once ObjectCreationAsStatement
            Action act = () => new FileSystem((Type) null);

            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("type");
        }

        [Fact]
        public void FileSystem_Constructor_Type_OperatingSystem_Null()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Action act = () => new FileSystem(typeof(FileSystemUnitTests), null);

            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("operatingSystem");
        }
    }
}