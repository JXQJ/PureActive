using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Network.Devices.UnitTests
{
    [Trait("Category", "Unit")]
    public class NetworkDevicesUnitTests : TestBaseLoggable<NetworkDevicesUnitTests>
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
