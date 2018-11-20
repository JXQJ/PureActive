using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using PureActive.Core.Abstractions.System;
using PureActive.Core.Extensions;
using PureActive.Core.System;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Core.IntegrationTests.System
{
    [Trait("Category", "Integration")]
    public class ProcessRunnerIntegrationTests : TestBaseLoggable<ProcessRunnerIntegrationTests>
    {
        private IProcessRunner _processRunner;
        private IFileSystem _fileSystem;

        public ProcessRunnerIntegrationTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _processRunner = new ProcessRunner(TestLoggerFactory.CreatePureLogger<ProcessRunner>());
            _fileSystem = new FileSystem(typeof(ProcessRunnerIntegrationTests));
        }

        [Fact]
        public async Task ProcessRunner_RunProcessAsync_ArpTest_NoTimeout()
        {
            var arpCommandPath = _fileSystem.ArpCommandPath();
            var args = new[] { $"-a" };

            var result = await _processRunner.RunProcessAsync(arpCommandPath, args, null);
            result.Should().NotBeNull().And.Subject.Should().BeOfType(typeof(ProcessResult));
            result.Completed.Should().BeTrue();
            result.Output.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task ProcessRunner_RunProcessAsync_ArpTest_InstantTimeout()
        {
            var arpCommandPath = _fileSystem.ArpCommandPath();
            var args = new[] { $"-a" };

            var result = await _processRunner.RunProcessAsync(arpCommandPath, args, new TimeSpan(10));
            result.Should().NotBeNull().And.Subject.Should().BeOfType(typeof(ProcessResult));
            result.Completed.Should().BeFalse();
            result.Output.Trim().Should().BeNullOrEmpty();
        }

    }
}