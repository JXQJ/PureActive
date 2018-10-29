using Microsoft.Extensions.Logging;
using PureActive.Logger.Provider.Serilog.Interfaces;
using PureActive.Logger.Provider.Serilog.Settings;
using PureActive.Logger.Provider.Serilog.Types;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Logging.Abstractions.Types;
using PureActive.Serilog.Sink.Xunit.Interfaces;
using PureActive.Serilog.Sink.Xunit.Sink;
using Xunit.Abstractions;

namespace PureActive.Serilog.Sink.Xunit.TestBase
{
    public abstract class LoggingUnitTestBase<T>
    {
        protected readonly IPureTestLoggerFactory TestLoggerFactory;
        protected readonly ISerilogLoggerSettings LoggerSettings;
        protected readonly IPureLogger Logger;
        protected readonly ITestOutputHelper TestOutputHelper;

        protected LoggingUnitTestBase(ITestOutputHelper testOutputHelper, LogLevel initialMinimumLevel = LogLevel.Debug, 
            XUnitSerilogFormatter xUnitSerilogFormatter = XUnitSerilogFormatter.RenderedCompactJsonFormatter)
        {
            TestOutputHelper = testOutputHelper;
            LoggerSettings = new SerilogLoggerSettings(initialMinimumLevel, LoggingOutputFlags.Testing);

            var loggerConfiguration =
                XunitLoggingSink.CreateXUnitLoggerConfiguration(testOutputHelper, LoggerSettings,
                    xUnitSerilogFormatter);

            TestLoggerFactory = XunitLoggingSink.CreateXUnitSerilogFactory(LoggerSettings, loggerConfiguration);

            Logger = TestLoggerFactory?.CreatePureLogger<T>();
        }
    }
}
