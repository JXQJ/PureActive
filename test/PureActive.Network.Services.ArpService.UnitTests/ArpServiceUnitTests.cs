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

namespace PureActive.Network.Services.ArpService.UnitTests
{
    [Trait("Category", "Unit")]
    public class ArpServiceUnitTests : TestBaseLoggable<ArpServiceUnitTests>
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly IArpService _arpService;

        public ArpServiceUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            var commonServices = CommonServices.CreateInstance(TestLoggerFactory, "ArpServiceUnitTests");
            _cancellationTokenSource = new CancellationTokenSource();
            IPingService pingService = new PingService.PingService(commonServices);
            _arpService = new ArpService(commonServices, pingService);
        }

        [Fact]
        public void ArpService_Constructor()
        {
            _arpService.Should().NotBeNull();
        }

        [Fact]
        public void ArpService_Count()
        {
            _arpService.Count.Should().Be(0);
        }
    }

}