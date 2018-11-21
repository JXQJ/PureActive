// ***********************************************************************
// Assembly         : PureActive.Core.Abstractions
// Author           : SteveBu
// Created          : 11-03-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-03-2018
// ***********************************************************************
// <copyright file="JobStatus.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace PureActive.Core.Abstractions.Queue
{
    /// <summary>
    /// The status of a job in the queue.
    /// </summary>
    public class JobStatus
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="enteredState">State of the entered.</param>
        public JobStatus(JobState state, DateTimeOffset enteredState)
        {
            State = state;
            EnteredState = enteredState;
        }

        /// <summary>
        /// The state of the job.
        /// </summary>
        /// <value>The state.</value>
        public JobState State { get; }

        /// <summary>
        /// The time at which the job entered
        /// its current state.
        /// </summary>
        /// <value>The state of the entered.</value>
        public DateTimeOffset EnteredState { get; }
    }
}