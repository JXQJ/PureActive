using FluentAssertions;
using Microsoft.Extensions.Logging;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Logging.Abstractions.Types;
using PureActive.Network.Devices.PureObject;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Network.Devices.UnitTests.PureObject
{
    [Trait("Category", "Unit")]
    public class PureObjectBaseUnitTests : TestBaseLoggable<PureObjectBaseUnitTests>
    {
        public PureObjectBaseUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        private class PureObjectTest : PureObjectBase
        {
            public PureObjectTest(IPureLoggerFactory loggerFactory): base(loggerFactory)
            {
               
            }

            public ulong IncreaseObjectVersion()
            {
                return IncrementObjectVersion();
            }
        }

        [Fact]
        public void PureObjectBase_ToString()
        {
            var objectBase  = new PureObjectTest(TestLoggerFactory);

            TestOutputHelper.WriteLine(objectBase.ToString());
        }

        [Fact]
        public void PureObjectBase_ToStringLoggable()
        {
            var objectBase = new PureObjectTest(TestLoggerFactory);

            TestOutputHelper.WriteLine(objectBase.ToString(LogLevel.Debug, LoggableFormat.ToStringWithParents));
        }

        [Fact]
        public void PureObjectBase_Comparison()
        {
            var objectBase1 = new PureObjectTest(TestLoggerFactory);
            var objectBase2 = new PureObjectTest(TestLoggerFactory);

            Assert.False(objectBase1.Equals(objectBase2), "objectBase1.Equals(objectBase2)");
            Assert.False(objectBase1.IsSameObjectId(objectBase2), "objectBase1.IsSameObjectId(objectBase2)");
            Assert.True(objectBase1.IsSameObjectVersion(objectBase2), "objectBase1.IsSameObjectVersion(objectBase2)");
        }

        [Fact]
        public void PureObjectBase_Equals()
        {
            var objectBase1 = new PureObjectTest(TestLoggerFactory);
            var objectBase2 = new PureObjectTest(TestLoggerFactory) {ObjectId = objectBase1.ObjectId};

            // ObjectId's are same but Creation and Modification dates are different
            Assert.False(objectBase1.Equals(objectBase2), "objectBase1.Equals(objectBase2)");
            Assert.True(objectBase1.IsSameObjectId(objectBase2), "objectBase1.IsSameObjectId(objectBase2)");
            Assert.True(objectBase1.IsSameObjectVersion(objectBase2), "objectBase1.IsSameObjectVersion(objectBase2)");
        }

        [Fact]
        public void PureObjectBase_Clone()
        {
            var objectBase1 = new PureObjectTest(TestLoggerFactory);
            var objectBase2 = objectBase1.CloneInstance();

            // Objects version is the same but everyone else
            Assert.False(objectBase1.Equals(objectBase2), "objectBase1.Equals(objectBase2)");
            Assert.False(objectBase1.IsSameObjectId(objectBase2), "objectBase1.IsSameObjectId(objectBase2)");
            Assert.True(objectBase1.IsSameObjectVersion(objectBase2), "objectBase1.IsSameObjectVersion(objectBase2)");
        }
        [Fact]
        public void PureObjectBase_CopyInstance()
        {
            var objectBase1 = new PureObjectTest(TestLoggerFactory);
            var objectBase2 = objectBase1.CopyInstance();

            // Objects version is the same but everyone else
            Assert.True(objectBase1.Equals(objectBase2), "objectBase1.Equals(objectBase2)");
            Assert.True(objectBase1.IsSameObjectId(objectBase2), "objectBase1.IsSameObjectId(objectBase2)");
            Assert.True(objectBase1.IsSameObjectVersion(objectBase2), "objectBase1.IsSameObjectVersion(objectBase2)");
        }

        [Fact]
        public void PureObjectBase_IncrementObjectVersion()
        {
            var objectBase1 = new PureObjectTest(TestLoggerFactory);
            var objectVersion = objectBase1.ObjectVersion;

            objectBase1.IncreaseObjectVersion().Should().Be(objectVersion + 1);
            objectBase1.ObjectVersion.Should().Be(objectVersion + 1);
        }
    }
}