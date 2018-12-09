using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Hosting.IntegrationTests.Configuration
{
    [Trait("Category", "Integration")]
    public class ServiceCollectionExtensionsIntegrationTests : TestBaseLoggable<ServiceCollectionExtensionsIntegrationTests>
    {
        public ServiceCollectionExtensionsIntegrationTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        [Fact]
        public void ServiceCollectionExtensionsIntegrationTests_AddHangfireQueue()
        {
         
        }
    }
}