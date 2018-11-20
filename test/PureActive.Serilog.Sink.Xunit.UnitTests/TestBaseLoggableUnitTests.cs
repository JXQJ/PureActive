using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions.Internal;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Extensions;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Serilog.Events;
using Serilog.Sinks.TestCorrelator;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Serilog.Sink.Xunit.UnitTests
{
    [Trait("Category", "Unit")]
    public class TestBaseLoggableUnitTests : TestBaseLoggable<TestBaseLoggableUnitTests>
    {
        public TestBaseLoggableUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void TestBaseLoggable_Constructor()
        {
            TestLoggerFactory.Should().NotBeNull("initialized in constructor");
            LoggerSettings.Should().NotBeNull("initialized in constructor");
            Logger.Should().NotBeNull("initialized in constructor");
            TestOutputHelper.Should().NotBeNull("initialized in constructor");
        }

        [Fact]
        public void TestBaseLoggable_Logger_SourceContext()
        {
            Logger.Should().NotBeNull("initialized in constructor");

            using (TestCorrelator.CreateContext())
            {
                Logger.LogInformation("Test");

                TestCorrelator.GetLogEventsFromCurrentContext()
                    .Should().ContainSingle()
                    .Which.Properties["SourceContext"].ToString().Should()
                    .Be(TypeNameHelper.GetTypeDisplayName(typeof(TestBaseLoggableUnitTests)).AddDoubleQuote());
            }
        }

        [Fact]
        public void TestBaseLoggable_Logger_TestCorrelator_Param_Int()
        {
            Logger.Should().NotBeNull("initialized in constructor");

            using (TestCorrelator.CreateContext())
            {
                int count = 15;

                var dictionary = new Dictionary<string, LogEventPropertyValue>
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
        public void TestBaseLoggable_Logger_TestCorrelator_String()
        {
            Logger.Should().NotBeNull("initialized in constructor");

            using (TestCorrelator.CreateContext())
            {
                Logger.LogInformation("Test: TestBaseLoggable_Logger");

                TestCorrelator.GetLogEventsFromCurrentContext()
                    .Should().ContainSingle()
                    .Which.MessageTemplate.Text
                    .Should().Be("Test: TestBaseLoggable_Logger");
            }
        }
    }
}