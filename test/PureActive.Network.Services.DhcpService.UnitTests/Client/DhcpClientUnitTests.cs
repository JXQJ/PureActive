using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Network.Services.DhcpService.UnitTests.Client
{
    [Trait("Category", "Unit")]
    public class DhcpClientUnitTests : LoggingUnitTestBase<DhcpClientUnitTests>
    {
        public DhcpClientUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        [Fact]
        public void DhcpClient_Create()
        {

        }
    }
}
