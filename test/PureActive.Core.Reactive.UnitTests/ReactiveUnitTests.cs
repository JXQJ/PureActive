using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Core.Reactive.UnitTests
{
    [Trait("Category", "Unit")]
    public class ReactiveTests : TestLoggerBase<ReactiveTests>
    {

        public ReactiveTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        [Fact]
        public void Reactive_Create()
        {

        }
    }
}
