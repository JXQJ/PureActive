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

namespace PureActive.Hosting.IntegrationTests.Configuration
{
    [Trait("Category", "Integration")]
    public class HostingConfigurationBuilderIntegrationTests : TestBaseLoggable<HostingConfigurationBuilderIntegrationTests>
    {
        public HostingConfigurationBuilderIntegrationTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        [Fact]
        public void HostingConfigurationBuilderIntegrationTests_AddAppSettings_HostingEnvironment()
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