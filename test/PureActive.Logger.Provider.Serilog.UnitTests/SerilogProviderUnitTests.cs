using System;
using Microsoft.Extensions.Logging;
using PureActive.Core.System;
using PureActive.Logger.Provider.Serilog.Configuration;
using PureActive.Logger.Provider.Serilog.Settings;
using PureActive.Logging.Abstractions.Types;
using Serilog.Events;
using Xunit;
using OperatingSystem = PureActive.Core.System.OperatingSystem;

namespace PureActive.Logger.Provider.Serilog.UnitTests
{
    public class SerilogProviderUnitTests
    {
        [Fact]
        public void SerilogProvider_CreateLogger_AppConsoleFile()
        {
            var logFileName = "SerilogProviderUnitTests.log";
            var fileSystem = new FileSystem(typeof(SerilogProviderUnitTests));

            var loggerSettings = new SerilogLoggerSettings(fileSystem, LogEventLevel.Debug, LoggingOutputFlags.AppConsoleFile);
            var loggerConfiguration =
                LoggerConfigurationFactory.CreateLoggerConfiguration((string)null, logFileName, loggerSettings, b => true);

            var loggerFactory = LoggerConfigurationFactory.CreatePureSeriLoggerFactory(loggerSettings, loggerConfiguration);

            var logger = loggerFactory.CreateLogger<SerilogProviderUnitTests>();

            logger.LogDebug("Test");
        }
    }
}
