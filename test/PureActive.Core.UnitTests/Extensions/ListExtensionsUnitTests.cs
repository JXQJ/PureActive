using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using PureActive.Core.Extensions;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Core.UnitTests.Extensions
{
    public class ListExtensionsUnitTests : LoggingUnitTestBase<ListExtensionsUnitTests>
    {
        public ListExtensionsUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        [Fact]
        public void ListExtensions_ShuffleList()
        {
            var listOfIntegers = new List<int>
            {
                1, 2, 4, 5, 6, 8, 9, 10
            };

            var listRandom = listOfIntegers.ToList();
            listRandom.Shuffle();

            listRandom.Should().NotBeAscendingInOrder();
            listRandom.Should().NotBeDescendingInOrder();
        }

        [Fact]
        public void ListExtensions_AddItemList()
        {
            var listOfIntegers = new List<int>
            {
                1, 2, 4, 5, 6, 8, 9, 10
            };

            var oldCount = listOfIntegers.Count;
            var listOfIntegersReturned = listOfIntegers.AddItem(11);

            listOfIntegersReturned.Should().BeSameAs(listOfIntegers);
            listOfIntegersReturned.Count.Should().Be(oldCount + 1);
            listOfIntegersReturned.Last().Should().Be(11);
        }

        [Fact]
        public void ListExtensions_AddItemCollection()
        {
            var listOfIntegers = new List<int>
            {
                1, 2, 4, 5, 6, 8, 9, 10
            };

            var oldCount = ((ICollection<int>) listOfIntegers).Count();
            var listOfIntegersReturned = ((ICollection<int>) listOfIntegers).AddItem(11);

            listOfIntegersReturned.Should().BeSameAs(listOfIntegers);
            listOfIntegersReturned.Count.Should().Be(oldCount + 1);
            listOfIntegersReturned.Last().Should().Be(11);
        }

        public class ObjectField
        {
            public Guid Guid { get; set; } = Guid.NewGuid();
        }

        public class ObjectTest: ICloneable
        {
            public Guid Guid { get; set; } = Guid.NewGuid();
            public ObjectField ObjectField { get; set; } = new ObjectField();

            public object Clone()
            {
                return new ObjectTest
                {
                    Guid = Guid,
                    ObjectField = new ObjectField()
                    {
                        Guid = ObjectField.Guid
                    }
                };
            }
        }

        [Fact]
        public void ListExtensions_CloneList()
        {
            var objectTestList = new List<ObjectTest>
            {
                new ObjectTest(),
                new ObjectTest(),
                new ObjectTest(),
                new ObjectTest(),
                new ObjectTest(),
            };

            var objectTestListClone = objectTestList.CloneList();
            objectTestListClone.Should().BeEquivalentTo(objectTestList);
            objectTestList.First().ObjectField.Should().NotBeSameAs(objectTestListClone.First().ObjectField);
        }
    }
}
