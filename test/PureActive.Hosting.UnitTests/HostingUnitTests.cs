using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Hosting.UnitTests
{
    [Trait("Category", "Unit")]
    public class HostingTests : TestLoggerBase<HostingTests>
    {
        public HostingTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        [Fact]
        public void Hosting_Constructor()
        {

        }
    }
}
