using System;
using FluentAssertions;
using PureActive.Core.Abstractions.System;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;
using OperatingSystem = PureActive.Core.System.OperatingSystem;

namespace PureActive.Core.IntegrationTests.System
{
    [Trait("Category", "Integration")]
    public class OperatingSystemIntegrationTests : TestBaseLoggable<OperatingSystemIntegrationTests>
    {
        private readonly IOperatingSystem _operatingSystem;

        public OperatingSystemIntegrationTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _operatingSystem = new OperatingSystem();
        }

        [Fact]
        public void OperatingSystem_IsLinux()
        {
            var osVersion = Environment.OSVersion;

            if (_operatingSystem.IsLinux())
                osVersion.Platform.Should().Be(PlatformID.Unix);
        }

        [Fact]
        public void OperatingSystem_IsOsx()
        {
            var osVersion = Environment.OSVersion;

            if (_operatingSystem.IsOsx())
                osVersion.Platform.Should().Be(PlatformID.MacOSX);
        }


        [Fact]
        public void OperatingSystem_IsWindows()
        {
            var osVersion = Environment.OSVersion;

            if (_operatingSystem.IsWindows())
            {
                Assert.True(osVersion.Platform == PlatformID.Win32NT || osVersion.Platform == PlatformID.Win32S || osVersion.Platform == PlatformID.Win32Windows);
            }
        }
    }
}