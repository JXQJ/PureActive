using System;
using System.ComponentModel;
using FluentAssertions;
using PureActive.Core.Extensions;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Core.UnitTests.Extensions
{
    [Trait("Category", "Unit")]
    public class EnumExtensionsUnitTests : TestBaseLoggable<EnumExtensionsUnitTests>
    {
        public EnumExtensionsUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        [Theory]
        [InlineData("First Second", "First_Second")]
        [InlineData(" ", null)]
        [InlineData("\t", null)]
        [InlineData("\r\n", null)]
        public void EnumExtensions_ToEnumString(string testString, string expectedString)
        {
            testString.ToEnumString().Should().Be(expectedString);
        }

        // ReSharper disable InconsistentNaming
        private enum TestEnum
        {
            [Description("First Second")]
            First_Second,

            [Description("First Second Third")]
            First_Second_Third
        }
        // ReSharper restore InconsistentNaming

        [Theory]
        [InlineData(TestEnum.First_Second, "First Second")]
        [InlineData(TestEnum.First_Second_Third, "First Second Third")]
        public void EnumExtensions_FromEnumString(Enum testEnum, string expectedString)
        {
            testEnum.FromEnumValue().Should().Be(expectedString);
        }

        [Theory]
        [InlineData(TestEnum.First_Second, "First Second")]
        [InlineData(TestEnum.First_Second_Third, "First Second Third")]
        public void EnumExtensions_GetDescription(Enum testEnum, string expectedString)
        {
            testEnum.GetDescription().Should().Be(expectedString);
        }
    }
}
 