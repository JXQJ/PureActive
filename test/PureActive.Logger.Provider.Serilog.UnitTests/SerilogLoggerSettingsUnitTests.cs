using FluentAssertions;
using Microsoft.Extensions.Logging;
using PureActive.Core.Abstractions.System;
using PureActive.Core.System;
using PureActive.Logger.Provider.Serilog.Settings;
using PureActive.Logger.Provider.Serilog.Types;
using PureActive.Logging.Abstractions.Types;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Serilog.Events;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Logger.Provider.Serilog.UnitTests
{
    [Trait("Category", "Unit")]
    public class SerilogLoggerSettingsUnitTests : TestBaseLoggable<SerilogLoggerSettingsUnitTests>
    {
        private readonly IFileSystem _fileSystem;

        public SerilogLoggerSettingsUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _fileSystem = new FileSystem(typeof(SerilogLoggerSettingsUnitTests));
        }

        [Theory]
        [InlineData(LogEventLevel.Verbose)]
        [InlineData(LogEventLevel.Debug)]
        [InlineData(LogEventLevel.Information)]
        [InlineData(LogEventLevel.Warning)]
        [InlineData(LogEventLevel.Error)]
        [InlineData(LogEventLevel.Fatal)]
        public void SerilogLoggerSettings_Create_Default_LogEventLevels(LogEventLevel logEventLevel)
        {
            var loggerSettings = new SerilogLoggerSettings(_fileSystem, logEventLevel, LoggingOutputFlags.Default);

            loggerSettings.LoggingOutputFlags.Should().Be(LoggingOutputFlags.Default);

            // Serilog LogEventLevels
            loggerSettings.GetSerilogLogLevel(LoggingOutputFlags.Default).MinimumLogEventLevel.Should().Be(logEventLevel);
            loggerSettings.GetSerilogLogLevel(LoggingOutputFlags.Default).InitialLogEventLevel.Should().Be(logEventLevel);

            // Msft LogLevels
            loggerSettings.GetLogLevel(LoggingOutputFlags.Default).MinimumLogLevel.Should().Be(SerilogLogLevel.SerilogToMsftLogLevel(logEventLevel));
            loggerSettings.GetLogLevel(LoggingOutputFlags.Default).InitialLogLevel.Should().Be(SerilogLogLevel.SerilogToMsftLogLevel(logEventLevel));
        }

        [Theory]
        [InlineData(LogLevel.Trace)]
        [InlineData(LogLevel.Debug)]
        [InlineData(LogLevel.Information)]
        [InlineData(LogLevel.Warning)]
        [InlineData(LogLevel.Error)]
        [InlineData(LogLevel.Critical)]
//        [InlineData(LogLevel.None)]
        public void SerilogLoggerSettings_Create_Default_LogLevels(LogLevel logLevel)
        {
            var loggerSettings = new SerilogLoggerSettings(_fileSystem, logLevel, LoggingOutputFlags.Default);

            loggerSettings.LoggingOutputFlags.Should().Be(LoggingOutputFlags.Default);

            // Serilog LogEventLevels
            loggerSettings.GetSerilogLogLevel(LoggingOutputFlags.Default).MinimumLogEventLevel.Should().Be(SerilogLogLevel.MsftToSerilogLogLevel(logLevel));
            loggerSettings.GetSerilogLogLevel(LoggingOutputFlags.Default).InitialLogEventLevel.Should().Be(SerilogLogLevel.MsftToSerilogLogLevel(logLevel));

            // Msft LogLevels
            loggerSettings.GetLogLevel(LoggingOutputFlags.Default).MinimumLogLevel.Should().Be(logLevel);
            loggerSettings.GetLogLevel(LoggingOutputFlags.Default).InitialLogLevel.Should().Be(logLevel);
        }
    }
}
