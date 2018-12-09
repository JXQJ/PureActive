using FluentAssertions;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PureActive.Core.System;
using PureActive.Hosting.Configuration;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Serilog.Events;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Hosting.UnitTests.Configuration
{
    [Trait("Category", "Unit")]
    public class HostingConfigurationBuilderUnitTests : TestBaseLoggable<HostingConfigurationBuilderUnitTests>
    {
        public HostingConfigurationBuilderUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        [Fact]
        public void HostingConfigurationBuilderUnitTests_AddLoggerSettings()
        {
            var fileSystem = new FileSystem();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(fileSystem.GetCurrentDirectory())
                .AddLoggerSettings(LogLevel.Debug)
                .AddEnvironmentVariables()
                .Build();

            configuration.Should().NotBeNull();
        }


        [Fact]
        public void HostingConfigurationBuilderUnitTests_AddLoggerSettings_Config()
        {
            var fileSystem = new FileSystem();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(fileSystem.GetCurrentDirectory())
                .AddLoggerSettings(LogEventLevel.Debug)
                .AddEnvironmentVariables()
                .Build();

            configuration.Should().NotBeNull();
        }

        [Fact]
        public void HostingConfigurationBuilderUnitTests_AddAppSettings_HostingEnvironment()
        {

            var fileSystem = new FileSystem();
            var hostingEnvironment = new HostingEnvironment();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(fileSystem.GetCurrentDirectory())
                .AddLoggerSettings(LogEventLevel.Debug)
                .AddAppSettings(hostingEnvironment)
                .AddEnvironmentVariables()
                .Build();

            configuration.Should().NotBeNull();
        }
    }
}