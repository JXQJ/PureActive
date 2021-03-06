// ***********************************************************************
// Assembly         : PureActive.Core.Reactive.UnitTests
// Author           : SteveBu
// Created          : 11-17-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="ReactiveUnitTests.cs" company="BushChang Corporation">
//     � 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Reactive.Linq;
using PureActive.Core.Reactive.Extensions;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Core.Reactive.UnitTests
{
    /// <summary>
    /// Class ReactiveUnitTests.
    /// Implements the <see cref="Serilog.Sink.Xunit.TestBase.TestBaseLoggable{ReactiveUnitTests}" />
    /// </summary>
    /// <seealso cref="Serilog.Sink.Xunit.TestBase.TestBaseLoggable{ReactiveUnitTests}" />
    /// <autogeneratedoc />
    [Trait("Category", "Unit")]
    public class ReactiveUnitTests : TestBaseLoggable<ReactiveUnitTests>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReactiveUnitTests"/> class.
        /// </summary>
        /// <param name="testOutputHelper">The test output helper.</param>
        /// <autogeneratedoc />
        public ReactiveUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        /// <summary>
        /// Defines the test method LogToConsole.
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public void Reactive_LogToConsole()
        {
            Observable.Range(1, 5)
                .LogToConsole("Range")
                .Where(x => x % 2 == 0)
                .LogToConsole("Where")
                .Select(x => x * 3)
                .SubscribeConsole("final");
        }

        /// <summary>
        /// Defines the test method LogToConsole with Exception
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public void Reactive_LogToConsole_Exception()
        {
            Observable.Throw<int>(new InvalidOperationException())
            .LogToConsole("Throws")
            .SubscribeConsole("Throws");
        }
    }
}