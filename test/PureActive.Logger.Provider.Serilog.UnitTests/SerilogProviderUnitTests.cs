using Microsoft.Extensions.Logging;
using PureActive.Logger.Provider.Serilog.Configuration;
using PureActive.Logger.Provider.Serilog.Settings;
using Serilog.Events;
using Xunit;

namespace PureActive.Logger.Provider.Serilog.UnitTests
{
    public class SerilogProviderUnitTests
    {
        [Fact]
        public void TestCreateLogger()
        {
            var loggerSettings = new PureLoggerSettings(LogEventLevel.Debug);

            var loggerFactory = LoggerConfigurationFactory.CreateSerilogFactory(loggerSettings);

            var logger = loggerFactory.CreateLogger<SerilogProviderUnitTests>();

            logger.LogDebug("Test");
        }
    }
}
