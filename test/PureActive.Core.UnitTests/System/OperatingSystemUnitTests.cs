using FluentAssertions;
using PureActive.Core.Abstractions.System;
using PureActive.Core.System;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Core.UnitTests.System
{
    [Trait("Category", "Unit")]
    public class OperatingSystemTests : TestLoggerBase<OperatingSystemTests>
    {
        public OperatingSystemTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        [Fact]
        public void OperationSystem_Constructor()
        {
            var operatingSystem = new OperatingSystem();
            operatingSystem.Should().NotBeNull().And.Subject.Should().BeAssignableTo<IOperatingSystem>();
        }
    }
}