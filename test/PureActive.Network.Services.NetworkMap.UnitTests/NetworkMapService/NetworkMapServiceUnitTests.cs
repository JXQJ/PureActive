using FluentAssertions;
using PureActive.Network.Abstractions.CommonNetworkServices;
using PureActive.Network.Abstractions.Network;
using PureActive.Network.Abstractions.NetworkMapService;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Network.Services.NetworkMap.UnitTests.NetworkMapService
{
    [Trait("Category", "Unit")]
    public class NetworkMapServiceUnitTests : TestBaseLoggable<NetworkMapServiceUnitTests>
    {
        public NetworkMapServiceUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _commonNetworkServices =
                CommonNetworkServices.CreateInstance(TestLoggerFactory, "NetworkMapServiceUnitTests");
        }

        private readonly ICommonNetworkServices _commonNetworkServices;

        private INetworkMapService CreateNetworkMapService()
        {
            // Common Network Services
            var dhcpService = new DhcpService.DhcpService(_commonNetworkServices);
            var networkMap = new Devices.Network.NetworkMap(_commonNetworkServices);

            return new NetworkMap.NetworkMapService(networkMap, dhcpService);
        }

        [Fact]
        public void NetworkMapService_Constructor()
        {
            var networkMapService = CreateNetworkMapService();
            networkMapService.Should().NotBeNull();
            networkMapService.NetworkMap.Should().NotBeNull().And.Subject.Should().BeAssignableTo<INetworkMap>();
        }
    }
}