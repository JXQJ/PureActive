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
        public ISerilogLogProviderSettings Default { get; internal set; }
        public ISerilogLogProviderSettings File { get; }
        public ISerilogLogProviderSettings Console { get; }
        public ISerilogLogProviderSettings TestConsole { get; }
        public ISerilogLogProviderSettings TestCorrelator { get; }
        public ISerilogLogProviderSettings AppInsights { get; }

        public IConfiguration Configuration { get; }

        
        public SerilogLoggerSettings(LogEventLevel initialMinimumLogEventLevelDefault, LogEventLevel initialMinimumLogEventLevelFile, 
            LogEventLevel initialMinimumLogEventLevelConsole, LogEventLevel initialMinimumLogEventLevelTestConsole, LogEventLevel initialMinimumLogEventLevelTestCorrelator,
            LogEventLevel initialMinimumLogEventLevelAppInsights)
        {
            Default = new SerilogLogProviderSettings(initialMinimumLogEventLevelDefault);
            File = new SerilogLogProviderSettings(initialMinimumLogEventLevelFile);
            Console = new SerilogLogProviderSettings(initialMinimumLogEventLevelConsole);
            TestConsole = new SerilogLogProviderSettings(initialMinimumLogEventLevelTestConsole);
            TestCorrelator = new SerilogLogProviderSettings(initialMinimumLogEventLevelTestCorrelator);

            // TODO: Read AppInsights from settings
            AppInsights = new SerilogLogProviderSettings(initialMinimumLogEventLevelAppInsights);

            Configuration = DefaultLoggerSettingsConfiguration(initialMinimumLogEventLevelDefault);
        }

        public SerilogLoggerSettings(LogLevel initialMinimumLogLevelDefault, LogLevel initialMinimumLogLevelFile, LogLevel initialMinimumLogLevelConsole, 
            LogLevel initialMinimumLogLevelTestConsole, LogLevel initialMinimumLogLevelTestCorrelator, LogLevel initialMinimumLogLevelAppInsights) :
            this(SerilogLogProviderSettings.LogLevelToLogEventLevel(initialMinimumLogLevelDefault), 
                SerilogLogProviderSettings.LogLevelToLogEventLevel(initialMinimumLogLevelFile),
                SerilogLogProviderSettings.LogLevelToLogEventLevel(initialMinimumLogLevelConsole), 
                SerilogLogProviderSettings.LogLevelToLogEventLevel(initialMinimumLogLevelTestConsole),
                SerilogLogProviderSettings.LogLevelToLogEventLevel(initialMinimumLogLevelTestCorrelator),
                SerilogLogProviderSettings.LogLevelToLogEventLevel(initialMinimumLogLevelAppInsights))
        {

        }

        public SerilogLoggerSettings(LogEventLevel initialMinimumLogEventLevelDefault):
            this(initialMinimumLogEventLevelDefault, initialMinimumLogEventLevelDefault, initialMinimumLogEventLevelDefault, 
                initialMinimumLogEventLevelDefault, initialMinimumLogEventLevelDefault, LogEventLevel.Information)
        {

        }

        public SerilogLoggerSettings(LogLevel initialMinimumLogLevelDefault) :
            this(SerilogLogProviderSettings.LogLevelToLogEventLevel(initialMinimumLogLevelDefault))
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

            Default = new SerilogLogProviderSettings(initialMinimumLevel);
            File = new SerilogLogProviderSettings(initialMinimumLevel);
            Console = new SerilogLogProviderSettings(initialMinimumLevel);
            TestConsole = new SerilogLogProviderSettings(initialMinimumLevel);
            TestCorrelator = new SerilogLogProviderSettings(initialMinimumLevel);

            // TODO: Read AppInsights from settings
            AppInsights = new SerilogLogProviderSettings(LogEventLevel.Information);
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
            return DefaultLoggerSettingsConfiguration(SerilogLogProviderSettings.LogLevelToLogEventLevel(initialMinimumLevel));
        }
    }
}
