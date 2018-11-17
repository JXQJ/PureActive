using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Logger.Provider.ApplicationInsights.UnitTests
{
    [Trait("Category", "Unit")]
    public class ApplicationInsightsProviderUnitTests : LoggingUnitTestBase<ApplicationInsightsProviderUnitTests>
    {
        public ApplicationInsightsProviderUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        [Fact]
        public void ApplicationInsightsProvider_Constructor()
        {

        }
    }
}
