using FluentAssertions;
using PureActive.Network.Abstractions.CommonNetworkServices;
using PureActive.Network.Abstractions.NetworkDevice;
using PureActive.Network.Abstractions.Types;
using PureActive.Network.Devices.Network;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Network.Services.IntegrationTests.Network
{
    [Trait("Category", "Integration")]
    public class NetworkDeviceBaseIntegrationTests : TestBaseLoggable<NetworkDeviceBaseIntegrationTests>
    {
        private readonly ICommonNetworkServices _commonNetworkServices;

        public NetworkDeviceBaseIntegrationTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _commonNetworkServices = CommonNetworkServices.CreateInstance(TestLoggerFactory, "NetworkDeviceBaseIntegrationTests");
        }
    }
}