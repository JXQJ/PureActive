using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using PureActive.Core.Abstractions.Async;
using PureActive.Core.Async;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Core.UnitTests.Async
{
    [Trait("Category", "Unit")]
    public class OperationRunnerTests : TestLoggerBase<OperationRunnerTests>
    {
        private readonly IOperationRunner _operationRunner;

        public OperationRunnerTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _operationRunner = new OperationRunner(TestLoggerFactory.CreatePureLogger<OperationRunner>());
        }

        [Fact]
        public void OperationRunner_ValidateConstructor()
        {
            _operationRunner.Should().NotBeNull();
        }


        [Fact]
        public async Task OperationRunner_Delay()
        {
            // Test a delay of 2 secs on a 4 sec timeout
            Assert.True(await _operationRunner.RunOperationWithTimeoutAsync(() => Task.Delay(2000), new TimeSpan(0, 0, 4), CancellationToken.None));
        }

        [Fact]
        public async Task OperationRunner_Timeout()
        {
            // Test a delay of 4 secs on a 2 sec timeout
            Assert.False(await _operationRunner.RunOperationWithTimeoutAsync(() => Task.Delay(4000), new TimeSpan(0,0,2), CancellationToken.None));
        }


    }
}
