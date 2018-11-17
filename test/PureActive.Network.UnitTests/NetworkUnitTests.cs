using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Network.UnitTests
{
    [Trait("Category", "Unit")]
    public class NetworkTests : TestLoggerBase<NetworkTests>
    {
        public NetworkTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        [Fact]
        public void Network_Constructor()
        {

        }
    }
}
