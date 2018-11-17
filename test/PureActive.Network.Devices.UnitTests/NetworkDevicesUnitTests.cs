using PureActive.Network.Devices.UnitTests.PureObjectGraph;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Network.Devices.UnitTests
{
    [Trait("Category", "Unit")]
    public class NetworkDevicesTests : TestLoggerBase<NetworkDevicesTests>
    {
        public NetworkDevicesTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        [Fact]
        public void NetworkDevices_Create()
        {

        }
    }
}
