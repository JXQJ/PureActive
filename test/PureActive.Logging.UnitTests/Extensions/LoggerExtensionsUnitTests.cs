using Microsoft.Extensions.Logging;
using PureActive.Logging.Extensions.Extensions;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Logging.UnitTests.Extensions
{
    [Trait("Category", "Unit")]
    public class LoggerExtensionsTests : TestLoggerBase<LoggerExtensionsTests>
    {
        public LoggerExtensionsTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        [Fact]
        public void LoggerExtensions_BeginPropertyScope()
        {
            using (Logger.BeginPropertyScope("PropertyInt", 14))
            {
                Logger.LogDebug("Log Int Property Scope");
            }
        }
    }
}
