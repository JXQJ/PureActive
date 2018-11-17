using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Core.UnitTests.System
{
    [Trait("Category", "Unit")]
    public class ProcessRunnerTests : TestLoggerBase<ProcessRunnerTests>
    {
        public ProcessRunnerTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

    }
}