using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PureActive.Logger.Provider.Serilog.Configuration;
using PureActive.Logger.Provider.Serilog.Interfaces;
using PureActive.Logger.Provider.Serilog.Types;
using PureActive.Logging.Abstractions.Interfaces;
using Serilog.Events;

namespace PureActive.Logger.Provider.Serilog.Settings
{
    public class SerilogLoggerSettings: ISerilogLoggerSettings
    {
        public IConfiguration Configuration { get; }

        public LogEventLevel DefaultLogEventLevel { get; set; }

        public LogLevel DefaultLogLevel
        {
            get => SerilogLogLevel.SerilogToMsftLogLevel(DefaultLogEventLevel);
            set => DefaultLogEventLevel = SerilogLogLevel.MsftToSerilogLogLevel(value);
        }

        private readonly Dictionary<string, ISerilogLogLevel> _serilogLogLevels = new Dictionary<string, ISerilogLogLevel>();
        private readonly object _objectLock = new object();

        public SerilogLoggerSettings(LogEventLevel defaultLogEventLevel)
        {
            DefaultLogEventLevel = defaultLogEventLevel;
            Configuration = DefaultLoggerSettingsConfiguration(defaultLogEventLevel);
        }

        public SerilogLoggerSettings(LogLevel defaultLogLevel) : 
            this(SerilogLogLevel.MsftToSerilogLogLevel(defaultLogLevel))
        {

        }

        public SerilogLoggerSettings(IConfiguration configuration)
        {
            Configuration = configuration ?? throw new ArgumentException(nameof(configuration));

            var minimumLevelString = configuration.GetSection("Serilog:MinimumLevel")?["Default"];

            var minimumLogEventLevel = LogEventLevel.Information;

            if (minimumLevelString != null)
            {
                Enum.TryParse(minimumLevelString, true, out minimumLogEventLevel);
            }

            DefaultLogEventLevel = minimumLogEventLevel;

            // TODO: Figure out which loggers to register from configuration
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

        public ISerilogLogLevel RegisterSerilogLogLevel(string key, LogEventLevel logEventLevel)
        {
            lock (_objectLock)
            {
                if (_serilogLogLevels.TryGetValue(key, out var serilogLogLevel))
                {
                    serilogLogLevel.MinimumLevel = logEventLevel;
                }
                else
                {
                    serilogLogLevel = new SerilogLogLevel(logEventLevel);
                    _serilogLogLevels.Add(key, serilogLogLevel);
                }

                return serilogLogLevel;
            }
        }

        public ISerilogLogLevel RegisterSerilogLogLevel(string key, LogLevel logLevel) =>
            RegisterSerilogLogLevel(key, SerilogLogLevel.MsftToSerilogLogLevel(logLevel));

  

        public IPureLogLevel RegisterLogLevel(string key, LogLevel logLevel) => RegisterSerilogLogLevel(key, logLevel);
        public IPureLogLevel RegisterLogLevel(string key, LogEventLevel logEventLevel) => RegisterSerilogLogLevel(key, logEventLevel);

        public ISerilogLogLevel GetSerilogLogLevel(string key)
        {
            lock (_objectLock)
            {
                return _serilogLogLevels[key];
            }
        }
        public IPureLogLevel GetLogLevel(string key) => GetSerilogLogLevel(key);

        public ISerilogLogLevel GetOrRegisterSerilogLogLevel(string key, LogEventLevel logEventLevel)
        {
            lock (_objectLock)
            {
                if (!_serilogLogLevels.TryGetValue(key, out var serilogLogLevel))
                {
                    serilogLogLevel = new SerilogLogLevel(logEventLevel);
                    _serilogLogLevels.Add(key, serilogLogLevel);
                }

                return serilogLogLevel;
            }
        }

        public ISerilogLogLevel GetOrRegisterSerilogLogLevel(string key, LogLevel logLevel) => 
            GetOrRegisterSerilogLogLevel(key, SerilogLogLevel.MsftToSerilogLogLevel(logLevel));
     
        public IPureLogLevel GetOrRegisterLogLevel(string key, LogLevel logLevel) =>
            GetOrRegisterSerilogLogLevel(key, logLevel);

        public IPureLogLevel GetOrRegisterDefaultLogLevel(string key) => GetOrRegisterSerilogLogLevel(key, DefaultLogEventLevel);
  
        public ISerilogLogLevel GetOrRegisterSerilogLogDefaultLevel(string key) => 
            GetOrRegisterSerilogLogLevel(key, DefaultLogEventLevel);
    }
}
