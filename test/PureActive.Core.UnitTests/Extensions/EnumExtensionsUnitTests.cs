// ***********************************************************************
// Assembly         : PureActive.Core.UnitTests
// Author           : SteveBu
// Created          : 11-17-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="EnumExtensionsUnitTests.cs" company="BushChang Corporation">
//     � 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.ComponentModel;
using FluentAssertions;
using PureActive.Core.Extensions;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Core.UnitTests.Extensions
{
    /// <summary>
    /// Class EnumExtensionsUnitTests.
    /// Implements the <see cref="Serilog.Sink.Xunit.TestBase.TestBaseLoggable{EnumExtensionsUnitTests}" />
    /// </summary>
    /// <seealso cref="Serilog.Sink.Xunit.TestBase.TestBaseLoggable{EnumExtensionsUnitTests}" />
    /// <autogeneratedoc />
    [Trait("Category", "Unit")]
    public class EnumExtensionsUnitTests : TestBaseLoggable<EnumExtensionsUnitTests>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumExtensionsUnitTests"/> class.
        /// </summary>
        /// <param name="testOutputHelper">The test output helper.</param>
        /// <autogeneratedoc />
        public EnumExtensionsUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        /// <summary>
        /// Defines the test method EnumExtensions_ToEnumString.
        /// </summary>
        /// <param name="testString">The test string.</param>
        /// <param name="expectedString">The expected string.</param>
        /// <autogeneratedoc />
        [Theory]
        [InlineData("First Second", "First_Second")]
        [InlineData(" ", null)]
        [InlineData("\t", null)]
        [InlineData("\r\n", null)]
        public void EnumExtensions_ToEnumString(string testString, string expectedString)
        {
            testString.ToEnumString().Should().Be(expectedString);
        }

        /// <summary>
        /// Defines the test method EnumExtensions_FromEnumString.
        /// </summary>
        /// <param name="testEnum">The test enum.</param>
        /// <param name="expectedString">The expected string.</param>
        /// <autogeneratedoc />
        [Theory]
        [InlineData(TestEnum.First_Second, "First Second")]
        [InlineData(TestEnum.First_Second_Third, "First Second Third")]
        public void EnumExtensions_FromEnumString(Enum testEnum, string expectedString)
        {
            testEnum.FromEnumValue().Should().Be(expectedString);
        }

        /// <summary>
        /// Defines the test method EnumExtensions_GetDescription.
        /// </summary>
        /// <param name="testEnum">The test enum.</param>
        /// <param name="expectedString">The expected string.</param>
        /// <autogeneratedoc />
        [Theory]
        [InlineData(TestEnum.First_Second, "First Second")]
        [InlineData(TestEnum.First_Second_Third, "First Second Third")]
        public void EnumExtensions_GetDescription(Enum testEnum, string expectedString)
        {
            testEnum.GetDescription().Should().Be(expectedString);
        }

        // ReSharper disable InconsistentNaming
        /// <summary>
        /// Enum TestEnum
        /// </summary>
        /// <autogeneratedoc />
        private enum TestEnum
        {
            /// <summary>
            /// The first second
            /// </summary>
            /// <autogeneratedoc />
            [Description("First Second")] First_Second,

            /// <summary>
            /// The first second third
            /// </summary>
            /// <autogeneratedoc />
            [Description("First Second Third")] First_Second_Third
        }
        // ReSharper restore InconsistentNaming
    }
}