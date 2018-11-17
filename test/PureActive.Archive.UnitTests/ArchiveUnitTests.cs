using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Archive.UnitTests
{
    [Trait("Category", "Unit")]
    public class ArchiveTests : TestLoggerBase<ArchiveTests>
    {
        public ArchiveTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        [Fact]
        public void Archive_Constructor()
        {

        }
    }
}
