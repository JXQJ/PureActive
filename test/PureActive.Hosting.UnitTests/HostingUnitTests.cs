using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Hosting.UnitTests
{
    [Trait("Category", "Unit")]
    public class HostingUnitTests : LoggingUnitTestBase<HostingUnitTests>
    {
        public HostingUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        [Fact]
        public void Hosting_Constructor()
        {

        }
    }
}
