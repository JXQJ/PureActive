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
    public class NetworkGatewayUnitTests : TestBaseLoggable<NetworkGatewayUnitTests>
    {
        private readonly ICommonNetworkServices _commonNetworkServices;

        private static readonly IPAddressSubnet TestIPAddressSubnet =
            new IPAddressSubnet(IPAddress.Parse("10.1.10.1"), IPAddressExtensions.SubnetClassC);

        public NetworkGatewayUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _commonNetworkServices = CommonNetworkServices.CreateInstance(TestLoggerFactory, "NetworkGatewayUnitTests");
        }

        [Fact]
        public void NetworkGateway_Constructor()
        {
            var networkGateway = new NetworkGateway(_commonNetworkServices, TestIPAddressSubnet);
            networkGateway.Should().NotBeNull();
        }

        [Fact]
        public void NetworkGateway_ToString()
        {
            var networkGateway = new NetworkGateway(_commonNetworkServices, TestIPAddressSubnet);
            TestOutputHelper.WriteLine(networkGateway.ToString());
        }
    }
}