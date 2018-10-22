using Microsoft.Extensions.Logging;
using PureActive.Logger.Provider.Serilog.Interfaces;
using Serilog.Core;
using Serilog.Events;

namespace PureActive.Logger.Provider.Serilog.Types
{
    public class PureLogProviderSettings : IPureLogProviderSettings
    {
        public LoggingLevelSwitch LoggingLevelSwitch { get; internal set; }

        public LogEventLevel MinimumLevel
        {
            get => LoggingLevelSwitch.MinimumLevel;
            set => LoggingLevelSwitch.MinimumLevel = value;
        }

        public LogLevel MinimumLogLevel
        {
            get => LogEventLevelToLogLevel(LoggingLevelSwitch.MinimumLevel);
            set => LoggingLevelSwitch.MinimumLevel = LogLevelToLogEventLevel(value);
        }

        public PureLogProviderSettings(LogEventLevel minimumLevel)
        {
            LoggingLevelSwitch = new LoggingLevelSwitch(minimumLevel);
        }

        /*
         * public enum LogEventLevel
           {
           /// <summary>
           /// Anything and everything you might want to know about
           /// a running block of code.
           /// </summary>
           Verbose,
           /// <summary>
           /// Internal system events that aren't necessarily
           /// observable from the outside.
           /// </summary>
           Debug,
           /// <summary>
           /// The lifeblood of operational intelligence - things
           /// happen.
           /// </summary>
           Information,
           /// <summary>Service is degraded or endangered.</summary>
           Warning,
           /// <summary>
           /// Functionality is unavailable, invariants are broken
           /// or data is lost.
           /// </summary>
           Error,
           /// <summary>
           /// If you have a pager, it goes off when one of these
           /// occurs.
           /// </summary>
           Fatal,
           }
           
         */

        public static LogEventLevel LogLevelToLogEventLevel(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Critical:
                    return LogEventLevel.Fatal;

                case LogLevel.Error:
                    return LogEventLevel.Error;

                case LogLevel.Warning:
                    return LogEventLevel.Warning;

                case LogLevel.Information:
                    return LogEventLevel.Information;
                case LogLevel.Debug:
                    return LogEventLevel.Debug;

                // ReSharper disable once RedundantCaseLabel
                case LogLevel.Trace:
                default:
                    return LogEventLevel.Verbose;
            }
        }

        public static LogLevel LogEventLevelToLogLevel(LogEventLevel logEventLevel)
        {
            switch (logEventLevel)
            {
                case LogEventLevel.Debug:
                    return LogLevel.Debug;

                case LogEventLevel.Information:
                    return LogLevel.Information;

                case LogEventLevel.Warning:
                    return LogLevel.Warning;

                case LogEventLevel.Error:
                    return LogLevel.Error;

                case LogEventLevel.Fatal:
                    return LogLevel.Critical;

                // ReSharper disable once RedundantCaseLabel
                case LogEventLevel.Verbose:
                default:
                    return LogLevel.Trace;
            }
        }
    }
}