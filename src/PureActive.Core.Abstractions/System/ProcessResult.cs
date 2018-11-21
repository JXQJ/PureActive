// ***********************************************************************
// Assembly         : PureActive.Core.Abstractions
// Author           : SteveBu
// Created          : 10-31-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-01-2018
// ***********************************************************************
// <copyright file="ProcessResult.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace PureActive.Core.Abstractions.System
{
    /// <summary>
    /// The result of executing a process.
    /// </summary>
    public class ProcessResult
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="completed">if set to <c>true</c> [completed].</param>
        /// <param name="output">The output.</param>
        public ProcessResult(bool completed, string output)
        {
            Completed = completed;
            Output = output;
        }

        /// <summary>
        /// Whether or not the process successfully completed without timing out.
        /// </summary>
        /// <value><c>true</c> if completed; otherwise, <c>false</c>.</value>
        public bool Completed { get; }

        /// <summary>
        /// The combined contents of the stdout/stderr streams of the process.
        /// </summary>
        /// <value>The output.</value>
        public string Output { get; }
    }
}