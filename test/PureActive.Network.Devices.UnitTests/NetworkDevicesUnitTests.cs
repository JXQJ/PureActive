using PureActive.Network.Devices.UnitTests.PureObjectGraph;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Network.Devices.UnitTests
{
    [Trait("Category", "Unit")]
    public class NetworkDevicesUnitTests : LoggingUnitTestBase<NetworkDevicesUnitTests>
    {
        public NetworkDevicesUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        [Fact]
        public void NetworkDevices_Create()
        {

        }
    }
}
