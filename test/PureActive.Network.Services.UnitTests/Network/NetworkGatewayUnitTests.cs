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

namespace PureActive.Network.Services.UnitTests.Network
{
    [Trait("Category", "Unit")]
    public class NetworkGatewayTests : TestLoggerBase<NetworkGatewayTests>
    {
        private readonly ICommonNetworkServices _commonNetworkServices;
        private readonly IPAddressSubnet _gatewayIPAddressSubnet;

        public NetworkGatewayTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _commonNetworkServices = CommonNetworkServices.CreateInstance(TestLoggerFactory, "NetworkGatewayTests");
            _gatewayIPAddressSubnet = IPAddressExtensions.GetDefaultGatewayAddressSubnet(Logger);
        }

        [Fact]
        public void TestDeviceToString()
        {
            var networkGateway = new NetworkGateway(_commonNetworkServices, _gatewayIPAddressSubnet);
            TestOutputHelper.WriteLine(networkGateway.ToString());
        }


        [Fact]
        public void TestDeviceLogging()
        {
            var networkGateway = new NetworkGateway(_commonNetworkServices, _gatewayIPAddressSubnet);

            var logLevel = LogLevel.Debug;

            using (networkGateway.PushLogProperties(logLevel))
            {
                Logger?.LogDebug("TestDeviceLogging {LogLevel}", logLevel);
            }
        }

        [Fact]
        public void TestDeviceLoggingWithParents()
        {
            var networkGateway = new NetworkGateway(_commonNetworkServices, _gatewayIPAddressSubnet);
            var logLevel = LogLevel.Debug;

            using (networkGateway.PushLogPropertiesParents(logLevel))
            {
                Logger?.LogDebug("TestDeviceLoggingWithParents {LogLevel}", logLevel);
            }
        }

        [Fact]
        public void TestSwitchNetworkGateway()
        {
            var networkGateway = new NetworkGateway(_commonNetworkServices, _gatewayIPAddressSubnet);
            var physicalAddress = networkGateway.PhysicalAddress;
            
            networkGateway.IPAddressSubnet = new IPAddressSubnet(IPAddress.Parse("10.1.10.33"), IPAddressExtensions.SubnetClassC);

            var newPhysicalAddress = networkGateway.PhysicalAddress;
            networkGateway.PhysicalAddress.Should().Be(newPhysicalAddress);
        }
    }
}