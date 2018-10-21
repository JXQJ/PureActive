using System;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PureActive.Core.Abstractions.System;
using PureActive.Logger.Provider.Serilog.Enrichers;
using PureActive.Logger.Provider.Serilog.Interfaces;
using Serilog;
using Serilog.AspNetCore;
using Serilog.Events;
using Serilog.ExtensionMethods;
using Serilog.Formatting;
using Serilog.Formatting.Compact;
using Serilog.Formatting.Json;

namespace PureActive.Logger.Provider.Serilog.Configuration
{
    /// <summary>
    ///     Creates a logger configuration.
    /// </summary>
    public static class LoggerConfigurationFactory
    {



        public static LoggerConfiguration CreateDefaultLoggerConfiguration(IPureLoggerSettings loggerSettings)
        {
            var loggerConfiguration = new LoggerConfiguration()
                .ReadFrom.Configuration(loggerSettings.Configuration)
                .MinimumLevel.ControlledBy(loggerSettings.Default.LoggingLevelSwitch)
                .Enrich.FromLogContext()
                .Enrich.With(new AsyncFriendlyStackTraceEnricher())
                .WriteTo.Console(levelSwitch:loggerSettings.Console.LoggingLevelSwitch); // Always write to the console

            return loggerConfiguration;
        }

        /// <summary>
        ///     Creates a logger for Serilog.
        /// </summary>
        public static LoggerConfiguration CreateLoggerConfiguration(
            IConfigurationRoot configurationRoot,
            IFileSystem fileSystem,
            string logFileName,
            IPureLoggerSettings loggerSettings,
            Func<LogEvent, bool> includeLogEvent)
        {
            var loggerConfiguration = CreateDefaultLoggerConfiguration(loggerSettings);

            // Write to disk if requested
            var rollingFilePath = fileSystem.LogFolderPath() + logFileName;
            loggerConfiguration.WriteTo.RollingFile(rollingFilePath, levelSwitch:loggerSettings.File.LoggingLevelSwitch);

            // Write to application insights if requested
            var appInsightsKey = configurationRoot?.GetSection("ApplicationInsights")?["InstrumentationKey"];

            if (appInsightsKey != null)
                loggerConfiguration.WriteTo.ApplicationInsightsTraces
                (
                    appInsightsKey,
                    loggerSettings.AppInsights.MinimumLevel,
                    null /*formatProvider*/,
                    (logEvent, formatProvider) =>
                        ConvertLogEventsToCustomTraceTelemetry(logEvent, formatProvider, includeLogEvent)
                );

            return loggerConfiguration;
        }

        /// <summary>
        ///     Converts Serilog traces/exceptions to application insights traces/exceptions.
        /// </summary>
        private static ITelemetry ConvertLogEventsToCustomTraceTelemetry(
            LogEvent logEvent,
            IFormatProvider formatProvider,
            Func<LogEvent, bool> includeLogEvent)
        {
            if (logEvent.Exception == null && includeLogEvent != null && !includeLogEvent(logEvent)) return null;

            // first create a default TraceTelemetry using the sink's default logic
            // .. but without the log level, and (rendered) message (template) included in the Properties
            var telemetry = logEvent.Exception == null
                ? logEvent.ToDefaultTraceTelemetry(
                    formatProvider,
                    false,
                    false,
                    false)
                : (ITelemetry) logEvent.ToDefaultExceptionTelemetry(
                    formatProvider,
                    false,
                    false,
                    false);

            // and remove RequestId from the telemetry properties
            if (logEvent.Properties.ContainsKey("RequestId"))
                ((ISupportProperties) telemetry).Properties.Remove("RequestId");

            // Convert OperationId to String
            if (logEvent.Properties.ContainsKey("OperationId"))
            {
                var operationId = logEvent.Properties["OperationId"].ToString();

                ((ISupportProperties) telemetry).Properties.Remove("OperationId");

                telemetry.Context.Operation.Id = operationId;
            }

            return telemetry;
        }

        public static ILoggerFactory CreateSerilogFactory(IPureLoggerSettings loggerSettings, LoggerConfiguration loggerConfiguration = null, bool useStaticLogger = true)
        {
            if (loggerConfiguration == null)
                loggerConfiguration = CreateDefaultLoggerConfiguration(loggerSettings);

            // Set 
            var logger = loggerConfiguration?.CreateLogger();

            if (useStaticLogger)
                Log.Logger = logger;

            return  new SerilogLoggerFactory(logger, useStaticLogger);
        }
    }
}