using FluentAssertions;
using PureActive.Hosting.CommonServices;
using PureActive.Network.Abstractions.ArpService;
using PureActive.Network.Abstractions.PingService;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Network.Services.ArpService.UnitTests
{
    [Trait("Category", "Unit")]
    public class ArpServiceUnitTests : TestBaseLoggable<ArpServiceUnitTests>
    {
        public ArpServiceUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            var commonServices = CommonServices.CreateInstance(TestLoggerFactory, "ArpServiceUnitTests");
            IPingService pingService = new PingService.PingService(commonServices);
            _arpService = new ArpService(commonServices, pingService);
        }

        private readonly IArpService _arpService;

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