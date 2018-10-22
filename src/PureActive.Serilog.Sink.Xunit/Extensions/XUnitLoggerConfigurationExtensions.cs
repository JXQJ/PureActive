using System;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Xunit.Abstractions;

namespace PureActive.Serilog.Sink.Xunit.Extensions
{
    public static class XUnitLoggerConfigurationExtensions
    {
        public const string DefaultOutputTemplate = "[{Timestamp:HH:mm:ss.fff} {Level:u3}]{Scope} {Message:lj}{NewLine}{Exception}";

        /// <summary>
        /// Writes log events to <see cref="T:System.Console" />.
        /// </summary>
        /// <param name="sinkConfiguration">Logger sink configuration.</param>
        /// <param name="testOutputHelper"></param>
        /// <param name="restrictedToMinimumLevel">The minimum level for
        /// events passed through the sink. Ignored when <paramref name="levelSwitch" /> is specified.</param>
        /// <param name="levelSwitch">A switch allowing the pass-through minimum level
        /// to be changed at runtime.</param>
        /// <param name="outputTemplate">A message template describing the format used to write to the sink.
        /// the default is <code>"[{Timestamp:HH:mm:ss} {Level:u3}] {Message}{NewLine}{Exception}"</code>.</param>
        /// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
        /// <returns>Configuration object allowing method chaining.</returns>
        public static LoggerConfiguration XUnit(this LoggerSinkConfiguration sinkConfiguration,
            ITestOutputHelper testOutputHelper,
            LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose,
            string outputTemplate = DefaultOutputTemplate,
            IFormatProvider formatProvider = null,
            LoggingLevelSwitch levelSwitch = null)
        {
            if (sinkConfiguration == null)
                throw new ArgumentNullException(nameof(sinkConfiguration));

   
            return sinkConfiguration.Sink(
                new XUnitLogEventSink(testOutputHelper, outputTemplate, formatProvider),
                restrictedToMinimumLevel, levelSwitch);
        }

        /// <summary>
        /// Writes log events to <see cref="T:System.Console" />.
        /// </summary>
        /// <param name="sinkConfiguration">Logger sink configuration.</param>
        /// <param name="testOutputHelper"></param>
        /// <param name="formatter">Controls the rendering of log events into text, for example to log JSON. To
        /// control plain text formatting, use the overload that accepts an output template.</param>
        /// <param name="restrictedToMinimumLevel">The minimum level for
        /// events passed through the sink. Ignored when <paramref name="levelSwitch" /> is specified.</param>
        /// <param name="levelSwitch">A switch allowing the pass-through minimum level
        /// to be changed at runtime.</param>
        /// <returns>Configuration object allowing method chaining.</returns>
        public static LoggerConfiguration XUnit(this LoggerSinkConfiguration sinkConfiguration,
            ITestOutputHelper testOutputHelper,
            ITextFormatter formatter,
            LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose,
            LoggingLevelSwitch levelSwitch = null)
        {
            if (sinkConfiguration == null)
                throw new ArgumentNullException(nameof(sinkConfiguration));

            return sinkConfiguration.Sink(
                new XUnitLogEventSink(testOutputHelper, formatter),
                restrictedToMinimumLevel, levelSwitch);
        }
    }
}
