using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Core.IntegrationTests.System
{
    [Trait("Category", "Integration")]
    public class OperatingSystemIntegrationTests : TestBaseLoggable<OperatingSystemIntegrationTests>
    {
        public OperatingSystemIntegrationTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }
    }
}