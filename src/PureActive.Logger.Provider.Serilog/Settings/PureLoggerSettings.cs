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
    public class PureLoggerSettings: IPureLoggerSettings
    {
        public IPureLogProviderSettings Default { get; internal set; }
        public IPureLogProviderSettings File { get; }
        public IPureLogProviderSettings Console { get; }
        public IPureLogProviderSettings Test { get; }
        public IPureLogProviderSettings AppInsights { get; }

        public IConfiguration Configuration { get; }

        
        public PureLoggerSettings(LogEventLevel initialMinimumLogEventLevelDefault, LogEventLevel initialMinimumLogEventLevelFile, 
            LogEventLevel initialMinimumLogEventLevelConsole, LogEventLevel initialMinimumLogEventLevelTest, LogEventLevel initialMinimumLogEventLevelAppInsights)
        {
            Default = new PureLogProviderSettings(initialMinimumLogEventLevelDefault);
            File = new PureLogProviderSettings(initialMinimumLogEventLevelFile);
            Console = new PureLogProviderSettings(initialMinimumLogEventLevelConsole);
            Test = new PureLogProviderSettings(initialMinimumLogEventLevelTest);

            // TODO: Read AppInsights from settings
            AppInsights = new PureLogProviderSettings(initialMinimumLogEventLevelAppInsights);

            Configuration = DefaultLoggerSettingsConfiguration(initialMinimumLogEventLevelDefault);
        }

        public PureLoggerSettings(LogLevel initialMinimumLogLevelDefault, LogLevel initialMinimumLogLevelFile, LogLevel initialMinimumLogLevelConsole, 
            LogLevel initialMinimumLogLevelTest, LogLevel initialMinimumLogLevelAppInsights) :
            this(PureLogProviderSettings.LogLevelToLogEventLevel(initialMinimumLogLevelDefault), PureLogProviderSettings.LogLevelToLogEventLevel(initialMinimumLogLevelFile),
                PureLogProviderSettings.LogLevelToLogEventLevel(initialMinimumLogLevelConsole), PureLogProviderSettings.LogLevelToLogEventLevel(initialMinimumLogLevelTest),
                PureLogProviderSettings.LogLevelToLogEventLevel(initialMinimumLogLevelAppInsights))
        {

        }

        public PureLoggerSettings(LogEventLevel initialMinimumLogEventLevelDefault):
            this(initialMinimumLogEventLevelDefault, initialMinimumLogEventLevelDefault, initialMinimumLogEventLevelDefault, initialMinimumLogEventLevelDefault, LogEventLevel.Information)
        {

        }

        public PureLoggerSettings(LogLevel initialMinimumLogLevelDefault) :
            this(PureLogProviderSettings.LogLevelToLogEventLevel(initialMinimumLogLevelDefault))
        { 
    
        }
        
        public PureLoggerSettings() : this(LogEventLevel.Information)
        {

        }

        public PureLoggerSettings(IConfiguration configuration)
        {
            Configuration = configuration ?? throw new ArgumentException(nameof(configuration));

            var minimumLevelString = configuration.GetSection("Serilog:MinimumLevel")?["Default"];

            var initialMinimumLevel = LogEventLevel.Information;

            if (minimumLevelString != null)
            {
                Enum.TryParse(minimumLevelString, true, out initialMinimumLevel);
            }

            Default = new PureLogProviderSettings(initialMinimumLevel);
            File = new PureLogProviderSettings(initialMinimumLevel);
            Console = new PureLogProviderSettings(initialMinimumLevel);
            Test = new PureLogProviderSettings(initialMinimumLevel);

            // TODO: Read AppInsights from settings
            AppInsights = new PureLogProviderSettings(LogEventLevel.Information);
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
            return DefaultLoggerSettingsConfiguration(PureLogProviderSettings.LogLevelToLogEventLevel(initialMinimumLevel));
        }
    }
}
