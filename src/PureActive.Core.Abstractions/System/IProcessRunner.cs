// ***********************************************************************
// Assembly         : PureActive.Core.Abstractions
// Author           : SteveBu
// Created          : 10-31-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-01-2018
// ***********************************************************************
// <copyright file="IProcessRunner.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Threading.Tasks;

namespace PureActive.Core.Abstractions.System
{
    /// <summary>
    /// Runs a process.
    /// </summary>
    public interface IProcessRunner
    {
        /// <summary>
        /// Runs a process, optionally killing the process if it has not completed
        /// in the given timeout. Returns the combined contents of stdout/stderr,
        /// along with whether the job completed in the given timeout.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="args">The arguments.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>Task&lt;ProcessResult&gt;.</returns>
        Task<ProcessResult> RunProcessAsync(
            string path,
            string[] args,
            TimeSpan? timeout);
    }
}