using System.Threading;
using System.Threading.Tasks;
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
    public class PingServiceIntegrationTests : TestBaseLoggable<PingServiceIntegrationTests>
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly IPingService _pingService;
        private const int DefaultNetworkTimeout = 250;
        private const int DefaultPingCalls = 5;

        public PingServiceIntegrationTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            var commonServices = CommonServices.CreateInstance(TestLoggerFactory, "PingServiceIntegrationTests");
            _cancellationTokenSource = new CancellationTokenSource();
            _pingService = new PingService(commonServices);
        }


        [Fact]
        public async Task PingService_PingNetworkEventWithLogging()
        {
            var ipAddressSubnet = new IPAddressSubnet(IPAddressExtensions.GetDefaultLocalNetworkAddress(Logger), IPAddressExtensions.SubnetClassC);

            _pingService.OnPingReply += PingReplyEventHandler;
            await _pingService.PingNetworkAsync(ipAddressSubnet, _cancellationTokenSource.Token, DefaultNetworkTimeout, DefaultPingCalls, false);
        }

        private void PingReplyEventHandler(object sender, PingReplyEventArgs args)
        {
            if (args != null)
            {
                TestOutputHelper.WriteLine($"Job: {args.PingJob.JobGuid}, TaskId: {args.PingJob.TaskId}, IPAddressSubnet: {args.PingJob.IPAddressSubnet}, Status: {args.PingReply.Status}");
            }
        }
    }
}