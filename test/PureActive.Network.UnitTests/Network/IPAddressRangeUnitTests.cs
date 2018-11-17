using System.Net;
using PureActive.Network.Abstractions.Extensions;
using PureActive.Network.Abstractions.Types;
using PureActive.Network.Extensions.Network;
using PureActive.Network.UnitTests.Extensions;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Network.UnitTests.Network
{
    [Trait("Category", "Unit")]
    public class IPAddressRangeUnitTests : TestBaseLoggable<IPAddressRangeUnitTests>
    {
        public IPAddressRangeUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }
    
        [Theory]
        [InlineData("10.1.10.255", "10.1.11.2", 4)]
        [InlineData("10.1.10.0", "10.1.11.255", 512)]
        public void IPAddressRangeTest(string ipAddressLowerInclusiveString, string ipAddressUpperInclusiveString, int total)
        {
            var ipAddressLowerInclusive = IPAddress.Parse(ipAddressLowerInclusiveString);
            var ipAddressUpperInclusive = IPAddress.Parse(ipAddressUpperInclusiveString);

            var ipAddressRange = new IPAddressRange(ipAddressLowerInclusive, ipAddressUpperInclusive);

            int count = 0;
            IPAddress ipAddressPrev = IPAddress.None;

            foreach (var ipAddress in ipAddressRange)
            {
                if (count++ == 0)
                    Assert.Equal(ipAddressLowerInclusive, ipAddress);
                else
                    Assert.Equal(ipAddress.Decrement(), ipAddressPrev);

                ipAddressPrev = ipAddress;
            }

            Assert.Equal(total, count);
            Assert.Equal(ipAddressUpperInclusive, ipAddressPrev);
        }

        [Theory]
        [InlineData("10.1.10.5", IPAddressExtensions.StringSubnetClassC, 256)]
        [InlineData("10.1.10.5", IPAddressExtensions.StringSubnetClassB, 65_536)]
 //       [InlineData("10.1.10.5", StringSubnetClassA, 16_777_216)]
        public void IPAddressRangeSubnetTest(string ipAddressLowerInclusiveString, string ipAddressSubnetString, int total)
        {
            var ipAddressLowerInclusive = IPAddress.Parse(ipAddressLowerInclusiveString);
            var ipAddressSubnetInclusive = IPAddress.Parse(ipAddressSubnetString);

            var ipAddressSubnet = new IPAddressSubnet(ipAddressLowerInclusive, ipAddressSubnetInclusive);

            var ipAddressRange = new IPAddressRange(ipAddressSubnet);

            int count = 0;
            IPAddress ipAddressPrev = IPAddress.None;

            foreach (var ipAddress in ipAddressRange)
            {
                if (count++ == 0)
                    Assert.Equal(ipAddressSubnet.NetworkAddress, ipAddress);
                else
                    Assert.Equal(ipAddress.Decrement(), ipAddressPrev);

                ipAddressPrev = ipAddress;
            }

            Assert.Equal(total, count);
            Assert.Equal(ipAddressSubnet.BroadcastAddress, ipAddressPrev);
        }
    }
}
