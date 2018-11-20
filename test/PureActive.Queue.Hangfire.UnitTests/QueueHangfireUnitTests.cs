using FluentAssertions;
using Hangfire.Logging;
using PureActive.Core.Extensions;
using PureActive.Queue.Hangfire.Queue;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Serilog.Sinks.TestCorrelator;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Queue.Hangfire.UnitTests
{
    [Trait("Category", "Unit")]
    public class QueueHangfireUnitTests : TestBaseLoggable<QueueHangfireUnitTests>
    {
        public QueueHangfireUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void QueueHangfire_Logging()
        {
            var hangFireLogger = new HangfireLogProvider(TestLoggerFactory);
            var sourceContext = "QueueHangfireUnitTests";
            var logger = hangFireLogger.GetLogger(sourceContext);
            var testString = "Test: QueueHangfire_Logging";

            using (TestCorrelator.CreateContext())
            {
                logger.Log(LogLevel.Debug, () => testString);

                TestCorrelator.GetLogEventsFromCurrentContext()
                    .Should().ContainSingle()
                    .Which.Properties["State"].ToString()
                    .Should().Be(testString.ToDoubleQuoted());

                TestCorrelator.GetLogEventsFromCurrentContext()
                    .Should().ContainSingle()
                    .Which.Properties["SourceContext"].ToString()
                    .Should().Be(sourceContext.ToDoubleQuoted());
            }
        }
    }
}