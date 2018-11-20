using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using PureActive.Core.Abstractions.System;
using PureActive.Core.System;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Core.UnitTests.System
{
    [Trait("Category", "Unit")]
    public class ProcessRunnerUnitTests : TestBaseLoggable<ProcessRunnerUnitTests>
    {
        private IProcessRunner _processRunner;
        private IFileSystem _fileSystem;

        public ProcessRunnerUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _processRunner = new ProcessRunner(TestLoggerFactory.CreatePureLogger<ProcessRunner>());
            _fileSystem = new FileSystem(typeof(ProcessRunnerUnitTests));
        }

        [Fact]
        public void ProcessRunner_Constructor()
        {
            _processRunner.Should().NotBeNull().And.Subject.Should().BeOfType(typeof(ProcessRunner));
            _fileSystem.Should().NotBeNull().And.Subject.Should().BeOfType(typeof(FileSystem));
        }


        [Fact]
        public void ProcessRunner_RunProcessAsync_Null_Path()
        {
            Func<Task> act = async () => await _processRunner.RunProcessAsync(null, new string[1], null);
            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("path");
        }

        [Fact]
        public void ProcessRunner_RunProcessAsync_Empty_Path()
        {
            Func<Task> act = async () => await _processRunner.RunProcessAsync("", new string[1], null);
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void ProcessRunner_RunProcessAsync_Null_Args()
        {
            var arpCommandPath = _fileSystem.ArpCommandPath();

            Func<Task> act = async () => await _processRunner.RunProcessAsync(arpCommandPath, null, null);
            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("args");
        }

    }
}