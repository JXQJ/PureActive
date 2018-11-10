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
        private static readonly Action<ILogger, string, Exception> _quoteAdded;

        static LoggerExtensionsTest()
        {
            _quoteAdded = LoggerMessage.Define<string>(
                LogLevel.Information,
                new EventId(2, nameof(QuoteAdded)),
                "Quote added (Quote = '{Quote}')");
        }

        public static void QuoteAdded(this IPureLogger logger, string quote)
        {
            _quoteAdded(logger, quote, null);

        }
    }


    public class LoggerMessageUnitTests : LoggingUnitTestBase<LoggerExtensionsUnitTests>
    {
        public LoggerMessageUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }
 
        [Fact]
        public void LoggerExtensions_TestQuoteAdded()
        {
            Logger.QuoteAdded("Test Quote Add");
        }
    }
}