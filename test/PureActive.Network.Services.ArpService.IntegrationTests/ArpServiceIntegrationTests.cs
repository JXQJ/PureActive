using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using PureActive.Hosting.Abstractions.Types;
using PureActive.Hosting.CommonServices;
using PureActive.Network.Abstractions.ArpService;
using PureActive.Network.Abstractions.Extensions;
using PureActive.Network.Abstractions.PingService;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Network.Services.ArpService.IntegrationTests
{
    [Trait("Category", "Integration")]
    public class ArpServiceIntegrationTests : TestBaseLoggable<ArpServiceIntegrationTests>
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly IArpService _arpService;

        public ArpServiceIntegrationTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            var commonServices = CommonServices.CreateInstance(TestLoggerFactory, "ArpServiceIntegrationTests");
            _cancellationTokenSource = new CancellationTokenSource();
            IPingService pingService = new PingService.PingService(commonServices);
            _arpService = new ArpService(commonServices, pingService);
        }
    
        [Fact]
        public async Task ArpService_StartStopAsync()
        {
            Assert.Equal(ServiceHostStatus.Stopped ,_arpService.ServiceHostStatus);
            await _arpService.StartAsync(_cancellationTokenSource.Token);
            Assert.Equal(ServiceHostStatus.StartPending, _arpService.ServiceHostStatus);
            await _arpService.StopAsync(_cancellationTokenSource.Token);
            Assert.Equal(ServiceHostStatus.Stopped, _arpService.ServiceHostStatus);
        }

        [Fact]
        public async Task ArpService_GatewayPhysicalAddress()
        {
            // Start ArpService
            Assert.Equal(ServiceHostStatus.Stopped, _arpService.ServiceHostStatus);
            await _arpService.StartAsync(_cancellationTokenSource.Token);
            Assert.Equal(ServiceHostStatus.StartPending, _arpService.ServiceHostStatus);

            var gatewayIPAddressSubnet = IPAddressExtensions.GetDefaultGatewayAddressSubnet(Logger);

            var physicalAddress = _arpService.GetPhysicalAddress(gatewayIPAddressSubnet.IPAddress);
            physicalAddress.Should().NotBeNull();

            await _arpService.StopAsync(_cancellationTokenSource.Token);
        }

        [Fact]
        public async Task ArpService_BogusPhysicalAddress()
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