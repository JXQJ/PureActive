﻿// ***********************************************************************
// Assembly         : PureActive.Logging.UnitTests
// Author           : SteveBu
// Created          : 11-17-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="LoggerMessageUnitTests.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using Microsoft.Extensions.Logging;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Logging.UnitTests.Extensions
{
    /// <summary>
    /// Class LoggerExtensionsTest.
    /// </summary>
    /// <autogeneratedoc />
    public static class LoggerExtensionsTest
    {
        /// <summary>
        /// The quote added message
        /// </summary>
        /// <autogeneratedoc />
        private static readonly Action<ILogger, string, Exception> QuoteAddedMessage;

        /// <summary>
        /// Initializes static members of the <see cref="LoggerExtensionsTest"/> class.
        /// </summary>
        /// <autogeneratedoc />
        static LoggerExtensionsTest()
        {
            QuoteAddedMessage = LoggerMessage.Define<string>(
                LogLevel.Information,
                new EventId(2, nameof(QuoteAdded)),
                "Quote added (Quote = '{Quote}')");
        }

        /// <summary>
        /// Quotes the added.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="quote">The quote.</param>
        /// <autogeneratedoc />
        public static void QuoteAdded(this IPureLogger logger, string quote)
        {
            QuoteAddedMessage(logger, quote, null);
        }
    }

    /// <summary>
    /// Class LoggerMessageUnitTests.
    /// Implements the <see cref="Serilog.Sink.Xunit.TestBase.TestBaseLoggable{LoggerExtensionsUnitTests}" />
    /// </summary>
    /// <seealso cref="Serilog.Sink.Xunit.TestBase.TestBaseLoggable{LoggerExtensionsUnitTests}" />
    /// <autogeneratedoc />
    [Trait("Category", "Unit")]
    public class LoggerMessageUnitTests : TestBaseLoggable<LoggerExtensionsUnitTests>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerMessageUnitTests"/> class.
        /// </summary>
        /// <param name="testOutputHelper">The test output helper.</param>
        /// <autogeneratedoc />
        public LoggerMessageUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        /// <summary>
        /// Defines the test method LoggerExtensions_TestQuoteAdded.
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public void LoggerExtensions_TestQuoteAdded()
        {
            Logger.QuoteAdded("Test Quote Add");
        }
    }
}