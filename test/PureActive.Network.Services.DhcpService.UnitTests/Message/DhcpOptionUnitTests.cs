using System.Collections;
using System.Collections.Generic;
using PureActive.Core.Extensions;
using PureActive.Network.Services.DhcpService.Message;
using PureActive.Network.Services.DhcpService.Types;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Network.Services.DhcpService.UnitTests.Message
{
    [Trait("Category", "Unit")]
    public class DhcpOptionTests : TestLoggerBase<DhcpOptionTests>
    {
        private static readonly byte[] PhoneRequestDhcpMsgBytes = new byte[]
        {
            1, 1, 6, 0, 122, 6, 156, 94, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 64, 152, 173, 49,
            47, 138, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 99, 130, 83, 99, 53, 1, 3, 55, 7,
            1, 121, 3, 6, 15, 119, 252, 57, 2, 5, 220, 61, 7, 1, 64, 152, 173, 49, 47, 138, 50, 4, 10, 1, 10, 198, 51,
            4, 0, 118, 167, 0, 12, 15, 83, 116, 101, 118, 101, 66, 117, 115, 105, 80, 104, 111, 110, 101, 88, 255, 0, 0,
            0, 0, 0,
        };

        private static readonly byte[] BcDenDiscoverDhcpMsgBytes = new byte[]
        {
            1, 1, 6, 0, 136, 1, 157, 80, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 21, 209, 16, 1,
            22, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 99, 130, 83, 99, 53, 1, 1, 61, 7, 1, 2,
            21, 209, 16, 1, 22, 12, 5, 66, 67, 68, 101, 110, 60, 8, 77, 83, 70, 84, 32, 53, 46, 48, 55, 14, 1, 3, 6, 15,
            31, 33, 43, 44, 46, 47, 119, 121, 249, 252, 255, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        };

        private static readonly byte[] CcDenRequestDhcpMsgBytes = new byte[]
        {
            1, 1, 6, 0, 18, 102, 45, 9, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 244, 150, 52, 210,
            110, 55, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 99, 130, 83, 99, 53, 1, 3, 61, 7,
            1, 244, 150, 52, 210, 110, 55, 50, 4, 10, 1, 10, 22, 54, 4, 10, 1, 10, 1, 12, 5, 67, 67, 68, 101, 110, 81,
            24, 0, 0, 0, 67, 67, 68, 101, 110, 46, 66, 117, 115, 104, 67, 104, 97, 110, 103, 46, 108, 111, 99, 97, 108,
            60, 8, 77, 83, 70, 84, 32, 53, 46, 48, 55, 14, 1, 3, 6, 15, 31, 33, 43, 44, 46, 47, 119, 121, 249, 252, 255,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        };

        public DhcpOptionTests(ITestOutputHelper output) : base(output)
        {

        }

        [Trait("Category", "Unit")]
        private class DhcpMessageGenerator : IEnumerable<object[]>
        {
            private readonly List<object[]> _data = new List<object[]>

            {
                new object[] {PhoneRequestDhcpMsgBytes},
                new object[] {BcDenDiscoverDhcpMsgBytes},
                new object[] { CcDenRequestDhcpMsgBytes },
            };

            public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Trait("Category", "Unit")]
        [Theory]
        [ClassData(typeof(DhcpMessageGenerator))]
        public void TestDhcpOptionTypeMap(byte[] dhcpMessageBytes)
        {
            DhcpMessage dhcpRequestMessage = new DhcpMessage(dhcpMessageBytes, TestLoggerFactory);

            var maxLength = dhcpRequestMessage.DhcpOptionKeys().MaxStringLength() + 2;

            foreach (var dhcpOption in dhcpRequestMessage.DhcpOptionKeys())
            {
                var dhcpOptionString = DhcpOptionTypeMap.GetDhcpOptionString(dhcpOption,dhcpRequestMessage.GetOptionData(dhcpOption),
                    DhcpOptionTypeMap.DhcpRequestListFormat.StringCommaSeparated, Logger);

                TestOutputHelper.WriteLine($"{dhcpOption.ToString().PadWithDelim(": ", maxLength)}{dhcpOptionString}");
            }
        }
    }
}
