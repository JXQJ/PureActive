using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PureActive.Logger.Provider.Serilog.Configuration;
using PureActive.Logger.Provider.Serilog.Interfaces;
using PureActive.Logger.Provider.Serilog.Types;
using Serilog.Events;

namespace PureActive.Logger.Provider.Serilog.Settings
{
    public class SerilogLoggerSettings: ISerilogLoggerSettings
    {
        public ISerilogLogLevel Default { get; internal set; }
        public ISerilogLogLevel File { get; }
        public ISerilogLogLevel Console { get; }
        public ISerilogLogLevel TestConsole { get; }
        public ISerilogLogLevel TestCorrelator { get; }
        public ISerilogLogLevel AppInsights { get; }

        public IConfiguration Configuration { get; }

        
        public SerilogLoggerSettings(LogEventLevel initialMinimumLogEventLevelDefault, LogEventLevel initialMinimumLogEventLevelFile, 
            LogEventLevel initialMinimumLogEventLevelConsole, LogEventLevel initialMinimumLogEventLevelTestConsole, LogEventLevel initialMinimumLogEventLevelTestCorrelator,
            LogEventLevel initialMinimumLogEventLevelAppInsights)
        {
            Default = new SerilogLogLevel(initialMinimumLogEventLevelDefault);
            File = new SerilogLogLevel(initialMinimumLogEventLevelFile);
            Console = new SerilogLogLevel(initialMinimumLogEventLevelConsole);
            TestConsole = new SerilogLogLevel(initialMinimumLogEventLevelTestConsole);
            TestCorrelator = new SerilogLogLevel(initialMinimumLogEventLevelTestCorrelator);

            // TODO: Read AppInsights from settings
            AppInsights = new SerilogLogLevel(initialMinimumLogEventLevelAppInsights);

            Configuration = DefaultLoggerSettingsConfiguration(initialMinimumLogEventLevelDefault);
        }

        public SerilogLoggerSettings(LogLevel initialMinimumLogLevelDefault, LogLevel initialMinimumLogLevelFile, LogLevel initialMinimumLogLevelConsole, 
            LogLevel initialMinimumLogLevelTestConsole, LogLevel initialMinimumLogLevelTestCorrelator, LogLevel initialMinimumLogLevelAppInsights) :
            this(SerilogLogLevel.MsftToSerilogLogLevel(initialMinimumLogLevelDefault), 
                SerilogLogLevel.MsftToSerilogLogLevel(initialMinimumLogLevelFile),
                SerilogLogLevel.MsftToSerilogLogLevel(initialMinimumLogLevelConsole), 
                SerilogLogLevel.MsftToSerilogLogLevel(initialMinimumLogLevelTestConsole),
                SerilogLogLevel.MsftToSerilogLogLevel(initialMinimumLogLevelTestCorrelator),
                SerilogLogLevel.MsftToSerilogLogLevel(initialMinimumLogLevelAppInsights))
        {

        }

        public SerilogLoggerSettings(LogEventLevel initialMinimumLogEventLevelDefault):
            this(initialMinimumLogEventLevelDefault, initialMinimumLogEventLevelDefault, initialMinimumLogEventLevelDefault, 
                initialMinimumLogEventLevelDefault, initialMinimumLogEventLevelDefault, LogEventLevel.Information)
        {

        }

        public SerilogLoggerSettings(LogLevel initialMinimumLogLevelDefault) :
            this(SerilogLogLevel.MsftToSerilogLogLevel(initialMinimumLogLevelDefault))
        { 
    
        }
        
        public SerilogLoggerSettings() : this(LogEventLevel.Information)
        {

        }

        public SerilogLoggerSettings(IConfiguration configuration)
        {
            Configuration = configuration ?? throw new ArgumentException(nameof(configuration));

            var minimumLevelString = configuration.GetSection("Serilog:MinimumLevel")?["Default"];

            var initialMinimumLevel = LogEventLevel.Information;

            if (minimumLevelString != null)
            {
                Enum.TryParse(minimumLevelString, true, out initialMinimumLevel);
            }

            Default = new SerilogLogLevel(initialMinimumLevel);
            File = new SerilogLogLevel(initialMinimumLevel);
            Console = new SerilogLogLevel(initialMinimumLevel);
            TestConsole = new SerilogLogLevel(initialMinimumLevel);
            TestCorrelator = new SerilogLogLevel(initialMinimumLevel);

            // TODO: Read AppInsights from settings
            AppInsights = new SerilogLogLevel(LogEventLevel.Information);
        }

        public static IConfiguration DefaultLoggerSettingsConfiguration(LogEventLevel initialMinimumLevel)
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddLoggerSettings(initialMinimumLevel)
                .AddEnvironmentVariables()
                .Build();
        }

        public static IConfiguration DefaultLoggerSettingsConfiguration(LogLevel initialMinimumLevel)
        {
            return DefaultLoggerSettingsConfiguration(SerilogLogLevel.MsftToSerilogLogLevel(initialMinimumLevel));
        }
    }
}
