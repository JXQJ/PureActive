﻿// ***********************************************************************
// Assembly         : PureActive.Logger.Provider.Serilog.UnitTests
// Author           : SteveBu
// Created          : 11-17-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-25-2018
// ***********************************************************************
// <copyright file="SerilogLoggerSettingsUnitTests.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using PureActive.Core.Abstractions.System;
using PureActive.Core.System;
using PureActive.Logger.Provider.Serilog.Interfaces;
using PureActive.Logger.Provider.Serilog.Settings;
using PureActive.Logger.Provider.Serilog.Types;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Logging.Abstractions.Types;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Serilog.Events;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Logger.Provider.Serilog.UnitTests
{
    /// <summary>
    /// Class SerilogLoggerSettingsUnitTests.
    /// Implements the <see cref="PureActive.Serilog.Sink.Xunit.TestBase.TestBaseLoggable{SerilogLoggerSettingsUnitTests}" />
    /// </summary>
    /// <seealso cref="PureActive.Serilog.Sink.Xunit.TestBase.TestBaseLoggable{SerilogLoggerSettingsUnitTests}" />
    /// <autogeneratedoc />
    [Trait("Category", "Unit")]
    public class SerilogLoggerSettingsUnitTests : TestBaseLoggable<SerilogLoggerSettingsUnitTests>
    {
        /// <summary>
        /// The file system
        /// </summary>
        /// <autogeneratedoc />
        private readonly IFileSystem _fileSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="SerilogLoggerSettingsUnitTests" /> class.
        /// </summary>
        /// <param name="testOutputHelper">The test output helper.</param>
        /// <autogeneratedoc />
        public SerilogLoggerSettingsUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _fileSystem = new FileSystem(typeof(SerilogLoggerSettingsUnitTests));
        }

        /// <summary>
        /// Defines the test method SerilogLoggerSettings_Constructor.
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public void SerilogLoggerSettings_Constructor()
        {
            var loggerSettings = new SerilogLoggerSettings(_fileSystem, LogEventLevel.Debug, LoggingOutputFlags.Default);
            loggerSettings.Should().NotBeNull().And.Subject.Should().BeAssignableTo<ISerilogLoggerSettings>();
            loggerSettings.GetLogLevel(LoggingOutputFlags.Default).InitialLogLevel.Should().Be(LogLevel.Debug);
            loggerSettings.GetLogLevel(LoggingOutputFlags.Default).MinimumLogLevel.Should().Be(LogLevel.Debug);
        }

        /// <summary>
        /// Defines the test method SerilogLoggerSettings_Constructor_FileSystem_Null.
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public void SerilogLoggerSettings_Constructor_FileSystem_Null()
        {
            Func<ISerilogLoggerSettings> fx = () => new SerilogLoggerSettings(null, LogEventLevel.Debug, LoggingOutputFlags.Default);
            fx.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("fileSystem");
        }

        /// <summary>
        /// Defines the test method SerilogLoggerSettings_Constructor_IConfiguration.
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public void SerilogLoggerSettings_Constructor_IConfiguration()
        {
            var configuration = SerilogLoggerSettings.DefaultLoggerSettingsConfiguration(LogLevel.Debug);
            var loggerSettings = new SerilogLoggerSettings(_fileSystem, configuration, LoggingOutputFlags.Default);

            loggerSettings.Should().NotBeNull().And.Subject.Should().BeAssignableTo<ISerilogLoggerSettings>();
            loggerSettings.Configuration.Should().Be(configuration);
            loggerSettings.GetLogLevel(LoggingOutputFlags.Default).InitialLogLevel.Should().Be(LogLevel.Debug);
            loggerSettings.GetLogLevel(LoggingOutputFlags.Default).MinimumLogLevel.Should().Be(LogLevel.Debug);

        }

        /// <summary>
        /// Defines the test method SerilogLoggerSettings_LogFolderPath.
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public void SerilogLoggerSettings_LogFolderPath()
        {
            var loggerSettings = new SerilogLoggerSettings(_fileSystem, LogEventLevel.Debug, LoggingOutputFlags.Default);
            loggerSettings.LogFolderPath.Should().NotBeNullOrEmpty();
        }

        /// <summary>
        /// Defines the test method SerilogLoggerSettings_TestLogFolderPath.
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public void SerilogLoggerSettings_TestLogFolderPath()
        {
            var loggerSettings = new SerilogLoggerSettings(_fileSystem, LogEventLevel.Debug, LoggingOutputFlags.Default);
            loggerSettings.TestLogFolderPath.Should().NotBeNullOrEmpty();
            _fileSystem.FolderExists(loggerSettings.TestLogFolderPath).Should().BeTrue();
        }

        /// <summary>
        /// Defines the test method SerilogLoggerSettings_Constructor_IConfiguration_Null.
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public void SerilogLoggerSettings_Constructor_IConfiguration_Null()
        {
            Func<ISerilogLoggerSettings> fx = () => new SerilogLoggerSettings(_fileSystem, null, LoggingOutputFlags.Default);
            fx.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("configuration");
        }

        /// <summary>
        /// Defines the test method SerilogLoggerSettings_Constructor_LoggingOutputFlags.
        /// </summary>
        /// <param name="loggingOutputFlags">The logging output flags.</param>
        /// <autogeneratedoc />
        [Theory]
        [InlineData(LoggingOutputFlags.Default)]
        [InlineData(LoggingOutputFlags.Console)]
        [InlineData(LoggingOutputFlags.RollingFile)]
        [InlineData(LoggingOutputFlags.AppInsights)]
        [InlineData(LoggingOutputFlags.XUnitConsole)]
        [InlineData(LoggingOutputFlags.TestCorrelator)]
        public void SerilogLoggerSettings_Constructor_LoggingOutputFlags(LoggingOutputFlags loggingOutputFlags)
        {
            var loggerSettings = new SerilogLoggerSettings(_fileSystem, LogEventLevel.Information, loggingOutputFlags);
            loggerSettings.Should().NotBeNull().And.Subject.Should().BeAssignableTo<ISerilogLoggerSettings>();
            var logLevel = loggerSettings.GetLogLevel(loggingOutputFlags);
            logLevel.Should().NotBeNull().And.Subject.Should().BeAssignableTo<IPureLogLevel>();

            logLevel.InitialLogLevel.Should().Be(LogLevel.Information);
            logLevel.MinimumLogLevel.Should().Be(LogLevel.Information);
        }



        /// <summary>
        /// Defines the test method SerilogLoggerSettings_Create_Default_LogEventLevels.
        /// </summary>
        /// <param name="logEventLevel">The log event level.</param>
        /// <autogeneratedoc />
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
            loggerSettings.GetSerilogLogLevel(LoggingOutputFlags.Default).MinimumLogEventLevel.Should()
                .Be(logEventLevel);
            loggerSettings.GetSerilogLogLevel(LoggingOutputFlags.Default).InitialLogEventLevel.Should()
                .Be(logEventLevel);

            // Msft LogLevels
            loggerSettings.GetLogLevel(LoggingOutputFlags.Default).MinimumLogLevel.Should()
                .Be(SerilogLogLevel.SerilogToMsftLogLevel(logEventLevel));
            loggerSettings.GetLogLevel(LoggingOutputFlags.Default).InitialLogLevel.Should()
                .Be(SerilogLogLevel.SerilogToMsftLogLevel(logEventLevel));
        }

        /// <summary>
        /// Defines the test method SerilogLoggerSettings_Create_Default_LogLevels.
        /// </summary>
        /// <param name="logLevel">The log level.</param>
        /// <autogeneratedoc />
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
            loggerSettings.GetSerilogLogLevel(LoggingOutputFlags.Default).MinimumLogEventLevel.Should()
                .Be(SerilogLogLevel.MsftToSerilogLogLevel(logLevel));
            loggerSettings.GetSerilogLogLevel(LoggingOutputFlags.Default).InitialLogEventLevel.Should()
                .Be(SerilogLogLevel.MsftToSerilogLogLevel(logLevel));

            // Msft LogLevels
            loggerSettings.GetLogLevel(LoggingOutputFlags.Default).MinimumLogLevel.Should().Be(logLevel);
            loggerSettings.GetLogLevel(LoggingOutputFlags.Default).InitialLogLevel.Should().Be(logLevel);
        }


    }
}