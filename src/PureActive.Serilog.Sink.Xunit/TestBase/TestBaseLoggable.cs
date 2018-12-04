﻿// ***********************************************************************
// Assembly         : PureActive.Serilog.Sink.Xunit
// Author           : SteveBu
// Created          : 11-17-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="TestBaseLoggable.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
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
    /// <summary>
    /// Class TestBaseLoggable.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <autogeneratedoc />
    public abstract class TestBaseLoggable<T>
    {
        /// <summary>
        /// The logger
        /// </summary>
        /// <autogeneratedoc />
        protected readonly IPureLogger Logger;
        /// <summary>
        /// The logger settings
        /// </summary>
        /// <autogeneratedoc />
        protected readonly ISerilogLoggerSettings LoggerSettings;
        /// <summary>
        /// The test logger factory
        /// </summary>
        /// <autogeneratedoc />
        protected readonly IPureTestLoggerFactory TestLoggerFactory;
        /// <summary>
        /// The test output helper
        /// </summary>
        /// <autogeneratedoc />
        protected readonly ITestOutputHelper TestOutputHelper;

        /// <summary>
        /// The test output helper
        /// </summary>
        /// <autogeneratedoc />
        protected readonly IFileSystem FileSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestBaseLoggable{T}"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="testOutputHelper">The test output helper.</param>
        /// <param name="initialMinimumLevel">The initial minimum level.</param>
        /// <param name="xUnitSerilogFormatter">The x unit serilog formatter.</param>
        /// <autogeneratedoc />
        protected TestBaseLoggable(IFileSystem fileSystem, ITestOutputHelper testOutputHelper,
            LogLevel initialMinimumLevel = LogLevel.Debug,
            XUnitSerilogFormatter xUnitSerilogFormatter = XUnitSerilogFormatter.RenderedCompactJsonFormatter)
        {
            FileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));

            TestOutputHelper = testOutputHelper;
            LoggerSettings = new SerilogLoggerSettings(fileSystem, initialMinimumLevel, LoggingOutputFlags.Testing);

            var loggerConfiguration =
                XunitLoggingSink.CreateXUnitLoggerConfiguration(testOutputHelper, LoggerSettings,
                    xUnitSerilogFormatter);

            TestLoggerFactory = XunitLoggingSink.CreateXUnitSerilogFactory(LoggerSettings, loggerConfiguration);

            Logger = TestLoggerFactory?.CreatePureLogger<T>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestBaseLoggable{T}"/> class.
        /// </summary>
        /// <param name="appFolderName">Name of the application folder.</param>
        /// <param name="operatingSystem">The operating system.</param>
        /// <param name="testOutputHelper">The test output helper.</param>
        /// <param name="initialMinimumLevel">The initial minimum level.</param>
        /// <param name="xUnitSerilogFormatter">The x unit serilog formatter.</param>
        /// <autogeneratedoc />
        protected TestBaseLoggable(string appFolderName, IOperatingSystem operatingSystem,
            ITestOutputHelper testOutputHelper,
            LogLevel initialMinimumLevel = LogLevel.Debug,
            XUnitSerilogFormatter xUnitSerilogFormatter = XUnitSerilogFormatter.RenderedCompactJsonFormatter)
            : this(new FileSystem(appFolderName, operatingSystem), testOutputHelper, initialMinimumLevel,
                xUnitSerilogFormatter)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestBaseLoggable{T}"/> class.
        /// </summary>
        /// <param name="operatingSystem">The operating system.</param>
        /// <param name="testOutputHelper">The test output helper.</param>
        /// <param name="initialMinimumLevel">The initial minimum level.</param>
        /// <param name="xUnitSerilogFormatter">The x unit serilog formatter.</param>
        /// <autogeneratedoc />
        protected TestBaseLoggable(IOperatingSystem operatingSystem, ITestOutputHelper testOutputHelper,
            LogLevel initialMinimumLevel = LogLevel.Debug,
            XUnitSerilogFormatter xUnitSerilogFormatter = XUnitSerilogFormatter.RenderedCompactJsonFormatter)
            : this(new FileSystem(typeof(T), operatingSystem), testOutputHelper, initialMinimumLevel,
                xUnitSerilogFormatter)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestBaseLoggable{T}"/> class.
        /// </summary>
        /// <param name="appFolderName">Name of the application folder.</param>
        /// <param name="testOutputHelper">The test output helper.</param>
        /// <param name="initialMinimumLevel">The initial minimum level.</param>
        /// <param name="xUnitSerilogFormatter">The x unit serilog formatter.</param>
        /// <autogeneratedoc />
        protected TestBaseLoggable(string appFolderName, ITestOutputHelper testOutputHelper,
            LogLevel initialMinimumLevel = LogLevel.Debug,
            XUnitSerilogFormatter xUnitSerilogFormatter = XUnitSerilogFormatter.RenderedCompactJsonFormatter)
            : this(new FileSystem(appFolderName), testOutputHelper, initialMinimumLevel,
                xUnitSerilogFormatter)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestBaseLoggable{T}"/> class.
        /// </summary>
        /// <param name="testOutputHelper">The test output helper.</param>
        /// <param name="initialMinimumLevel">The initial minimum level.</param>
        /// <param name="xUnitSerilogFormatter">The x unit serilog formatter.</param>
        /// <autogeneratedoc />
        protected TestBaseLoggable(ITestOutputHelper testOutputHelper, LogLevel initialMinimumLevel = LogLevel.Debug,
            XUnitSerilogFormatter xUnitSerilogFormatter = XUnitSerilogFormatter.RenderedCompactJsonFormatter)
            : this(new FileSystem(typeof(T)), testOutputHelper, initialMinimumLevel, xUnitSerilogFormatter)
        {
        }
    }
}