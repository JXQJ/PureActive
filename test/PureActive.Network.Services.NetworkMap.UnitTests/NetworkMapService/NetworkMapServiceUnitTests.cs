using System.Threading;
using System.Threading.Tasks;
using PureActive.Network.Abstractions.CommonNetworkServices;
using PureActive.Network.Abstractions.NetworkMapService;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Network.Services.NetworkMap.UnitTests.NetworkMapService
{
    [Trait("Category", "Unit")]
    public class NetworkMapServiceTests : TestLoggerBase<NetworkMapServiceTests>
    {
        private readonly ICommonNetworkServices _commonNetworkServices;

        public NetworkMapServiceTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _commonNetworkServices = CommonNetworkServices.CreateInstance(TestLoggerFactory, "NetworkMapServiceTests");
        }


        private INetworkMapService CreateNetworkMapService()
        {
            // Common Network Services
            var dhcpService = new DhcpService.DhcpService(_commonNetworkServices);
            var networkMap = new Devices.Network.NetworkMap(_commonNetworkServices);

            return new NetworkMap.NetworkMapService(networkMap, dhcpService);
        }

        [Fact]
        public async Task ServiceCreationTest()
        {
            var networkMapService = CreateNetworkMapService();

            if (networkMapService != null)
            {
                var cancellationTokenSource = new CancellationTokenSource();

                await networkMapService.StartAsync(cancellationTokenSource.Token);
                await networkMapService.StopAsync(cancellationTokenSource.Token);
            }

            Assert.NotNull(networkMapService);
        }
    }
}
