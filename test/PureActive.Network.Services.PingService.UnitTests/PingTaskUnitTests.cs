using System.Threading.Tasks;
using FluentAssertions;
using PureActive.Hosting.CommonServices;
using PureActive.Network.Abstractions.Extensions;
using PureActive.Network.Abstractions.PingService;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Network.Services.PingService.UnitTests
{
    [Trait("Category", "Unit")]
    public class PingTaskUnitTests : TestBaseLoggable<PingTaskUnitTests>
    {
        private readonly IPingService _pingService;

        public PingTaskUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            var commonServices = CommonServices.CreateInstance(TestLoggerFactory, "PingTaskUnitTests");
            _pingService = new PingService(commonServices);
        }

        [Fact]
        public async Task TestPingTask()
        {
            var ipAddress = IPAddressExtensions.GetDefaultLocalNetworkAddress(Logger);

            // Wait 5 seconds for a reply.
            int timeout = 500;
            var pingReply = await _pingService.PingIpAddressAsync(ipAddress, timeout);
            pingReply.Should().NotBeNull();
        }
    }
}