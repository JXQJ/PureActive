using System;
using Microsoft.Extensions.Logging;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Logging.UnitTests.Extensions
{
    public static class LoggerExtensionsTest
    {
        private static readonly Action<ILogger, string, Exception> QuoteAddedMessage;

        static LoggerExtensionsTest()
        {
            QuoteAddedMessage = LoggerMessage.Define<string>(
                LogLevel.Information,
                new EventId(2, nameof(QuoteAdded)),
                "Quote added (Quote = '{Quote}')");
        }

        public static void QuoteAdded(this IPureLogger logger, string quote)
        {
            QuoteAddedMessage(logger, quote, null);

        }
    }

    [Trait("Category", "Unit")]
    public class LoggerMessageTests : TestLoggerBase<LoggerExtensionsTests>
    {
        public LoggerMessageTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }
 
        [Fact]
        public void LoggerExtensions_TestQuoteAdded()
        {
            Logger.QuoteAdded("Test Quote Add");
        }
    }
}