using FluentAssertions;
using PureActive.Core.Extensions;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Core.UnitTests.Extensions
{
    [Trait("Category", "Unit")]
    public class FileExtensionsUnitTests : TestBaseLoggable<FileExtensionsUnitTests>
    {
        public FileExtensionsUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Theory]
        [InlineData("prefix", ".ext")]
        public void FileExtensions_PrefixExt(string prefix, string ext)
        {
            var randomFile = FileExtensions.GetRandomFileName(prefix, ext);

            TestOutputHelper.WriteLine(randomFile);
            randomFile.Should().EndWith(ext);
            randomFile.Should().StartWith(prefix);

            var randomFile2 = FileExtensions.GetRandomFileName(prefix, ext);
            randomFile2.Should().NotBe(randomFile);
            TestOutputHelper.WriteLine(randomFile2);
        }


        [Fact]
        public void FileExtensions_NullExt()
        {
            string prefix = "prefix";

            var randomFile = FileExtensions.GetRandomFileName(prefix, null);
            TestOutputHelper.WriteLine(randomFile);

            randomFile.Should().StartWith(prefix);

            var randomFile2 = FileExtensions.GetRandomFileName(prefix, null);
            TestOutputHelper.WriteLine(randomFile2);

            randomFile2.Should().NotBe(randomFile);
        }


        [Fact]
        public void FileExtensions_NullPrefix()
        {
            string ext = ".ext";

            var randomFile = FileExtensions.GetRandomFileName(null, ext);

            TestOutputHelper.WriteLine(randomFile);
            randomFile.Should().EndWith(ext);

            var randomFile2 = FileExtensions.GetRandomFileName(null, ext);
            TestOutputHelper.WriteLine(randomFile2);
            randomFile2.Should().NotBe(randomFile);
        }
    }
}