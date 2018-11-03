using PureActive.Network.Abstractions.CommonNetworkServices;
using PureActive.Network.Abstractions.Extensions;
using PureActive.Network.Abstractions.Types;
using PureActive.Network.Devices.Network;
using PureActive.Network.Services.Services;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Network.Services.UnitTests.Network
{
    public class NetworkDeviceBaseUnitTests : LoggingUnitTestBase<NetworkDeviceBaseUnitTests>
    {
        private readonly ICommonNetworkServices _commonNetworkServices;
        private readonly IPAddressSubnet _gatewayIPAddressSubnet;

        public NetworkDeviceBaseUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _commonNetworkServices = CommonNetworkServices.CreateInstance(TestLoggerFactory, "NetworkDeviceBaseUnitTests");
            _gatewayIPAddressSubnet = IPAddressExtensions.GetDefaultGatewayAddressSubnet(Logger);
        }

        [Fact]
        public void TestGetNetworkAdapters()
        {
            NetworkDeviceBase networkDeviceBase = new NetworkDeviceBase(_commonNetworkServices, DeviceType.LocalComputer);
        }
    }
}