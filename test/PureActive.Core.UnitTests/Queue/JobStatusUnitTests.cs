// ***********************************************************************
// Assembly         : PureActive.Core.UnitTests
// Author           : SteveBu
// Created          : 11-17-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 12-01-2018
// ***********************************************************************
// <copyright file="JobStatusUnitTests.cs" company="BushChang Corporation">
//     � 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using FluentAssertions;
using PureActive.Core.Abstractions.Queue;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Core.UnitTests.Queue
{
    /// <summary>
    /// Class JobStatusUnitTests.
    /// Implements the <see cref="Serilog.Sink.Xunit.TestBase.TestBaseLoggable{JobStatusUnitTests}" />
    /// </summary>
    /// <seealso cref="Serilog.Sink.Xunit.TestBase.TestBaseLoggable{JobStatusUnitTests}" />
    /// <autogeneratedoc />
    [Trait("Category", "Unit")]
    public class JobStatusUnitTests : TestBaseLoggable<JobStatusUnitTests>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JobStatusUnitTests"/> class.
        /// </summary>
        /// <param name="testOutputHelper">The test output helper.</param>
        /// <autogeneratedoc />
        public JobStatusUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        /// <summary>
        /// Defines the test method JobStatus_Constructor.
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public void JobStatus_Constructor()
        {
            var dateTimeOffset = DateTimeOffset.UtcNow;
            var jobStatus = new JobStatus(JobState.NotStarted, dateTimeOffset);

            jobStatus.EnteredState.Should().Be(dateTimeOffset);
            jobStatus.State.Should().Be(JobState.NotStarted);
        }
    }
}