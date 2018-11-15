using System;
using FluentAssertions;
using PureActive.Core.Extensions;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Core.UnitTests.Extensions
{
    public class GuidExtensionsUnitTests : LoggingUnitTestBase<GuidExtensionsUnitTests>
    {
        public GuidExtensionsUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        [Fact]
        public void GuidExtensions_UpperCase()
        {
            var guid = Guid.NewGuid();
            var guidNoDashes = guid.ToStringNoDashes();

            guidNoDashes.Should().Be(guid.ToString().ToUpper().Replace("-", ""));
        }

        [Fact]
        public void GuidExtensions_Empty()
        {
            Guid guid = new Guid();

            var guidNoDashes = guid.ToStringNoDashes();

            guidNoDashes.Should().Be("00000000000000000000000000000000");
        }
    }
}
