using System;
using Microsoft.Extensions.Logging;
using PureActive.Logger.Provider.Serilog.Configuration;
using PureActive.Logger.Provider.Serilog.Interfaces;
using PureActive.Logger.Provider.Serilog.Types;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Serilog.Sink.Xunit.Extensions;
using Serilog;
using Serilog.Formatting;
using Serilog.Formatting.Compact;
using Serilog.Formatting.Json;
using Xunit.Abstractions;

namespace PureActive.Serilog.Sink.Xunit.Sink
{
    public enum XUnitSerilogFormatter
    {
        None,
        JsonFormatter,
        CompactJsonFormatter,
        RenderedJsonFormatter,
        RenderedCompactJsonFormatter,
    }

    public class XunitLoggingSink
    {
        public static ITextFormatter GetXUnitSerilogFormatter(XUnitSerilogFormatter xUnitSerilogFormatter)
        {
            switch (xUnitSerilogFormatter)
            {
                case XUnitSerilogFormatter.JsonFormatter:
                    return new JsonFormatter();
                case XUnitSerilogFormatter.CompactJsonFormatter:
                    return new CompactJsonFormatter();
                case XUnitSerilogFormatter.RenderedJsonFormatter:
                    return new JsonFormatter(null, true);
                case XUnitSerilogFormatter.RenderedCompactJsonFormatter:
                    return new RenderedCompactJsonFormatter();
                default:
                    return null;
            }
        }
        
        public static LoggerConfiguration CreateXUnitLoggerConfiguration(ITestOutputHelper testOutputHelper, ISerilogLoggerSettings loggerSettings, XUnitSerilogFormatter xUnitSerilogFormatter)
        {
            if (testOutputHelper == null) throw new ArgumentNullException(nameof(testOutputHelper));
            if (loggerSettings == null) throw new ArgumentNullException(nameof(loggerSettings));

            var loggerConfiguration = LoggerConfigurationFactory.CreateDefaultLoggerConfiguration(loggerSettings);

            var jsonFormatter = GetXUnitSerilogFormatter(xUnitSerilogFormatter);

            if (jsonFormatter != null)
            {
                loggerConfiguration.WriteTo.XUnit(testOutputHelper, jsonFormatter, loggerSettings.TestConsole.MinimumLevel, loggerSettings.TestConsole.LoggingLevelSwitch);
            }
            else
            {
                loggerConfiguration.WriteTo.XUnit(testOutputHelper, loggerSettings.TestConsole.MinimumLevel, XUnitLoggerConfigurationExtensions.DefaultOutputTemplate,
                    null, loggerSettings.TestConsole.LoggingLevelSwitch);
            }

            // Configuration switch to TestCorrelator
            loggerConfiguration.WriteTo.TestCorrelator(loggerSettings.TestCorrelator.MinimumLevel, loggerSettings.TestCorrelator.LoggingLevelSwitch);

            return loggerConfiguration;
        }

        public static IPureLoggerFactory CreateXUnitSerilogFactory(ISerilogLoggerSettings loggerSettings, LoggerConfiguration loggerConfiguration)
        {
            if (loggerSettings == null) throw new ArgumentNullException(nameof(loggerSettings));
            if (loggerConfiguration == null) throw new ArgumentNullException(nameof(loggerConfiguration));

            var loggerFactory = LoggerConfigurationFactory.CreateSerilogFactory(loggerSettings, loggerConfiguration);

            loggerFactory.AddDebug();

            return new PureSeriLoggerFactory(loggerFactory);
        }

        public static IPureLoggerFactory CreateXUnitSerilogFactory(ITestOutputHelper testOutputHelper,
            ISerilogLoggerSettings loggerSettings,
            XUnitSerilogFormatter xUnitSerilogFormatter = XUnitSerilogFormatter.None)
        {
            if (testOutputHelper == null) throw new ArgumentNullException(nameof(testOutputHelper));
            if (loggerSettings == null) throw new ArgumentNullException(nameof(loggerSettings));

            var loggerConfiguration = CreateXUnitLoggerConfiguration(testOutputHelper, loggerSettings, xUnitSerilogFormatter);

            return CreateXUnitSerilogFactory(loggerSettings, loggerConfiguration);
        }
    }
}
