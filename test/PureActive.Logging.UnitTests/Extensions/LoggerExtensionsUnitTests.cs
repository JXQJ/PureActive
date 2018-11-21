// ***********************************************************************
// Assembly         : PureActive.Logging.UnitTests
// Author           : SteveBu
// Created          : 11-17-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="LoggerExtensionsUnitTests.cs" company="BushChang Corporation">
//     � 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Microsoft.Extensions.Logging;
using PureActive.Logging.Extensions.Extensions;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Logging.UnitTests.Extensions
{
    /// <summary>
    /// Class LoggerExtensionsUnitTests.
    /// Implements the <see cref="Serilog.Sink.Xunit.TestBase.TestBaseLoggable{LoggerExtensionsUnitTests}" />
    /// </summary>
    /// <seealso cref="Serilog.Sink.Xunit.TestBase.TestBaseLoggable{LoggerExtensionsUnitTests}" />
    /// <autogeneratedoc />
    [Trait("Category", "Unit")]
    public class LoggerExtensionsUnitTests : TestBaseLoggable<LoggerExtensionsUnitTests>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerExtensionsUnitTests"/> class.
        /// </summary>
        /// <param name="testOutputHelper">The test output helper.</param>
        /// <autogeneratedoc />
        public LoggerExtensionsUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        /// <summary>
        /// Defines the test method LoggerExtensions_BeginPropertyScope.
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public void LoggerExtensions_BeginPropertyScope()
        {
            using (Logger.BeginPropertyScope("PropertyInt", 14))
            {
                Logger.LogDebug("Log Int Property Scope");
            }
        }
    }
}