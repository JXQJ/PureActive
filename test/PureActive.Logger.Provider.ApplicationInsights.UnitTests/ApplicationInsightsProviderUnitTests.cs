using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Logger.Provider.ApplicationInsights.UnitTests
{
    [Trait("Category", "Unit")]
    public class ApplicationInsightsProviderTests : TestLoggerBase<ApplicationInsightsProviderTests>
    {
        public ApplicationInsightsProviderTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        [Fact]
        public void ApplicationInsightsProvider_Constructor()
        {

        }
    }
}
