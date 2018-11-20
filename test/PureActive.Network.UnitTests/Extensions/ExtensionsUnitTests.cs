using System.Net.NetworkInformation;
using PureActive.Network.Abstractions.Extensions;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Network.UnitTests.Extensions
{
    [Trait("Category", "Unit")]
    public class ExtensionsUnitTests : TestBaseLoggable<ExtensionsUnitTests>
    {
        public ExtensionsUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }
    
        private const string PhysicalAddressZeros = "00-00-00-00-00-00";

        [Fact]
        public void PhysicalAddress_ToDashString()
        {
            PhysicalAddress physicalAddress = PhysicalAddress.Parse("00-1A-8C-46-27-D0");
            var physicalAddressDash = physicalAddress.ToDashString();
            Assert.Equal(physicalAddressDash, $"00-1A-8C-46-27-D0");
        }

        [Fact]
        public void PhysicalAddress_ToColonString()
        {
            PhysicalAddress physicalAddress = PhysicalAddress.Parse("00-1A-8C-46-27-D0");
            var physicalAddressColon = physicalAddress.ToColonString();
            Assert.Equal(physicalAddressColon, $"00:1A:8C:46:27:D0");
        }


        [Theory]
        [InlineData("0-0-0-0-0-0", PhysicalAddressZeros)]
        [InlineData("1:1A:8C:46:27:D0", "01-1A-8C-46-27-D0")]
        [InlineData("00:1A:8C:46:27:D0", "00-1A-8C-46-27-D0")]
        [InlineData("", null)]
        [InlineData(null, null)]
        [InlineData("3:4:5", "03-04-05")]
        public void PhysicalAddress_NormalizedParse(string physicalAddressString, string physicalAddressStringExpectedValue)
        { 
            var physicalAddressExpectedValue = string.IsNullOrEmpty(physicalAddressStringExpectedValue) ? PhysicalAddress.None : PhysicalAddress.Parse(physicalAddressStringExpectedValue);
            var physicalAddressNormalized = PhysicalAddressExtensions.NormalizedParse(physicalAddressString);

            Assert.Equal(physicalAddressExpectedValue, physicalAddressNormalized);
        }
    }
}
