﻿using System;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Network.Abstractions.PureObject;
using PureActive.Network.Devices.PureObject;
using PureActive.Network.Devices.PureObjectGraph;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Network.Devices.UnitTests.PureObjectGraph
{
    public class PureObjectGraphUnitTests : LoggingUnitTestBase<PureObjectGraphUnitTests>
    {
        public PureObjectGraphUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        private class PureObjectBaseTest : PureObjectBase, IComparable<PureObjectBaseTest>
        {
            public string TestValue { get; set; }

            public PureObjectBaseTest(string testValue, IPureLoggerFactory loggerFactory) :base (loggerFactory)
            {
                TestValue = testValue ?? throw new ArgumentNullException(nameof(testValue));
            }

            public override int CompareTo(IPureObject other)
            {
                if (!(other is PureObjectBaseTest))
                    throw new ArgumentException("Object must be of type PureObjectBaseTest.");

                return CompareTo((PureObjectBaseTest)other);
            }

            public int CompareTo(PureObjectBaseTest other)
            {
                return other == null ? 1 : string.Compare(TestValue, other.TestValue, StringComparison.Ordinal);
            }
        }

        [Fact]
        public void VertexEqualityTest()
        {
            var objectBaseTest1 = new PureObjectBaseTest("VertexCreationTest", TestLoggerFactory);
            var objectBaseTest2 = new PureObjectBaseTest("VertexCreationTest", TestLoggerFactory);
            
            var vertex1 = new PureObjectVertex<PureObjectBaseTest>(objectBaseTest1);
            var vertex2 = new PureObjectVertex<PureObjectBaseTest>(objectBaseTest2);
            var vertex1Ref = vertex1;

            // ObjectIds are different
            Assert.False(vertex1.Equals(vertex2), "vertex1.Equals(vertex2)");
            Assert.True(vertex1.Equals(vertex1Ref), "vertex1.Equals(vertex1Ref)");
        }


        [Fact]
        public void VertexComparisonTest()
        {
            var objectBaseTest1 = new PureObjectBaseTest("VertexCreationTest", TestLoggerFactory);
            var objectBaseTest2 = new PureObjectBaseTest("VertexCreationTest", TestLoggerFactory);

            var vertex1 = new PureObjectVertex<PureObjectBaseTest>(objectBaseTest1);
            var vertex2 = new PureObjectVertex<PureObjectBaseTest>(objectBaseTest2);
            var vertex1Ref = vertex1;

            // ObjectIds are different
            Assert.True(vertex1.CompareTo(vertex2.Value) == 0, "vertex1.CompareTo(vertex2.Value) == 0");
            Assert.True(vertex1.CompareTo(vertex2) == 0, "vertex1.CompareTo(vertex2) == 0");
        }

        [Fact]
        public void GraphTest()
        {
            var graph = new PureObjectGraph<PureObjectBaseTest>();
            var gateway = new PureObjectBaseTest("Gateway", TestLoggerFactory);
            var computer1 = new PureObjectBaseTest("Computer1", TestLoggerFactory);
            var computer2 = new PureObjectBaseTest("Computer2", TestLoggerFactory);

            graph.AddVertex(gateway);
            graph.AddVertex(computer1);
            graph.AddVertex(computer2);

            graph.AddEdge(computer1, gateway, true);
            graph.AddEdge(computer2, gateway, true);

            graph.IndexEdges();
            graph.IndexVertices();
        }
    }
}