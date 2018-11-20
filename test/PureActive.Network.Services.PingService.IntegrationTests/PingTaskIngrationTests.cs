using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using PureActive.Hosting.CommonServices;
using PureActive.Network.Abstractions.Extensions;
using PureActive.Network.Abstractions.PingService;
using PureActive.Network.Abstractions.PingService.Events;
using PureActive.Network.Abstractions.Types;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Network.Services.PingService.IntegrationTests
{
    [Trait("Category", "Integration")]
    public class PingTaskIntegrationTests : TestBaseLoggable<PingTaskIntegrationTests>
    {
        public PingTaskIntegrationTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            var commonServices = CommonServices.CreateInstance(TestLoggerFactory, "PingTaskIntegrationTests");
            _cancellationTokenSource = new CancellationTokenSource();
            _pingService = new PingService(commonServices);
        }

        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly IPingService _pingService;
        private const int DefaultNetworkTimeout = 250;
        private const int DefaultPingCalls = 5;

        private void PingReplyEventHandler(object sender, PingReplyEventArgs args)
        {
            sender.Should().BeAssignableTo<IPingService>();

            if (args != null)
            {
                TestOutputHelper.WriteLine(
                    $"Job: {args.PingJob.JobGuid}, TaskId: {args.PingJob.TaskId}, IPAddressSubnet: {args.PingJob.IPAddressSubnet}, Status: {args.PingReply.Status}");
            }
        }

        [Fact]
        public async Task PingService_PingNetworkEventWithLogging()
        {
            var ipAddressSubnet = new IPAddressSubnet(IPAddressExtensions.GetDefaultLocalNetworkAddress(Logger),
                IPAddressExtensions.SubnetClassC);

            _pingService.OnPingReply += PingReplyEventHandler;
            await _pingService.PingNetworkAsync(ipAddressSubnet, _cancellationTokenSource.Token, DefaultNetworkTimeout,
                DefaultPingCalls, false);
        }

        [Fact]
        public async Task PingService_PingNetworkWithLogging()
        {
            var ipAddressSubnet = new IPAddressSubnet(IPAddressExtensions.GetDefaultLocalNetworkAddress(Logger),
                IPAddressExtensions.SubnetClassC);

            await _pingService.PingNetworkAsync(ipAddressSubnet, _cancellationTokenSource.Token, DefaultNetworkTimeout,
                DefaultPingCalls, false);
        }
    }
}