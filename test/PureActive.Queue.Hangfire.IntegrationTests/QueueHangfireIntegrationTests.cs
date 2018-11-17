using FluentAssertions;
using Hangfire.Logging;
using PureActive.Core.Extensions;
using PureActive.Queue.Hangfire.Queue;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Serilog.Sinks.TestCorrelator;
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
