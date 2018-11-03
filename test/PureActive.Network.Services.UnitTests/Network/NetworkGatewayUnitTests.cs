using System.Net;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using PureActive.Network.Abstractions.CommonNetworkServices;
using PureActive.Network.Abstractions.Extensions;
using PureActive.Network.Abstractions.Types;
using PureActive.Network.Devices.Network;
using PureActive.Network.Services.Services;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Network.Services.UnitTests.Network
{
    public class NetworkGatewayUnitTests : LoggingUnitTestBase<NetworkGatewayUnitTests>
    {
        private readonly ICommonNetworkServices _commonNetworkServices;
        private readonly IPAddressSubnet _gatewayIPAddressSubnet;

        public NetworkGatewayUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _commonNetworkServices = CommonNetworkServices.CreateInstance(TestLoggerFactory, "NetworkGatewayUnitTests");
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

            // TODO: Fix Logging With
            //using (networkGateway.With(logLevel))
            //{
                Logger?.LogDebug("TestDeviceLogging {LogLevel}", logLevel);
            //}
        }

        [Fact]
        public void TestDeviceLoggingWithParents()
        {
            var networkGateway = new NetworkGateway(_commonNetworkServices, _gatewayIPAddressSubnet);
            var logLevel = LogLevel.Debug;

            // TODO: Fix Logging With Parents
            //using (networkGateway.WithParents(logLevel))
            //{
                Logger?.LogDebug("TestDeviceLoggingWithParents {LogLevel}", logLevel);
            //}
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