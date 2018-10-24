using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions.Internal;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Extensions;
using Serilog.Events;
using Serilog.Sinks.TestCorrelator;

namespace PureActive.Serilog.Sink.Xunit.UnitTests
{
    public class LoggingUnitTestBaseUnitTests : LoggingUnitTestBase<LoggingUnitTestBaseUnitTests>
    {
        public LoggingUnitTestBaseUnitTests(ITestOutputHelper testOutputHelper) :
            base(testOutputHelper)
        {

        }

        [Fact]
        public void LoggingUnitTestBase_Constructor()
        {
            LoggerFactory.Should().NotBeNull("initialized in constructor");
            LoggerSettings.Should().NotBeNull("initialized in constructor");
            Logger.Should().NotBeNull("initialized in constructor");
            TestOutputHelper.Should().NotBeNull("initialized in constructor");
        }


        [Fact]
        public void LoggingUnitTestBase_Logger_TestCorrelator_String()
        {
            Logger.Should().NotBeNull("initialized in constructor");

            using (TestCorrelator.CreateContext())
            {
                Logger.LogInformation("Test: LoggingUnitTestBase_Logger");

                TestCorrelator.GetLogEventsFromCurrentContext()
                    .Should().ContainSingle()
                    .Which.MessageTemplate.Text
                    .Should().Be("Test: LoggingUnitTestBase_Logger");
            }
        }

        [Fact]
        public void LoggingUnitTestBase_Logger_TestCorrelator_Param_Int()
        {
            Logger.Should().NotBeNull("initialized in constructor");

            using (TestCorrelator.CreateContext())
            {
                int count = 15;

                var dictionary = new Dictionary<string, LogEventPropertyValue>()
                {
                    {"Count", new ScalarValue(count)}
                };

                Logger.LogInformation("{Count}", count);

                TestCorrelator.GetLogEventsFromCurrentContext()
                    .Should().ContainSingle()
                    .Which.MessageTemplate.Render(dictionary)
                    .Should().Be(count.ToString());
            }
        }

        [Fact]
        public void LoggingUnitTestBase_Logger_SourceContext()
        {
            Logger.Should().NotBeNull("initialized in constructor");

            using (TestCorrelator.CreateContext())
            {
                Logger.LogInformation("Test");

                TestCorrelator.GetLogEventsFromCurrentContext()
                    .Should().ContainSingle()
                    .Which.Properties["SourceContext"].ToString().Should()
                    .Be(TypeNameHelper.GetTypeDisplayName(typeof(LoggingUnitTestBaseUnitTests)).AddDoubleQuote());
            }
        }
    }
}
