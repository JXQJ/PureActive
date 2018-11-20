using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PureActive.Core.System;
using PureActive.Logger.Provider.Serilog.Types;
using Serilog.Events;

namespace PureActive.Hosting.Configuration
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddAppSettings(this IConfigurationBuilder config, string environmentName)
        {
            var fileSystem = new FileSystem();
            var settingsDirectory = fileSystem.SettingsFolder;

            config
                .AddJsonFile(settingsDirectory + "sharedsettings.json", false, true)
                .AddJsonFile(settingsDirectory + "appsettings.json", false, true)
                .AddJsonFile(settingsDirectory + $"appsettings.{environmentName}.json", true, true);

            return config;
        }

        public static IConfigurationBuilder AddAppSettings(this IConfigurationBuilder config,
            IHostingEnvironment hostingEnvironment)
        {
            return AddAppSettings(config, hostingEnvironment.EnvironmentName);
        }

        public static IConfigurationBuilder AddAppSettings(this IConfigurationBuilder config,
            Microsoft.Extensions.Hosting.IHostingEnvironment hostingEnvironment)
        {
            return AddAppSettings(config, hostingEnvironment.EnvironmentName);
        }

        public static IConfigurationBuilder AddAppSettings(this IConfigurationBuilder config)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (string.IsNullOrEmpty(environmentName))
                environmentName = EnvironmentName.Development;

            return AddAppSettings(config, environmentName);
        }

        //"Serilog": {
        //    "MinimumLevel": {
        //        "Default": "Debug",
        //        "Override": {
        //            "Microsoft": "Information",
        //            "System": "Warning"
        //        }
        //    }

        public static IConfigurationBuilder AddLoggerSettings(this IConfigurationBuilder config,
            LogEventLevel initialMinimumLevel)
        {
            config.AddInMemoryCollection(
                new Dictionary<string, string>
                {
                    ["Serilog:MinimumLevel:Default"] = initialMinimumLevel.ToString(),
                    ["Serilog:MinimumLevel:Override:Microsoft"] = "Information",
                    ["Serilog:MinimumLevel:Override:System"] = "Warning"
                }
            );

            return config;
        }

        public static IConfigurationBuilder AddLoggerSettings(this IConfigurationBuilder config,
            LogLevel initialMinimumLevel) =>
            AddLoggerSettings(config, SerilogLogLevel.MsftToSerilogLogLevel(initialMinimumLevel));
    }
}