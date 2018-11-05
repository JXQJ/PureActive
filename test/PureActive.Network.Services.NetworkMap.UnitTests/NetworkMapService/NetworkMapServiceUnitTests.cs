using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PureActive.Network.Abstractions.CommonNetworkServices;
using PureActive.Network.Abstractions.NetworkMapService;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Network.Services.NetworkMap.UnitTests.NetworkMapService
{
    public class NetworkMapServiceUnitTests : LoggingUnitTestBase<NetworkMapServiceUnitTests>
    {
        private readonly ICommonNetworkServices _commonNetworkServices;

        public NetworkMapServiceUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _commonNetworkServices = CommonNetworkServices.CreateInstance(TestLoggerFactory, "NetworkMapServiceUnitTests");
        }


        private INetworkMapService CreateNetworkMapService()
        {
            // Common Network Services
            var dhcpService = new DhcpService.DhcpService(_commonNetworkServices);
            var networkMap = new Devices.Network.NetworkMap(_commonNetworkServices);

            return new NetworkMap.NetworkMapService(networkMap, dhcpService, null);
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
