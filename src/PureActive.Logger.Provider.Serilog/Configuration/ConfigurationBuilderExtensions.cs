using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PureActive.Logger.Provider.Serilog.Types;
using Serilog.Events;

namespace PureActive.Logger.Provider.Serilog.Configuration
{
    public static class ConfigurationBuilderExtensions
    {
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