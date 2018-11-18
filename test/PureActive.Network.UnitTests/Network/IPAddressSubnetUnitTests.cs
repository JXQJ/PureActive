using System.Collections;
using System.Collections.Generic;
using System.Net;
using PureActive.Network.Abstractions.Extensions;
using PureActive.Network.Abstractions.Types;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Network.UnitTests.Network
{
    [Trait("Category", "Unit")]
    public class IPAddressSubnetUnitTests : TestBaseLoggable<IPAddressSubnetUnitTests>
    {
        public IPAddressSubnetUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        private class NetworkAddressTestGenerator : IEnumerable<object[]>
        {
            private static readonly IPAddress NetworkAddressClassA = IPAddress.Parse("10.0.0.0");
            private static readonly IPAddress NetworkAddressClassB = IPAddress.Parse("172.16.0.0");
            private static readonly IPAddress NetworkAddressClassC = IPAddress.Parse("192.168.1.0");

            private static readonly IPAddress NetworkAddressClass1C = IPAddress.Parse("10.0.1.0");

            public static readonly IPAddress TestAddressClassA = IPAddress.Parse("10.0.0.5");
            public static readonly IPAddress TestAddressClassB = IPAddress.Parse("172.16.0.5");
            public static readonly IPAddress TestAddressClassC = IPAddress.Parse("192.168.1.5");

            public static readonly IPAddress TestAddressClass1A = IPAddress.Parse("10.0.1.5");
            public static readonly IPAddress TestAddressClass1B = IPAddress.Parse("172.16.1.5");

            private readonly List<object[]> _data = new List<object[]>
            {
                new object[] {TestAddressClassA, IPAddressExtensions.SubnetClassA, NetworkAddressClassA},
                new object[] {TestAddressClassB, IPAddressExtensions.SubnetClassB, NetworkAddressClassB},
                new object[] {TestAddressClassC, IPAddressExtensions.SubnetClassC, NetworkAddressClassC},

                new object[] {TestAddressClass1A, IPAddressExtensions.SubnetClassA, NetworkAddressClassA},
                new object[] {TestAddressClass1B, IPAddressExtensions.SubnetClassB, NetworkAddressClassB},

                new object[] {TestAddressClass1A, IPAddressExtensions.SubnetClassC, NetworkAddressClass1C},
            };

            public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Theory]
        [ClassData(typeof(NetworkAddressTestGenerator))]
        public void NetworkAddressTest(IPAddress ipAddress, IPAddress subnetAddress, IPAddress ipAddressExpected)
        {
            IPAddressSubnet ipAddressSubnet = new IPAddressSubnet(ipAddress, subnetAddress);

            Assert.Equal(ipAddressExpected, ipAddressSubnet.NetworkAddress);
        }

        private class BroadcastAddressTestGenerator : IEnumerable<object[]>
        {

            private static readonly IPAddress BroadcastAddressClassA = IPAddress.Parse("10.255.255.255");
            private static readonly IPAddress BroadcastAddressClassB = IPAddress.Parse("172.16.255.255");
            private static readonly IPAddress BroadcastAddressClassC = IPAddress.Parse("192.168.1.255");

            private static readonly IPAddress BroadcastAddressClass1C = IPAddress.Parse("10.0.1.255");

            private readonly List<object[]> _data = new List<object[]>

            {
                new object[] {NetworkAddressTestGenerator.TestAddressClassA, IPAddressExtensions.SubnetClassA, BroadcastAddressClassA},
                new object[] {NetworkAddressTestGenerator.TestAddressClassB, IPAddressExtensions.SubnetClassB, BroadcastAddressClassB},
                new object[] {NetworkAddressTestGenerator.TestAddressClassC, IPAddressExtensions.SubnetClassC, BroadcastAddressClassC},

                new object[] {NetworkAddressTestGenerator.TestAddressClass1A, IPAddressExtensions.SubnetClassA, BroadcastAddressClassA},
                new object[] {NetworkAddressTestGenerator.TestAddressClass1B, IPAddressExtensions.SubnetClassB, BroadcastAddressClassB},

                new object[] {NetworkAddressTestGenerator.TestAddressClass1A, IPAddressExtensions.SubnetClassC, BroadcastAddressClass1C},
            };

            public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Theory]
        [ClassData(typeof(BroadcastAddressTestGenerator))]
        public void BroadcastAddressTest(IPAddress ipAddress, IPAddress subnetAddress, IPAddress ipAddressExpected)
        {
            IPAddressSubnet ipAddressSubnet = new IPAddressSubnet(ipAddress, subnetAddress);

            Assert.Equal(ipAddressExpected, ipAddressSubnet.BroadcastAddress);
        }
    }
}
