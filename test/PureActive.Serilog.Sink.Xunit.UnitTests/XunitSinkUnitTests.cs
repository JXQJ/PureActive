using System;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using PureActive.Logger.Provider.Serilog.Settings;
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


        [Fact]
        public void Create_XUnit_Sink_TestCorrelator()
        {
            var loggerSettings = new SerilogLoggerSettings(LogEventLevel.Debug);
            var loggerConfiguration = XunitLoggingSink.CreateXUnitLoggerConfiguration(_testOutputHelper, loggerSettings,
                XunitLoggingSink.XUnitSerilogFormatter.RenderedCompactJsonFormatter);

            var loggerFactory = XunitLoggingSink.CreateXUnitSerilogFactory(loggerSettings, loggerConfiguration);
            var logger = loggerFactory.CreateLogger<XunitSinkUnitTests>();

            using (TestCorrelator.CreateContext())
            {
                logger.LogInformation("Test: Create_XUnit_Sink_TestCorrelator");

                TestCorrelator.GetLogEventsFromCurrentContext()
                    .Should().ContainSingle()
                    .Which.MessageTemplate.Text
                    .Should().Be("Test: Create_XUnit_Sink_TestCorrelator");
            }
        }

        [Fact]
        public void Create_XUnit_Sink_RenderedCompactJsonFormatter()
        {
            var loggerSettings = new SerilogLoggerSettings(LogEventLevel.Debug);
            var loggerFactory = XunitLoggingSink.CreateXUnitSerilogFactory(_testOutputHelper, loggerSettings, 
                XunitLoggingSink.XUnitSerilogFormatter.RenderedCompactJsonFormatter);

            var logger = loggerFactory.CreateLogger<XunitSinkUnitTests>();

            logger.LogDebug("Create_XUnit_Sink_RenderedCompactJsonFormatter");
        }
    }
}
