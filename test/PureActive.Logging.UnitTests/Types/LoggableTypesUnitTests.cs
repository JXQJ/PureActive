﻿// ***********************************************************************
// Assembly         : PureActive.Logging.UnitTests
// Author           : SteveBu
// Created          : 11-24-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-24-2018
// ***********************************************************************
// <copyright file="LoggableTypesUnitTests.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using FluentAssertions;
using PureActive.Logging.Abstractions.Types;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Logging.UnitTests.Types
{
    /// <summary>
    /// Class LoggableTypesUnitTests.
    /// Implements the <see cref="PureActive.Serilog.Sink.Xunit.TestBase.TestBaseLoggable{LoggableTypesUnitTests}" />
    /// </summary>
    /// <seealso cref="PureActive.Serilog.Sink.Xunit.TestBase.TestBaseLoggable{LoggableTypesUnitTests}" />
    /// <autogeneratedoc />
    [Trait("Category", "Unit")]
    public class LoggableTypesUnitTests : TestBaseLoggable<LoggableTypesUnitTests>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoggableTypesUnitTests"/> class.
        /// </summary>
        /// <param name="testOutputHelper">The test output helper.</param>
        /// <autogeneratedoc />
        public LoggableTypesUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        /// <summary>
        /// Defines the test method LoggableFormatUtils_IsWithParents.
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public void LoggableFormatUtils_IsWithParents()
        {
            LoggableFormat.ToLogWithParents.IsWithParents().Should().BeTrue();
            LoggableFormat.ToLog.IsWithParents().Should().BeFalse();
        }

        /// <summary>
        /// Defines the test method LoggableFormatUtils_IsToLog
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public void LoggableFormatUtils_ToLog()
        {
            LoggableFormat.ToLogWithParents.IsToLog().Should().BeTrue();
            LoggableFormat.WithParents.IsToLog().Should().BeFalse();
        }

        /// <summary>
        /// Defines the test method LoggableFormatUtils_IsToString
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public void LoggableFormatUtils_ToString()
        {
            LoggableFormat.ToStringWithParents.IsToString().Should().BeTrue();
            LoggableFormat.ToLogWithParents.IsToString().Should().BeFalse();
        }
    }
}