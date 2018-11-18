using System.Net;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using PureActive.Logging.Extensions.Extensions;
using PureActive.Network.Abstractions.CommonNetworkServices;
using PureActive.Network.Abstractions.Extensions;
using PureActive.Network.Abstractions.Types;
using PureActive.Network.Devices.Network;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Network.Services.IntegrationTests.Network
{
    [Trait("Category", "Integration")]
    public class NetworkGatewayIntegrationTests : TestBaseLoggable<NetworkGatewayIntegrationTests>
    {
        private readonly ICommonNetworkServices _commonNetworkServices;
        private readonly IPAddressSubnet _gatewayIPAddressSubnet;

        public NetworkGatewayIntegrationTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _commonNetworkServices = CommonNetworkServices.CreateInstance(TestLoggerFactory, "NetworkGatewayIntegrationTests");
            _gatewayIPAddressSubnet = IPAddressExtensions.GetDefaultGatewayAddressSubnet(Logger);
        }

        [Fact]
        public void NetworkGateway_ToString()
        {
            var networkGateway = new NetworkGateway(_commonNetworkServices, _gatewayIPAddressSubnet);
            TestOutputHelper.WriteLine(networkGateway.ToString());
        }


        [Fact]
        public void NetworkGateway_Logging()
        {
            var networkGateway = new NetworkGateway(_commonNetworkServices, _gatewayIPAddressSubnet);

            var logLevel = LogLevel.Debug;

            using (networkGateway.PushLogProperties(logLevel))
            {
                Logger?.LogDebug("TestDeviceLogging {LogLevel}", logLevel);
            }
        }

        [Fact]
        public void NetworkGateway_Logging_WithParents()
        {
            var networkGateway = new NetworkGateway(_commonNetworkServices, _gatewayIPAddressSubnet);
            var logLevel = LogLevel.Debug;

            using (networkGateway.PushLogPropertiesParents(logLevel))
            {
                Logger?.LogDebug("TestDeviceLoggingWithParents {LogLevel}", logLevel);
            }
        }

        [Fact]
        public void NetworkGateway_SwitchGateway()
        {
            var networkGateway = new NetworkGateway(_commonNetworkServices, _gatewayIPAddressSubnet);
            var physicalAddress = networkGateway.PhysicalAddress;
            physicalAddress.Should().NotBeNull();
            
            networkGateway.IPAddressSubnet = new IPAddressSubnet(IPAddress.Parse("10.1.10.33"), IPAddressExtensions.SubnetClassC);

            var newPhysicalAddress = networkGateway.PhysicalAddress;
            networkGateway.PhysicalAddress.Should().Be(newPhysicalAddress);
        }
    }
}