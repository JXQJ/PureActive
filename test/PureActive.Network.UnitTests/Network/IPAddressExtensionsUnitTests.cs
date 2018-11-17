using System.Net;
using PureActive.Network.Abstractions.Extensions;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Network.UnitTests.Network
{
    [Trait("Category", "Unit")]
    public class IPAddressExtensionsUnitTests : TestBaseLoggable<IPAddressExtensionsUnitTests>
    {
        public IPAddressExtensionsUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        [Fact]
        public void IPAddressToLongTest()
        {
            IPAddress ipAddress = IPAddress.Parse("1.2.3.4");

            var longTest = ipAddress.ToLong();

            var ipAddressTest = new IPAddress(longTest);

            Assert.Equal(ipAddressTest, ipAddress);
        }

        [Theory]
        [InlineData("1.2.3.4", "1.2.3.5")]
        [InlineData("255.255.255.255", "255.255.255.255")]
        public void IPAddressIncrementTest(string ipAddressStringTest, string ipAddressStringExpected)
        {
            IPAddress ipAddressTest = IPAddress.Parse(ipAddressStringTest);
            IPAddress ipAddressExpected = IPAddress.Parse(ipAddressStringExpected);

            var ipAddressIncrement = ipAddressTest.Increment();
            
            Assert.Equal(ipAddressExpected, ipAddressIncrement);
        }

        [Theory]
        [InlineData("1.2.3.4", "1.2.3.3")]
        [InlineData("0.0.0.0", "0.0.0.0")]
        public void IPAddressDecrementTest(string ipAddressStringTest, string ipAddressStringExpected)
        {
            IPAddress ipAddressTest = IPAddress.Parse(ipAddressStringTest);
            IPAddress ipAddressExpected = IPAddress.Parse(ipAddressStringExpected);

            var ipAddressDecrement = ipAddressTest.Decrement();

            Assert.Equal(ipAddressExpected, ipAddressDecrement);
        }

        [Fact]
        public void GetDefaultGatewayAddressSubnetTest()
        {
            var addressSubnet = IPAddressExtensions.GetDefaultGatewayAddressSubnet(Logger);
        }
    }
}
