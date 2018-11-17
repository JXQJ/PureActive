using Microsoft.Extensions.Logging;
using PureActive.Core.Abstractions.System;
using PureActive.Core.System;
using PureActive.Logger.Provider.Serilog.Interfaces;
using PureActive.Logger.Provider.Serilog.Settings;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Logging.Abstractions.Types;
using PureActive.Serilog.Sink.Xunit.Interfaces;
using PureActive.Serilog.Sink.Xunit.Sink;
using Xunit.Abstractions;

namespace PureActive.Serilog.Sink.Xunit.TestBase
{
    public abstract class TestLoggerBase<T>
    {
        protected readonly IPureTestLoggerFactory TestLoggerFactory;
        protected readonly ISerilogLoggerSettings LoggerSettings;
        protected readonly IPureLogger Logger;
        protected readonly ITestOutputHelper TestOutputHelper;

        protected TestLoggerBase(IFileSystem fileSystem, ITestOutputHelper testOutputHelper,
            LogLevel initialMinimumLevel = LogLevel.Debug,
            XUnitSerilogFormatter xUnitSerilogFormatter = XUnitSerilogFormatter.RenderedCompactJsonFormatter)
        {
            TestOutputHelper = testOutputHelper;
            LoggerSettings = new SerilogLoggerSettings(fileSystem, initialMinimumLevel, LoggingOutputFlags.Testing);

            var loggerConfiguration =
                XunitLoggingSink.CreateXUnitLoggerConfiguration(testOutputHelper, LoggerSettings,
                    xUnitSerilogFormatter);

            TestLoggerFactory = XunitLoggingSink.CreateXUnitSerilogFactory(LoggerSettings, loggerConfiguration);

            Logger = TestLoggerFactory?.CreatePureLogger<T>();
        }

        protected TestLoggerBase(string appFolderName, IOperatingSystem operatingSystem, ITestOutputHelper testOutputHelper,
            LogLevel initialMinimumLevel = LogLevel.Debug,
            XUnitSerilogFormatter xUnitSerilogFormatter = XUnitSerilogFormatter.RenderedCompactJsonFormatter)
            : this(new FileSystem(appFolderName, operatingSystem), testOutputHelper, initialMinimumLevel,
                xUnitSerilogFormatter)
        {

        }

        protected TestLoggerBase(IOperatingSystem operatingSystem, ITestOutputHelper testOutputHelper,
            LogLevel initialMinimumLevel = LogLevel.Debug,
            XUnitSerilogFormatter xUnitSerilogFormatter = XUnitSerilogFormatter.RenderedCompactJsonFormatter)
            : this(new FileSystem(typeof(T), operatingSystem), testOutputHelper, initialMinimumLevel,
                xUnitSerilogFormatter)
        {

        }

        protected TestLoggerBase(string appFolderName, ITestOutputHelper testOutputHelper,
            LogLevel initialMinimumLevel = LogLevel.Debug,
            XUnitSerilogFormatter xUnitSerilogFormatter = XUnitSerilogFormatter.RenderedCompactJsonFormatter)
            : this(new FileSystem(appFolderName), testOutputHelper, initialMinimumLevel,
                xUnitSerilogFormatter)
        {

        }

        protected TestLoggerBase(ITestOutputHelper testOutputHelper, LogLevel initialMinimumLevel = LogLevel.Debug,
            XUnitSerilogFormatter xUnitSerilogFormatter = XUnitSerilogFormatter.RenderedCompactJsonFormatter)
            : this(new FileSystem(typeof(T)), testOutputHelper, initialMinimumLevel,xUnitSerilogFormatter)
        {

        }
    }
}
