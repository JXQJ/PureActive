using FluentAssertions;
using PureActive.Network.Abstractions.CommonNetworkServices;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Network.Services.IntegrationTests.Network
{
    [Trait("Category", "Integration")]
    public class NetworkDeviceBaseIntegrationTests : TestBaseLoggable<NetworkDeviceBaseIntegrationTests>
    {
        public NetworkDeviceBaseIntegrationTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _commonNetworkServices =
                CommonNetworkServices.CreateInstance(TestLoggerFactory, "NetworkDeviceBaseIntegrationTests");
        }

        private readonly ICommonNetworkServices _commonNetworkServices;

        [Fact]
        public void NetworkDeviceBase_Constructor()
        {
            _commonNetworkServices.Should().NotBeNull();
        }
    }
}