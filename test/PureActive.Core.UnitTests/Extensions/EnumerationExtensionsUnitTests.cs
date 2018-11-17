using System.Collections.Generic;
using FluentAssertions;
using PureActive.Core.Extensions;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Core.UnitTests.Extensions
{
    [Trait("Category", "Unit")]
    public class EnumerationExtensionsTests : TestLoggerBase<EnumerationExtensionsTests>
    {
        public EnumerationExtensionsTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        [Fact]
        public void EnumerationExtensions_StringList()
        {
            var stringList = new List<string>()
            {
                "12345",
                "123456",
                "123",
                "15"
            };

            var maxLength = stringList.MaxStringLength();

            maxLength.Should().Be("123456".Length);
        }

        [Fact]
        public void EnumerationExtensions_Empty()
        {
            var maxLength = new List<string>().MaxStringLength();

            maxLength.Should().Be(0);
        }

        [Fact]
        public void EnumerationExtensions_Null()
        {
            var maxLength = ((List<string>) null).MaxStringLength();

            maxLength.Should().Be(0);
        }

        [Fact]
        public void EnumerationExtensions_IntList()
        {
            var intList = new List<int>()
            {
                12345,
                123456,
                123,
                15
            };

            var maxLength = intList.MaxStringLength();

            maxLength.Should().Be("123456".Length);
        }

        private class TestObject
        {
            
        }

        [Fact]
        public void EnumerationExtensions_TestObject()
        {
            var testObjectList = new List<TestObject>()
            {
                new TestObject()
            };

            var maxLength = testObjectList.MaxStringLength();

            maxLength.Should().Be(typeof(TestObject).ToString().Length);
        }
    }
}
