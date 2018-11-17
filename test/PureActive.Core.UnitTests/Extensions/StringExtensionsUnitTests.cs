using System.Linq;
using System.Reflection;
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
            ((string) null).ToCamelCase().Should().BeNull();
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
            ((string) null).ToPascalCase().Should().BeNull();
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
            ((string) null).ToAlphaNumeric().Should().BeNull();
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
            ((string) null).RemoveWhitespace().Should().BeNull();
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
            ((string) null).ToNumeric().Should().BeNull();
        }


        [Theory]
        [InlineData("1231 \n", "1231")]
        [InlineData("1231 \r\n 42 324 324 ", "1231\n42 324 324")]
        [InlineData(" \r\n ", "")]
        [InlineData(" \r\n 1", "1")]
        [InlineData(" \r\n 1 \r\n\t 2", "1\n2")]
        public void StringExtensions_TrimEveryLine(string testString, string expectedString)
        {
            testString.TrimEveryLine().Should().Be(expectedString);
        }

        [Fact]
        public void StringExtensions_TrimEveryLine_Empty()
        {
            "".TrimEveryLine().Should().Be("");
        }

        [Fact]
        public void StringExtensions_TrimEveryLine_Null()
        {
            ((string) null).ToNumeric().Should().BeNull();
        }


        [Theory]
        [InlineData("First|Last", '|', new[] {"First", "Last"})]
        [InlineData("First|Middle|Last", '|', new[] {"First", "Middle|Last"})]
        [InlineData("First", '|', new[] {"First", ""})]
        [InlineData(" First | Last ", '|', new[] {"First", "Last"})]
        [InlineData(" First |  ", '|', new[] {"First", ""})]
        [InlineData("| Last ", '|', new[] {"", "Last"})]
        public void StringExtensions_SplitOnFirstDelim(string testString, char chDelim, string[] expectStrings)
        {
            var result = testString.SplitOnFirstDelim(chDelim);

            result.Should().NotBeNull().And.Subject.Should().AllBeOfType(typeof(string)).And.Subject.Count().Should()
                .Be(2);
            result.Should().ContainInOrder(expectStrings);
        }


        [Fact]
        public void StringExtensions_SplitOnFirstDelim_Empty()
        {
            var result = "".SplitOnFirstDelim('|');
            result.Should().NotBeNull().And.Subject.Should().BeOfType(typeof(string[])).And.Subject.Count().Should()
                .Be(2);
            result[0].Should().BeNull();
            result[1].Should().BeNull();
        }

        [Fact]
        public void StringExtensions_SplitOnFirstDelim_Null()
        {
            var result = ((string) null).SplitOnFirstDelim('|');

            result.Should().NotBeNull().And.Subject.Should().BeOfType(typeof(string[])).And.Subject.Count().Should()
                .Be(2);
            result[0].Should().BeNull();
            result[1].Should().BeNull();
        }


        [Theory]
        [InlineData("First|Last", '|', new[] {"First", "Last"})]
        [InlineData("First|Middle|Last", '|', new[] {"First|Middle", "Last"})]
        [InlineData("First", '|', new[] {"First", ""})]
        [InlineData(" First | Last ", '|', new[] {"First", "Last"})]
        [InlineData(" First |  ", '|', new[] {"First", ""})]
        [InlineData("| Last ", '|', new[] {"", "Last"})]
        public void StringExtensions_SplitOnLastDelim(string testString, char chDelim, string[] expectStrings)
        {
            var result = testString.SplitOnLastDelim(chDelim);

            result.Should().NotBeNull().And.Subject.Should().AllBeOfType(typeof(string)).And.Subject.Count().Should()
                .Be(2);
            result.Should().ContainInOrder(expectStrings);
        }


        [Fact]
        public void StringExtensions_SplitOnLastDelim_Empty()
        {
            var result = "".SplitOnLastDelim('|');
            result.Should().NotBeNull().And.Subject.Should().BeOfType(typeof(string[])).And.Subject.Count().Should()
                .Be(2);
            result[0].Should().BeNull();
            result[1].Should().BeNull();
        }

        [Fact]
        public void StringExtensions_SplitOnLastDelim_Null()
        {
            var result = ((string) null).SplitOnLastDelim('|');

            result.Should().NotBeNull().And.Subject.Should().BeOfType(typeof(string[])).And.Subject.Count().Should()
                .Be(2);
            result[0].Should().BeNull();
            result[1].Should().BeNull();
        }

        [Fact]
        public void StringExtensions_ProcessSplits_Null()
        {
            var methodInfo =
                typeof(StringExtensions).GetMethod("ProcessSplits", BindingFlags.NonPublic | BindingFlags.Static);

            object[] parameters = {null, 1};

            var result = (string[]) methodInfo.Invoke(null, parameters);

            result.Should().NotBeNull().And.Subject.Should().BeOfType(typeof(string[])).And.Subject.Count().Should()
                .Be(2);
            result[0].Should().BeNull();
            result[1].Should().BeNull();
        }

        [Fact]
        public void StringExtensions_NullIfWhitespace_Space()
        {
            " ".NullIfWhitespace().Should().BeNull();
        }

        [Fact]
        public void StringExtensions_NullIfWhitespace_Empty()
        {
            "".NullIfWhitespace().Should().BeNull();
        }


        [Fact]
        public void StringExtensions_NullIfWhitespace_Null()
        {
            ((string) null).NullIfWhitespace().Should().BeNull();
        }

        [Theory]
        [InlineData("1231 \n", "1231")]
        [InlineData("1231 \r\n 42 324 324 ", "1231 \r\n 42 324 324")]
        [InlineData(" \r\n ", null)]
        [InlineData(" \t\r\n ", null)]
        [InlineData(" \r\n 1", "1")]
        public void StringExtensions_NullIfWhitespace(string testString, string expectedString)
        {
            testString.NullIfWhitespace().Should().Be(expectedString);
        }


        [Theory]
        [InlineData("First|Last", '|', "First")]
        [InlineData("First|Last", '*', "First|Last")]
        [InlineData("|Last", '|', "")]
        public void StringExtensions_StringBeforeDelim(string testString, char chDelim, string expectedString)
        {
            testString.StringBeforeDelim(chDelim).Should().Be(expectedString);
        }

        [Fact]
        public void StringExtensions_StringBeforeDelim_Empty()
        {
            "".StringBeforeDelim('|').Should().BeNull();
        }

        [Fact]
        public void StringExtensions_StringBeforeDelim_Null()
        {
            ((string) null).StringBeforeDelim('|').Should().BeNull();
        }


        [Theory]
        [InlineData("First|Last", '|', "Last")]
        [InlineData("First|Last", '*', "")]
        [InlineData("|Last", '|', "Last")]
        [InlineData("First|", '|', "")]
        public void StringExtensions_StringAfterDelim(string testString, char chDelim, string expectedString)
        {
            testString.StringAfterDelim(chDelim).Should().Be(expectedString);
        }

        [Fact]
        public void StringExtensions_StringAfterDelim_Empty()
        {
            "".StringAfterDelim('|').Should().BeNull();
        }

        [Fact]
        public void StringExtensions_StringAfterDelim_Null()
        {
            ((string)null).StringAfterDelim('|').Should().BeNull();
        }


        [Theory]
        [InlineData("Yes", true)]
        [InlineData("Y", true)]
        [InlineData("No", false)]
        [InlineData("N", false)]
        [InlineData("Yellow", true)]
        [InlineData("Nope", false)]
        public void StringExtensions_ParseYesNo(string testString, bool? expectedBool)
        {
            testString.ParseYesNo().Should().Be(expectedBool);
        }

        [Fact]
        public void StringExtensions_ParseYesNo_Empty()
        {
            "".ParseYesNo().Should().BeNull();
        }

        [Fact]
        public void StringExtensions_ParseYesNo_Null()
        {
            ((string)null).ParseYesNo().Should().BeNull();
        }


        [Theory]
        [InlineData("Yes", true)]
        [InlineData("No", false)]
        [InlineData("Y", null)]
        [InlineData("N", null)]
        [InlineData("Yellow", null)]
        [InlineData("Nope", null)]
        public void StringExtensions_ParseYesNoStrict(string testString, bool? expectedBool)
        {
            testString.ParseYesNoStrict().Should().Be(expectedBool);
        }

        [Fact]
        public void StringExtensions_ParseYesNoStrict_Empty()
        {
            "".ParseYesNoStrict().Should().BeNull();
        }

        [Fact]
        public void StringExtensions_ParseYesNoStrict_Null()
        {
            ((string)null).ParseYesNoStrict().Should().BeNull();
        }


        

        [Theory]
        [InlineData("1.0", 1.0)]
        [InlineData(".1", .1)]
        [InlineData("15", 15.0)]
        [InlineData("-1", -1.0)]
        [InlineData(" 15 ", 15.0)]
        [InlineData(" -1 ", -1.0)]
        [InlineData(" - 1 ", null)]
        [InlineData("1/3", null)]
        [InlineData(null, null)]
        [InlineData("", null)]
        [InlineData(" ", null)]
        public void StringExtensions_ParseDoubleOrNull(string testString, double? expectedDouble)
        {
            testString.ParseDoubleOrNull().Should().Be(expectedDouble);
        }

        [Theory]
        [InlineData("1.0", null)]
        [InlineData(".1", null)]
        [InlineData("15", 15)]
        [InlineData("-1", -1)]
        [InlineData("1/3", null)]
        [InlineData(" 15 ", 15)]
        [InlineData(" -1 ", -1)]
        [InlineData(" - 1 ", null)]
        [InlineData(null, null)]
        [InlineData("", null)]
        [InlineData(" ", null)]
        public void StringExtensions_ParseIntOrNull(string testString, int? expectedInt)
        {
            testString.ParseIntOrNull().Should().Be(expectedInt);
        }

        [Theory]
        [InlineData("1.0", null)]
        [InlineData(".1", null)]
        [InlineData("15", 15L)]
        [InlineData("-1", -1L)]
        [InlineData("1/3", null)]
        [InlineData(" 15 ", 15L)]
        [InlineData(" -1 ", -1L)]
        [InlineData(" - 1 ", null)]
        [InlineData(null, null)]
        [InlineData("", null)]
        [InlineData(" ", null)]
        public void StringExtensions_ParseLongOrNull(string testString, long? expectedLong)
        {
            testString.ParseLongOrNull().Should().Be(expectedLong);
        }

        [Theory]
        [InlineData(null, "\"\"")]
        [InlineData("", "\"\"")]
        [InlineData(" ", "\" \"")]
        [InlineData("Test", "\"Test\"")]
        public void StringExtensions_ToDoubleQuote(string testString, string expectedString)
        {
            testString.ToDoubleQuoted().Should().Be(expectedString);
        }

        [Theory]
        [InlineData("Property", ":", 20, "Property:           ")]
        [InlineData("Property", ":", 5, "Property:")]
        [InlineData(null, ":", 5, ":    ")]
        [InlineData(null, null, 5, "     ")]
        public void StringExtensions_PadWithDelim(string testString, string delimString, int length, string expectedString)
        {
            testString.PadWithDelim(delimString, length).Should().Be(expectedString);
        }

    }
}
 