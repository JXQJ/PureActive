using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using PureActive.Hosting.Abstractions.Types;
using PureActive.Hosting.CommonServices;
using PureActive.Network.Abstractions.ArpService;
using PureActive.Network.Abstractions.Extensions;
using PureActive.Network.Abstractions.PingService;
using PureActive.Network.Services.Services.ArpService;
using PureActive.Network.Services.Services.PingService;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Network.Services.UnitTests.Network
{
    public class ArpServiceUnitTests : LoggingUnitTestBase<ArpServiceUnitTests>
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly IArpService _arpService;

        public ArpServiceUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            var commonServices = CommonServices.CreateInstance(TestLoggerFactory, "ArpServiceUnitTests");
            _cancellationTokenSource = new CancellationTokenSource();
            IPingService pingService = new PingService(commonServices);
            _arpService = new ArpService(commonServices, pingService);
        }
    
        [Fact]
        public async Task TestArpServiceStartStopAsync()
        {
            Assert.Equal(ServiceHostStatus.Stopped ,_arpService.ServiceHostStatus);
            await _arpService.StartAsync(_cancellationTokenSource.Token);
            Assert.Equal(ServiceHostStatus.StartPending, _arpService.ServiceHostStatus);
            await _arpService.StopAsync(_cancellationTokenSource.Token);
            Assert.Equal(ServiceHostStatus.Stopped, _arpService.ServiceHostStatus);
        }

        [Fact]
        public async Task TestArpServiceGatewayPhysicalAddress()
        {
            // Start ArpService
            Assert.Equal(ServiceHostStatus.Stopped, _arpService.ServiceHostStatus);
            await _arpService.StartAsync(_cancellationTokenSource.Token);
            Assert.Equal(ServiceHostStatus.StartPending, _arpService.ServiceHostStatus);

            var gatewayIPAddressSubnet = IPAddressExtensions.GetDefaultGatewayAddressSubnet(Logger);

            var physicalAddress = _arpService.GetPhysicalAddress(gatewayIPAddressSubnet.IPAddress);

            await _arpService.StopAsync(_cancellationTokenSource.Token);
        }

        [Fact]
        public async Task TestArpServiceBogusPhysicalAddress()
        {
            // Start ArpService
            Assert.Equal(ServiceHostStatus.Stopped, _arpService.ServiceHostStatus);
            await _arpService.StartAsync(_cancellationTokenSource.Token);
            Assert.Equal(ServiceHostStatus.StartPending, _arpService.ServiceHostStatus);

            var ipAddress = IPAddress.Parse("203.0.113.1");

            var physicalAddress = _arpService.GetPhysicalAddress(ipAddress);

            Assert.Equal(PhysicalAddress.None, physicalAddress);

            await _arpService.StopAsync(_cancellationTokenSource.Token);
        }
    }
}