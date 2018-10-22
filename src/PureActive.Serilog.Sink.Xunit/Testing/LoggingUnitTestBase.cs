using Microsoft.Extensions.Logging;
using PureActive.Logger.Provider.Serilog.Interfaces;
using PureActive.Logger.Provider.Serilog.Settings;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Logging.Extensions.Logging;
using Xunit.Abstractions;

namespace PureActive.Serilog.Sink.Xunit.Testing
{
    public abstract class LoggingUnitTestBase<T>
    {
        protected readonly IPureLoggerFactory LoggerFactory;
        protected readonly ISerilogLoggerSettings LoggerSettings;
        protected readonly IPureLogger Logger;
        protected readonly ITestOutputHelper TestOutputHelper;

        protected LoggingUnitTestBase(ITestOutputHelper testOutputHelper, LogLevel initialMinimumLevel = LogLevel.Debug, 
            XunitLoggingSink.XUnitSerilogFormatter xUnitSerilogFormatter = XunitLoggingSink.XUnitSerilogFormatter.RenderedCompactJsonFormatter)
        {
            TestOutputHelper = testOutputHelper;
            LoggerSettings = new SerilogLoggerSettings(initialMinimumLevel);

            var loggerConfiguration =
                XunitLoggingSink.CreateXUnitLoggerConfiguration(testOutputHelper, LoggerSettings,
                    xUnitSerilogFormatter);

            LoggerFactory = XunitLoggingSink.CreateXUnitSerilogFactory(LoggerSettings, loggerConfiguration);

            Logger = new PureLogger(LoggerFactory?.CreateLogger<T>());
        }
    }
}
