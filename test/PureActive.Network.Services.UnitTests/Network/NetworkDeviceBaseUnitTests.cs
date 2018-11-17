using PureActive.Network.Abstractions.CommonNetworkServices;
using PureActive.Network.Abstractions.Extensions;
using PureActive.Network.Abstractions.Types;
using PureActive.Network.Devices.Network;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Network.Services.UnitTests.Network
{
    [Trait("Category", "Unit")]
    public class NetworkDeviceBaseTests : TestLoggerBase<NetworkDeviceBaseTests>
    {
        private readonly ICommonNetworkServices _commonNetworkServices;
        private readonly IPAddressSubnet _gatewayIPAddressSubnet;

        public NetworkDeviceBaseTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _commonNetworkServices = CommonNetworkServices.CreateInstance(TestLoggerFactory, "NetworkDeviceBaseTests");
            _gatewayIPAddressSubnet = IPAddressExtensions.GetDefaultGatewayAddressSubnet(Logger);
        }

        [Fact]
        public void TestGetNetworkAdapters()
        {
            NetworkDeviceBase networkDeviceBase = new NetworkDeviceBase(_commonNetworkServices, DeviceType.LocalComputer);
        }
    }
}