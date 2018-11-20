using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Network.UnitTests
{
    [Trait("Category", "Unit")]
    public class NetworkUnitTests : TestBaseLoggable<NetworkUnitTests>
    {
        public NetworkUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void Network_Constructor()
        {
        }
    }
}