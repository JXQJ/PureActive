using FluentAssertions;
using PureActive.Core.Extensions;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Core.UnitTests.Extensions
{
    [Trait("Category", "Unit")]
    public class IntegerExtensionsUnitTests : TestBaseLoggable<IntegerExtensionsUnitTests>
    {
        public IntegerExtensionsUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        [Fact]
        public void IntegerExtensions_ToInt16HexString()
        {
            const short int16 = 256;
            var prefix = "0x";
            var hexString = int16.ToHexString(prefix);

            hexString.Should().Be($"{prefix}{int16:X4}");
        }

        [Fact]
        public void IntegerExtensions_ToInt32HexString()
        {
            const int int32 = 256;
            var prefix = "0x";
            var hexString = int32.ToHexString(prefix);

            hexString.Should().Be($"{prefix}{int32:X8}");
        }

        [Fact]
        public void IntegerExtensions_ToUInt16HexString()
        {
            const ushort uint16 = 256;
            var prefix = "0x";
            var hexString = uint16.ToHexString(prefix);

            hexString.Should().Be($"{prefix}{uint16:X4}");
        }

        [Fact]
        public void IntegerExtensions_ToUInt32HexString()
        {
            const uint uint32 = 256;
            var prefix = "0x";
            var hexString = uint32.ToHexString(prefix);

            hexString.Should().Be($"{prefix}{uint32:X8}");
        }

        [Fact]
        public void IntegerExtensions_ToInt16HexStringNullPrefix()
        {
            // ReSharper disable ExpressionIsAlwaysNull
            const short int16 = 256;
            string prefix = null;
            var hexString = int16.ToHexString(prefix);

            hexString.Should().Be($"{prefix}{int16:X4}");
            // ReSharper restore ExpressionIsAlwaysNull
        }
    }
}
