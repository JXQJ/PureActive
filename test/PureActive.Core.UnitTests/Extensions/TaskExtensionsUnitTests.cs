using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using PureActive.Core.Extensions;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Core.UnitTests.Extensions
{
    public class TaskExtensionsUnitTests : LoggingUnitTestBase<TaskExtensionsUnitTests>
    {

        public TaskExtensionsUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }


        [Fact]
        public async Task TaskExtensions_Empty()
        {
            List<Task> tasks = new List<Task>();

            await tasks.WaitForTasks(CancellationToken.None, Logger);
        }

        [Fact]
        public async Task TaskExtensions_Wait()
        {
            List<Task> tasks = new List<Task>
            {
                Task.Delay(300),
                Task.Delay(100)
            };

            var stopWatch = Stopwatch.StartNew();

            await tasks.WaitForTasks(CancellationToken.None, Logger);

            stopWatch.Stop();

            stopWatch.ElapsedMilliseconds.Should().BeGreaterOrEqualTo(300);
            TestOutputHelper.WriteLine(stopWatch.ElapsedMilliseconds.ToString());
        }

        [Fact]
        public void TaskExtensions_Cancel()
        {
            List<Task> tasks = new List<Task>
            {
                Task.Delay(300),
            };

            var cts = new CancellationTokenSource();

            cts.CancelAfter(100);

            var result = tasks.WaitForTasks(cts.Token, Logger);

            result.Status.Should().Be(TaskStatus.Faulted);
        }

        [Fact]
        public void TaskExtensions_Exception()
        {
            List<Task> tasks = new List<Task>
            {
               Task.Run(() => throw new InvalidOperationException())
            };

            var result = tasks.WaitForTasks(CancellationToken.None, Logger);

            result.Status.Should().Be(TaskStatus.Faulted);
        }

    }
}
