using Microsoft.Extensions.Logging;
using PureActive.Logger.Provider.Serilog.Configuration;
using PureActive.Logger.Provider.Serilog.Settings;
using PureActive.Logging.Abstractions.Types;
using Serilog.Events;
using Xunit;

namespace PureActive.Logger.Provider.Serilog.UnitTests
{
    public class SerilogProviderUnitTests
    {
        [Fact]
        public void TestCreateLogger()
        {
            var loggerSettings = new SerilogLoggerSettings(LogEventLevel.Debug, LoggingOutputFlags.Testing);
            var loggerFactory = LoggerConfigurationFactory.CreatePureSeriLoggerFactory(loggerSettings);

            var logger = loggerFactory.CreateLogger<SerilogProviderUnitTests>();

            logger.LogDebug("Test");
        }
    }
}
