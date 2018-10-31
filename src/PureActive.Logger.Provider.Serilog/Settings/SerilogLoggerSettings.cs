using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PureActive.Core.Abstractions.System;
using PureActive.Logger.Provider.Serilog.Configuration;
using PureActive.Logger.Provider.Serilog.Interfaces;
using PureActive.Logger.Provider.Serilog.Types;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Logging.Abstractions.Types;
using Serilog.Events;

namespace PureActive.Logger.Provider.Serilog.Settings
{
    public class SerilogLoggerSettings: ISerilogLoggerSettings
    {
        public LoggingOutputFlags LoggingOutputFlags { get; set; }

        public IConfiguration Configuration { get; }
        private readonly IFileSystem _fileSystem;

        private readonly Dictionary<string, ISerilogLogLevel> _serilogLogLevels = new Dictionary<string, ISerilogLogLevel>();
        private readonly object _objectLock = new object();

        public SerilogLoggerSettings(IFileSystem fileSystem, LogEventLevel defaultLogEventLevel, LoggingOutputFlags loggingOutputFlags)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            LoggingOutputFlags = loggingOutputFlags;
            Configuration = DefaultLoggerSettingsConfiguration(defaultLogEventLevel);

            RegisterLogLevel(LoggingOutputFlags.Default, defaultLogEventLevel);


            if (loggingOutputFlags.HasFlag(LoggingOutputFlags.Console))
            {
                RegisterLogLevel(LoggingOutputFlags.Console, defaultLogEventLevel);
            }

            if (loggingOutputFlags.HasFlag(LoggingOutputFlags.AppInsights))
            {
                RegisterLogLevel(LoggingOutputFlags.AppInsights, LogEventLevel.Information);
            }

            if (loggingOutputFlags.HasFlag(LoggingOutputFlags.RollingFile))
            {
                RegisterLogLevel(LoggingOutputFlags.RollingFile, defaultLogEventLevel);
            }

            if (loggingOutputFlags.HasFlag(LoggingOutputFlags.TestCorrelator))
            {
                RegisterLogLevel(LoggingOutputFlags.TestCorrelator, defaultLogEventLevel);
            }

            if (loggingOutputFlags.HasFlag(LoggingOutputFlags.XUnitConsole))
            {
                RegisterLogLevel(LoggingOutputFlags.XUnitConsole, defaultLogEventLevel);
            }
        }

        public SerilogLoggerSettings(IFileSystem fileSystem, LogLevel defaultLogLevel, LoggingOutputFlags loggingOutputFlags) : 
            this(fileSystem, SerilogLogLevel.MsftToSerilogLogLevel(defaultLogLevel), loggingOutputFlags)
        {

        }

        public SerilogLoggerSettings(IFileSystem fileSystem, IConfiguration configuration, LoggingOutputFlags loggingOutputFlags)
            : this(fileSystem, ParseConfigurationLogLevel(configuration), loggingOutputFlags)
        {
            Configuration = configuration ?? throw new ArgumentException(nameof(configuration));
        }

        private static LogEventLevel ParseConfigurationLogLevel(IConfiguration configuration)
        {
            var minimumLevelString = configuration.GetSection("Serilog:MinimumLevel")?["Default"];

            var minimumLogEventLevel = LogEventLevel.Information;

            if (minimumLevelString != null)
            {
                Enum.TryParse(minimumLevelString, true, out minimumLogEventLevel);
            }

            return minimumLogEventLevel;
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
                    serilogLogLevel.MinimumLogEventLevel = logEventLevel;
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

        public IPureLogLevel GetOrRegisterDefaultLogLevel(string key) => GetOrRegisterSerilogLogLevel(key, GetLogLevel(LoggingOutputFlags.Default).MinimumLogLevel);
        public ISerilogLogLevel GetOrRegisterSerilogLogDefaultLevel(string key) =>
            GetOrRegisterSerilogLogLevel(key, GetLogLevel(LoggingOutputFlags.Default).MinimumLogLevel);

        public IPureLogLevel RegisterLogLevel(LoggingOutputFlags loggingOutputFlag, LogLevel logLevel) =>
            RegisterLogLevel(loggingOutputFlag.ToString(), logLevel);

        public IPureLogLevel GetLogLevel(LoggingOutputFlags loggingOutputFlag) =>
            GetLogLevel(loggingOutputFlag.ToString());

        public IPureLogLevel GetOrRegisterLogLevel(LoggingOutputFlags loggingOutputFlag, LogLevel logLevel) =>
            GetOrRegisterLogLevel(loggingOutputFlag.ToString(), logLevel);

        public IPureLogLevel GetOrRegisterDefaultLogLevel(LoggingOutputFlags loggingOutputFlag) =>
            GetOrRegisterDefaultLogLevel(loggingOutputFlag.ToString());

        public string LogFolderPath => _fileSystem?.LogFolderPath();
   
        public IPureLogLevel RegisterLogLevel(LoggingOutputFlags loggingOutputFlag, LogEventLevel logEventLevel) =>
            RegisterLogLevel(loggingOutputFlag.ToString(), logEventLevel);

        public ISerilogLogLevel GetSerilogLogLevel(LoggingOutputFlags loggingOutputFlag) =>
            GetSerilogLogLevel(loggingOutputFlag.ToString());

        public ISerilogLogLevel RegisterSerilogLogLevel(LoggingOutputFlags loggingOutputFlag,
            LogEventLevel logEventLevel) => RegisterSerilogLogLevel(loggingOutputFlag.ToString(), logEventLevel);

        public ISerilogLogLevel RegisterSerilogLogLevel(LoggingOutputFlags loggingOutputFlag, LogLevel logLevel) =>
            RegisterSerilogLogLevel(loggingOutputFlag.ToString(), logLevel);

        public ISerilogLogLevel GetOrRegisterSerilogLogLevel(LoggingOutputFlags loggingOutputFlag, LogLevel logLevel) =>
            GetOrRegisterSerilogLogLevel(loggingOutputFlag.ToString(), logLevel);

        public ISerilogLogLevel GetOrRegisterSerilogLogLevel(LoggingOutputFlags loggingOutputFlag,
            LogEventLevel logEventLevel) => GetOrRegisterSerilogLogLevel(loggingOutputFlag.ToString(), logEventLevel);

        public ISerilogLogLevel GetOrRegisterSerilogLogDefaultLevel(LoggingOutputFlags loggingOutputFlag) =>
            GetOrRegisterSerilogLogDefaultLevel(loggingOutputFlag.ToString());
    }
}
