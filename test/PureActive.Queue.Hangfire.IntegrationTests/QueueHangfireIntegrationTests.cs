using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Queue.Hangfire.IntegrationTests
{
    [Trait("Category", "Integration")]
    public class QueueHangfireIntegrationTests : TestBaseLoggable<QueueHangfireIntegrationTests>
    {
        public QueueHangfireIntegrationTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        [Fact]
        public void QueueHangfire_Integration()
        {

        }
        
    }
}
