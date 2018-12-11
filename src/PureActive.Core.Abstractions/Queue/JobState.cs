// ***********************************************************************
// Assembly         : PureActive.Core.Abstractions
// Author           : SteveBu
// Created          : 11-03-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="JobState.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace PureActive.Core.Abstractions.Queue
{
    /// <summary>
    /// The state of a job in the queue.
    /// </summary>
    public enum JobState
    {
        /// <summary>
        /// Job state unknown or initialized to default
        /// </summary>
        Unknown,
        /// <summary>
        /// Job not found
        /// </summary>
        NotFound,
        /// <summary>
        /// Job not started
        /// </summary>
        NotStarted,
        /// <summary>
        /// Job in progress
        /// </summary>
        InProgress,
        /// <summary>
        /// Job completed
        /// </summary>
        Completed,


    }
}