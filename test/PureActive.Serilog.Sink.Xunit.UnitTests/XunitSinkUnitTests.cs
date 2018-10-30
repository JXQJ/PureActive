using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using PureActive.Logger.Provider.Serilog.Settings;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Logging.Abstractions.Types;
using PureActive.Serilog.Sink.Xunit.Sink;
using Serilog.Events;
using Serilog.Sinks.TestCorrelator;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Serilog.Sink.Xunit.UnitTests
{
    public class XunitSinkUnitTests
    {
        private readonly ITestOutputHelper _testOutputHelper;
        
        public XunitSinkUnitTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper ?? throw new ArgumentNullException(nameof(testOutputHelper));
        }

        private IPureLogger CreateLogger(LogEventLevel logEventLevel,
            XUnitSerilogFormatter xUnitSerilogFormatter = XUnitSerilogFormatter.RenderedCompactJsonFormatter)
        {
            var loggerSettings = new SerilogLoggerSettings(logEventLevel, LoggingOutputFlags.Testing);
            var loggerConfiguration = XunitLoggingSink.CreateXUnitLoggerConfiguration(_testOutputHelper, loggerSettings, xUnitSerilogFormatter);

            var loggerFactory = XunitLoggingSink.CreateXUnitSerilogFactory(loggerSettings, loggerConfiguration);

            var logger = loggerFactory.CreatePureLogger<XunitSinkUnitTests>();

            logger.Should().NotBeNull("CreateLogger should always succeed");

            return logger;
        }

        [Fact]
        public void XunitSink_Create_TestCorrelator()
        {
            var loggerSettings = new SerilogLoggerSettings(LogEventLevel.Debug, LoggingOutputFlags.TestCorrelator);
            var loggerConfiguration = XunitLoggingSink.CreateXUnitLoggerConfiguration(_testOutputHelper, loggerSettings, XUnitSerilogFormatter.RenderedCompactJsonFormatter);
            var loggerFactory = XunitLoggingSink.CreateXUnitSerilogFactory(loggerSettings, loggerConfiguration);
            var logger = loggerFactory.CreatePureLogger<XunitSinkUnitTests>();

            logger.Should().NotBeNull("CreateLogger should always succeed");

            using (TestCorrelator.CreateContext())
            {
                logger.LogInformation("Test: XunitSink_Create_TestCorrelator");

                TestCorrelator.GetLogEventsFromCurrentContext()
                    .Should().ContainSingle()
                    .Which.MessageTemplate.Text
                    .Should().Be("Test: XunitSink_Create_TestCorrelator");
            }

        }


        [Fact]
        public void XunitSink_String_TestCorrelator()
        {
            var logger = CreateLogger(LogEventLevel.Debug);

            using (TestCorrelator.CreateContext())
            {
                logger.LogInformation("Test: XunitSink_String_TestCorrelator");

                TestCorrelator.GetLogEventsFromCurrentContext()
                    .Should().ContainSingle()
                    .Which.MessageTemplate.Text
                    .Should().Be("Test: XunitSink_String_TestCorrelator");
            }
        }

        [Fact]
        public void XunitSink_Create_RenderedCompactJsonFormatter()
        {
            var logger = CreateLogger(LogEventLevel.Debug);

            logger.LogDebug("Create_XUnit_Sink_RenderedCompactJsonFormatter");
        }

        [Fact]
        public void XunitSink_Logger_TestCorrelator_Param_Int()
        {
            var logger = CreateLogger(LogEventLevel.Debug);

            using (TestCorrelator.CreateContext())
            {
                const int count = 15;

                var dictionary = new Dictionary<string, LogEventPropertyValue>()
                {
                    {"Count", new ScalarValue(count)}
                };

                logger.LogInformation("{Count}", count);

                TestCorrelator.GetLogEventsFromCurrentContext()
                    .Should().ContainSingle()
                    .Which.MessageTemplate.Render(dictionary)
                    .Should().Be(count.ToString());
            }
        }
    }
}
