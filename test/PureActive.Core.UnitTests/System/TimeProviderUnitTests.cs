using System;
using FluentAssertions;
using PureActive.Core.System;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Core.UnitTests.System
{
    [Trait("Category", "Unit")]
    public class TimeProviderUnitTests : TestBaseLoggable<TimeProviderUnitTests>
    {
        public TimeProviderUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        [Fact]
        public void TimeProvider_Constructor()
        {
            var timeProvider = new TimeProvider();
            var utcNow = DateTime.UtcNow;
            timeProvider.UtcNow.Should().BeCloseTo(utcNow);
        }

    }
}