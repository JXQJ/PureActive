using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Core.UnitTests.System
{
    [Trait("Category", "Unit")]
    public class ProcessRunnerUnitTests : TestBaseLoggable<ProcessRunnerUnitTests>
    {
        public ProcessRunnerUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

    }
}