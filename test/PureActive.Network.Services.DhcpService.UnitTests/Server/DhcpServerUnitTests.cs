using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Network.Services.DhcpService.UnitTests.Server
{
    public class DhcpServerUnitTests : LoggingUnitTestBase<DhcpServerUnitTests>
    {
        public DhcpServerUnitTests(ITestOutputHelper output) : base(output)
        {
  
        }


        [Fact]
        public void DhcpServer_Create()
        {

        }
    }
}
