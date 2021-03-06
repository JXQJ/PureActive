using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using PureActive.Hosting.Abstractions.System;
using PureActive.Hosting.CommonServices;
using PureActive.Serilog.Sink.Xunit.Extensions;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Serilog.Sinks.TestCorrelator;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Network.Services.PingService.IntegrationTests
{
    [Trait("Category", "Integration")]
    public class PingServiceRxIntegrationTests : TestBaseLoggable<PingServiceRxIntegrationTests>
    {
        public PingServiceRxIntegrationTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _commonServices = CommonServices.CreateInstance(TestLoggerFactory, "PingServiceRxIntegrationTests");

            _pingServiceRx = new PingServiceRx(_commonServices);
        }

        private readonly PingServiceRx _pingServiceRx;
        private readonly ICommonServices _commonServices;

        [Fact]
        public void PingServiceRx_Constructor()
        {
            _pingServiceRx.Should().NotBeNull().And.Subject.Should().BeOfType<PingServiceRx>();
        }

        [Fact]
        public async Task PingServiceRx_PingDefaultGateway_TestConsole()
        {
            var ipAddressDefaultGateway = _commonServices.NetworkingSystem.GetDefaultGatewayAddress();
            ipAddressDefaultGateway.Should().NotBeNull().And.Subject.Should().NotBe(IPAddress.None);

            using (TestCorrelator.CreateContext())
            {
                var observable = _pingServiceRx.PingRequestAsync(ipAddressDefaultGateway);
                observable.Should().NotBeNull().And.Subject.Should().BeAssignableTo<IObservable<PingReply>>();

                using (var subscription =
                    observable.SubscribeTestConsole(TestOutputHelper, "PingServiceRxIntegrationTests"))
                {
                    subscription.Should().NotBeNull().And.Subject.Should().BeAssignableTo<IDisposable>();

                    var pingReply = await observable.FirstAsync();
                    pingReply.Should().NotBeNull().And.Subject.Should().BeOfType<PingReply>()
                        .And.Subject.As<PingReply>().Status.Should()
                        .Match<IPStatus>(ips => ips == IPStatus.Success || ips == IPStatus.TimedOut);

                    // Give up time for Logging to Propagate
                    await Task.Delay(1000);
                }
            }
        }

        [Fact]
        public async Task PingServiceRx_PingDefaultGateway_TestLogger()
        {
            var ipAddressDefaultGateway = _commonServices.NetworkingSystem.GetDefaultGatewayAddress();
            ipAddressDefaultGateway.Should().NotBeNull().And.Subject.Should().NotBe(IPAddress.None);

            using (TestCorrelator.CreateContext())
            {
                var observable = _pingServiceRx.PingRequestAsync(ipAddressDefaultGateway);
                observable.Should().NotBeNull().And.Subject.Should().BeAssignableTo<IObservable<PingReply>>();

                using (var subscription =
                    observable.SubscribeTestLogger(Logger, "PingServiceRxIntegrationTests"))
                {
                    subscription.Should().NotBeNull().And.Subject.Should().BeAssignableTo<IDisposable>();

                    var pingReply = await observable.FirstAsync();
                    pingReply.Should().NotBeNull().And.Subject.Should().BeOfType<PingReply>()
                        .And.Subject.As<PingReply>().Status.Should()
                        .Match<IPStatus>(ips => ips == IPStatus.Success || ips == IPStatus.TimedOut);

                    // Give up time for Logging to Propagate
                    await Task.Delay(1000);

                    var logEventsList = TestCorrelator.GetLogEventsFromCurrentContext().ToList();

                    logEventsList.Should().HaveCount(2);
                    logEventsList.First().MessageTemplate.IsLogEventOnNext().Should().BeTrue();
                    logEventsList.Last().MessageTemplate.IsLogEventOnCompleted().Should().BeTrue();
                    logEventsList.Last().MessageTemplate.IsLogEventOnError().Should().BeFalse();
                }
            }
        }
    }
}