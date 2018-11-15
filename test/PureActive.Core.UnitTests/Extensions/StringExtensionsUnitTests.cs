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
        [InlineData("V", "v")]
        [InlineData("\t", "\t")]
        public void StringExtensions_ToCamelCase(string testString, string expectedString)
        {
            testString.ToCamelCase().Should().Be(expectedString);
        }

        [Fact]
        public void StringExtensions_ToCamelCase_Empty()
        {
            "".ToCamelCase().Should().Be("");
        }

        [Fact]
        public void StringExtensions_ToCamelCase_Null()
        {
            ((string)null).ToCamelCase().Should().BeNull();
        }

        [Theory]
        [InlineData("value", "Value")]
        [InlineData("v", "V")]
        [InlineData("\t", "\t")]
        public void StringExtensions_ToPascalCase(string testString, string expectedString)
        {
            testString.ToPascalCase().Should().Be(expectedString);
        }

        [Fact]
        public void StringExtensions_ToPascalCase_Empty()
        {
            "".ToPascalCase().Should().Be("");
        }

        [Fact]
        public void StringExtensions_ToPascalCase_Null()
        {
            ((string)null).ToPascalCase().Should().BeNull();
        }


        [Theory]
        [InlineData("123*1%_ABC", "1231ABC")]
        [InlineData("_", "")]
        [InlineData("abc", "abc")]
        [InlineData("abc\t\n", "abc")]
        public void StringExtensions_ToAlphaNumeric(string testString, string expectedString)
        {
            testString.ToAlphaNumeric().Should().Be(expectedString);
        }

        [Fact]
        public void StringExtensions_ToAlphaNumeric_Empty()
        {
            "".ToAlphaNumeric().Should().Be("");
        }

        [Fact]
        public void StringExtensions_ToAlphaNumeric_Null()
        {
            ((string)null).ToAlphaNumeric().Should().BeNull();
        }


        [Theory]
        [InlineData("123 1 ABC", "1231ABC")]
        [InlineData(" ", "")]
        [InlineData("\n", "")]
        [InlineData("\t", "")]
        [InlineData("abc", "abc")]
        [InlineData("abc\t\n", "abc")]
        public void StringExtensions_RemoveWhitespace(string testString, string expectedString)
        {
            testString.RemoveWhitespace().Should().Be(expectedString);
        }

        [Fact]
        public void StringExtensions_RemoveWhitespace_Empty()
        {
            "".RemoveWhitespace().Should().Be("");
        }

        [Fact]
        public void StringExtensions_RemoveWhitespace_Null()
        {
            ((string)null).RemoveWhitespace().Should().BeNull();
        }


        [Theory]
        [InlineData("123 1 ABC", "1231")]
        [InlineData(" ", "")]
        [InlineData("\n", "")]
        [InlineData("\t", "")]
        [InlineData("abc", "")]
        [InlineData("123\t\n", "123")]
        public void StringExtensions_ToNumeric(string testString, string expectedString)
        {
            testString.ToNumeric().Should().Be(expectedString);
        }

        [Fact]
        public void StringExtensions_ToNumeric_Empty()
        {
            "".ToNumeric().Should().Be("");
        }

        [Fact]
        public void StringExtensions_ToNumeric_Null()
        {
            ((string)null).ToNumeric().Should().BeNull();
        }


    }
}
 