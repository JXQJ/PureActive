﻿// ***********************************************************************
// Assembly         : PureActive.Network.Devices.UnitTests
// Author           : SteveBu
// Created          : 11-17-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="PureObjectBaseUnitTests.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
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
    /// <summary>
    /// Class PureObjectBaseUnitTests.
    /// Implements the <see cref="Serilog.Sink.Xunit.TestBase.TestBaseLoggable{PureObjectBaseUnitTests}" />
    /// </summary>
    /// <seealso cref="Serilog.Sink.Xunit.TestBase.TestBaseLoggable{PureObjectBaseUnitTests}" />
    /// <autogeneratedoc />
    [Trait("Category", "Unit")]
    public class PureObjectBaseUnitTests : TestBaseLoggable<PureObjectBaseUnitTests>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PureObjectBaseUnitTests" /> class.
        /// </summary>
        /// <param name="testOutputHelper">The test output helper.</param>
        /// <autogeneratedoc />
        public PureObjectBaseUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        /// <summary>
        /// Class PureObjectTest.
        /// Implements the <see cref="PureObjectBase" />
        /// </summary>
        /// <seealso cref="PureObjectBase" />
        /// <autogeneratedoc />
        private class PureObjectTest : PureObjectBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="PureObjectTest" /> class.
            /// </summary>
            /// <param name="loggerFactory">The logger factory.</param>
            /// <autogeneratedoc />
            public PureObjectTest(IPureLoggerFactory loggerFactory) : base(loggerFactory)
            {
            }

            /// <summary>
            /// Increases the object version.
            /// </summary>
            /// <returns>System.UInt64.</returns>
            /// <autogeneratedoc />
            public ulong IncreaseObjectVersion()
            {
                return IncrementObjectVersion();
            }
        }

        /// <summary>
        /// Defines the test method PureObjectBase_Clone.
        /// </summary>
        /// <autogeneratedoc />
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

        /// <summary>
        /// Defines the test method PureObjectBase_Comparison.
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public void PureObjectBase_Comparison()
        {
            var objectBase1 = new PureObjectTest(TestLoggerFactory);
            var objectBase2 = new PureObjectTest(TestLoggerFactory);

            Assert.False(objectBase1.Equals(objectBase2), "objectBase1.Equals(objectBase2)");
            Assert.False(objectBase1.IsSameObjectId(objectBase2), "objectBase1.IsSameObjectId(objectBase2)");
            Assert.True(objectBase1.IsSameObjectVersion(objectBase2), "objectBase1.IsSameObjectVersion(objectBase2)");
        }

        /// <summary>
        /// Defines the test method PureObjectBase_CopyInstance.
        /// </summary>
        /// <autogeneratedoc />
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

        /// <summary>
        /// Defines the test method PureObjectBase_Equals using CopyInstance
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public void PureObjectBase_Equals_CopyInstance()
        {
            var objectBase1 = new PureObjectTest(TestLoggerFactory);
            var objectBase2 = objectBase1.CopyInstance();

            // ObjectId's are same but Creation and Modification dates are different
            Assert.True(objectBase1.Equals(objectBase2), "objectBase1.Equals(objectBase2)");
            Assert.True(objectBase1.IsSameObjectId(objectBase2), "objectBase1.IsSameObjectId(objectBase2)");
            Assert.True(objectBase1.IsSameObjectVersion(objectBase2), "objectBase1.IsSameObjectVersion(objectBase2)");
        }

        /// <summary>
        /// Defines the test method PureObjectBase_Equals using CloneInstance
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public void PureObjectBase_Equals_CloneInstance()
        {
            var objectBase1 = new PureObjectTest(TestLoggerFactory);
            var objectBase2 = objectBase1.CloneInstance();

            // ObjectId's are same but Creation and Modification dates are different
            Assert.False(objectBase1.Equals(objectBase2), "objectBase1.Equals(objectBase2)");
            Assert.False(objectBase1.IsSameObjectId(objectBase2), "objectBase1.IsSameObjectId(objectBase2)");
            Assert.True(objectBase1.IsSameObjectVersion(objectBase2), "objectBase1.IsSameObjectVersion(objectBase2)");
        }

        /// <summary>
        /// Defines the test method PureObjectBase_IncrementObjectVersion.
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public void PureObjectBase_IncrementObjectVersion()
        {
            var objectBase1 = new PureObjectTest(TestLoggerFactory);
            var objectVersion = objectBase1.ObjectVersion;

            objectBase1.IncreaseObjectVersion().Should().Be(objectVersion + 1);
            objectBase1.ObjectVersion.Should().Be(objectVersion + 1);
        }

        /// <summary>
        /// Defines the test method PureObjectBase_ToString.
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public void PureObjectBase_ToString()
        {
            var objectBase = new PureObjectTest(TestLoggerFactory);

            TestOutputHelper.WriteLine(objectBase.ToString());
        }

        /// <summary>
        /// Defines the test method PureObjectBase_ToStringLoggable.
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public void PureObjectBase_ToStringLoggable()
        {
            var objectBase = new PureObjectTest(TestLoggerFactory);

            TestOutputHelper.WriteLine(objectBase.ToString(LogLevel.Debug, LoggableFormat.ToStringWithParents));
        }
    }
}