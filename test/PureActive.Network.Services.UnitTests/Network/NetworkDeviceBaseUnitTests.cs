using FluentAssertions;
using PureActive.Network.Abstractions.CommonNetworkServices;
using PureActive.Network.Abstractions.NetworkDevice;
using PureActive.Network.Abstractions.Types;
using PureActive.Network.Devices.Network;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Network.Services.UnitTests.Network
{
    [Trait("Category", "Unit")]
    public class NetworkDeviceBaseUnitTests : TestBaseLoggable<NetworkDeviceBaseUnitTests>
    {
        public NetworkDeviceBaseUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _commonNetworkServices =
                CommonNetworkServices.CreateInstance(TestLoggerFactory, "NetworkDeviceBaseUnitTests");
        }

        private readonly ICommonNetworkServices _commonNetworkServices;

        [Fact]
        public void NetworkDeviceBase_Constructor()
        {
            NetworkDeviceBase networkDeviceBase =
                new NetworkDeviceBase(_commonNetworkServices, DeviceType.LocalComputer);
            networkDeviceBase.Should().NotBeNull().And.Subject.Should().BeAssignableTo<INetworkDevice>();
        }
    }
}