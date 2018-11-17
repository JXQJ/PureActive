using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Core.UnitTests.System
{
    [Trait("Category", "Unit")]
    public class ProcessRunnerUnitTests : LoggingUnitTestBase<ProcessRunnerUnitTests>
    {
        public ProcessRunnerUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

    }
}