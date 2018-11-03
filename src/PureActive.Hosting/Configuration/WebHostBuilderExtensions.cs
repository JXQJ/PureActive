using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PureActive.Core.Abstractions.System;
using PureActive.Core.System;
using PureActive.Logger.Provider.Serilog.Configuration;
using PureActive.Logger.Provider.Serilog.Interfaces;
using PureActive.Logger.Provider.Serilog.Settings;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Logging.Abstractions.Types;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using OperatingSystem = PureActive.Core.System.OperatingSystem;

namespace PureActive.Hosting.Configuration
{
    public static class WebHostBuilderExtensions
    {
        /// <summary>
        ///     Returns the configuration for the webapp.
        /// </summary>
        private static IConfigurationRoot GetAppConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddAppSettings()
                .AddEnvironmentVariables()
                .Build();
        }


        public static IWebHostBuilder UseSystemSettings(this IWebHostBuilder webHostBuilder, string logFileName, Func<LogEvent, bool> includeLogEvent)
        {
            var operatingSystem = new OperatingSystem();
            var appConfiguration = GetAppConfiguration();
            var fileSystem = new FileSystem(appConfiguration, operatingSystem);

            var loggerSettings = new SerilogLoggerSettings(fileSystem, appConfiguration, LoggingOutputFlags.AppFull);
            var loggerConfiguration = LoggerConfigurationFactory.CreateLoggerConfiguration(appConfiguration, logFileName, loggerSettings, includeLogEvent);
            var loggerFactory = LoggerConfigurationFactory.CreatePureSeriLoggerFactory(loggerSettings, loggerConfiguration);

            webHostBuilder.ConfigureServices(services =>
                services.AddSingleton<IPureLoggerFactory>(loggerFactory));

            webHostBuilder.ConfigureServices(services =>
                services.AddSingleton<ILoggerFactory>(loggerFactory));

            webHostBuilder.ConfigureServices(services =>
                services.AddSingleton<IOperatingSystem>(operatingSystem));

            webHostBuilder.ConfigureServices(services =>
                services.AddSingleton<IFileSystem>(fileSystem));

            return webHostBuilder;
        }
    }
}