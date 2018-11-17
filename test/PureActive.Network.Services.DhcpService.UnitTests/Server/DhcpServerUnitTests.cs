using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Network.Services.DhcpService.UnitTests.Server
{
    [Trait("Category", "Unit")]
    public class DhcpServerTests : TestLoggerBase<DhcpServerTests>
    {
        public DhcpServerTests(ITestOutputHelper output) : base(output)
        {
  
        }


        [Fact]
        public void DhcpServer_Create()
        {

        }
    }
}
