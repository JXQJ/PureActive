// ***********************************************************************
// Assembly         : PureActive.Hosting.UnitTests
// Author           : SteveBu
// Created          : 11-17-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="HostingUnitTests.cs" company="BushChang Corporation">
//     � 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Hosting.UnitTests
{
    /// <summary>
    /// Class HostingUnitTests.
    /// Implements the <see cref="Serilog.Sink.Xunit.TestBase.TestBaseLoggable{HostingUnitTests}" />
    /// </summary>
    /// <seealso cref="Serilog.Sink.Xunit.TestBase.TestBaseLoggable{HostingUnitTests}" />
    /// <autogeneratedoc />
    [Trait("Category", "Unit")]
    public class HostingUnitTests : TestBaseLoggable<HostingUnitTests>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HostingUnitTests"/> class.
        /// </summary>
        /// <param name="testOutputHelper">The test output helper.</param>
        /// <autogeneratedoc />
        public HostingUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        /// <summary>
        /// Defines the test method Hosting_Constructor.
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public void Hosting_Constructor()
        {
        }
    }
}