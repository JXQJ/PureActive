using FluentAssertions;
using PureActive.Core.Extensions;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Core.UnitTests.Extensions
{
    public class StringExtensionsUnitTests : LoggingUnitTestBase<StringExtensionsUnitTests>
    {
        public StringExtensionsUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        [Theory]
        [InlineData("Value", "value")]
        public void StringExtensions_ToCamelCase(string testString, string expectedString)
        {
            testString.ToCamelCase().Should().Be(expectedString);
        }
    }
}
 