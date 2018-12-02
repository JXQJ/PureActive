// ***********************************************************************
// Assembly         : PureActive.Serilog.Sink.Xunit.UnitTests
// Author           : SteveBu
// Created          : 10-21-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="XunitSinkUnitTests.cs" company="BushChang Corporation">
//     � 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using PureActive.Core.System;
using PureActive.Logger.Provider.Serilog.Settings;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Logging.Abstractions.Types;
using PureActive.Serilog.Sink.Xunit.Sink;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.TestCorrelator;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Serilog.Sink.Xunit.UnitTests
{
    /// <summary>
    /// Class XunitSinkUnitTests.
    /// </summary>
    /// <autogeneratedoc />
    [Trait("Category", "Unit")]
    public class XunitSinkUnitTests
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XunitSinkUnitTests"/> class.
        /// </summary>
        /// <param name="testOutputHelper">The test output helper.</param>
        /// <exception cref="ArgumentNullException">testOutputHelper</exception>
        /// <autogeneratedoc />
        public XunitSinkUnitTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper ?? throw new ArgumentNullException(nameof(testOutputHelper));
        }

        /// <summary>
        /// The test output helper
        /// </summary>
        /// <autogeneratedoc />
        private readonly ITestOutputHelper _testOutputHelper;

        /// <summary>
        /// Creates the pure logger.
        /// </summary>
        /// <param name="logEventLevel">The log event level.</param>
        /// <param name="xUnitSerilogFormatter">The x unit serilog formatter.</param>
        /// <returns>IPureLogger.</returns>
        /// <autogeneratedoc />
        private IPureLogger CreatePureLogger(LogEventLevel logEventLevel,
            XUnitSerilogFormatter xUnitSerilogFormatter = XUnitSerilogFormatter.RenderedCompactJsonFormatter)
        {
            var fileSystem = new FileSystem(typeof(XunitSinkUnitTests));
            var loggerSettings = new SerilogLoggerSettings(fileSystem, logEventLevel, LoggingOutputFlags.Testing);
            var loggerConfiguration =
                XunitLoggingSink.CreateXUnitLoggerConfiguration(_testOutputHelper, loggerSettings,
                    xUnitSerilogFormatter);

            var loggerFactory = XunitLoggingSink.CreateXUnitSerilogFactory(loggerSettings, loggerConfiguration);

            var logger = loggerFactory.CreatePureLogger<XunitSinkUnitTests>();

            logger.Should().NotBeNull("CreatePureLogger should always succeed");

            return logger;
        }

        /// <summary>
        /// Defines the test method XunitSink_Create Formatter.
        /// </summary>
        /// <autogeneratedoc />
        [Theory]
        [InlineData(XUnitSerilogFormatter.CompactJsonFormatter)]
        [InlineData(XUnitSerilogFormatter.JsonFormatter)]
        [InlineData(XUnitSerilogFormatter.RenderedCompactJsonFormatter)]
        [InlineData(XUnitSerilogFormatter.RenderedJsonFormatter)]
        [InlineData(XUnitSerilogFormatter.None)]
        public void XunitSink_Create_Formatter(XUnitSerilogFormatter xUnitSerilogFormatter)
        {
            var logger = CreatePureLogger(LogEventLevel.Debug, xUnitSerilogFormatter);

            logger.LogDebug("Create_XUnit_Sink_{xUnitSerilogFormatter}", xUnitSerilogFormatter);
        }


        /// <summary>
        /// Defines the test method XunitSink_Create_TestCorrelator.
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public void XunitSink_Create_TestCorrelator()
        {
            var fileSystem = new FileSystem(typeof(XunitSinkUnitTests));
            var loggerSettings =
                new SerilogLoggerSettings(fileSystem, LogEventLevel.Debug, LoggingOutputFlags.TestCorrelator);
            var loggerConfiguration = XunitLoggingSink.CreateXUnitLoggerConfiguration(_testOutputHelper, loggerSettings,
                XUnitSerilogFormatter.RenderedCompactJsonFormatter);
            var loggerFactory = XunitLoggingSink.CreateXUnitSerilogFactory(loggerSettings, loggerConfiguration);
            var logger = loggerFactory.CreatePureLogger<XunitSinkUnitTests>();

            logger.Should().NotBeNull("CreatePureLogger should always succeed");

            using (TestCorrelator.CreateContext())
            {
                logger.LogInformation("Test: XunitSink_Create_TestCorrelator");

                TestCorrelator.GetLogEventsFromCurrentContext()
                    .Should().ContainSingle()
                    .Which.MessageTemplate.Text
                    .Should().Be("Test: XunitSink_Create_TestCorrelator");
            }
        }

        /// <summary>
        /// Defines the test method XunitSink_Logger_TestCorrelator_Param_Int.
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public void XunitSink_Logger_TestCorrelator_Param_Int()
        {
            var logger = CreatePureLogger(LogEventLevel.Debug);

            using (TestCorrelator.CreateContext())
            {
                const int count = 15;

                var dictionary = new Dictionary<string, LogEventPropertyValue>
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


        /// <summary>
        /// Defines the test method XunitSink_String_TestCorrelator.
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public void XunitSink_String_TestCorrelator()
        {
            var logger = CreatePureLogger(LogEventLevel.Debug);

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
        public void XunitSink_CreateXUnitLoggerConfiguration_Null()
        {
            var fileSystem = new FileSystem(typeof(XunitSinkUnitTests));
            var loggerSettings = new SerilogLoggerSettings(fileSystem, LogEventLevel.Debug, LoggingOutputFlags.TestCorrelator);

            Func<LoggerConfiguration> fx = () => XunitLoggingSink.CreateXUnitLoggerConfiguration(null, loggerSettings, XUnitSerilogFormatter.RenderedCompactJsonFormatter);

            fx.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("testOutputHelper");

            fx = () => XunitLoggingSink.CreateXUnitLoggerConfiguration(_testOutputHelper, null, XUnitSerilogFormatter.RenderedCompactJsonFormatter);

            fx.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("loggerSettings");
        }
    }
}