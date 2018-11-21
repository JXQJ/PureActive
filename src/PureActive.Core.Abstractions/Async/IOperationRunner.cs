// ***********************************************************************
// Assembly         : PureActive.Core.Abstractions
// Author           : SteveBu
// Created          : 11-01-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-01-2018
// ***********************************************************************
// <copyright file="IOperationRunner.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PureActive.Core.Abstractions.Async
{
    /// <summary>
    /// Functionality to execute one or more generic operations
    /// with fallback behavior.
    /// </summary>
    public interface IOperationRunner
    {
        /// <summary>
        /// Executes a set of operations in parallel, with a limit
        /// on the maximum number of simultaneous operations.
        /// </summary>
        /// <typeparam name="TSource">The type of the t source.</typeparam>
        /// <typeparam name="TResult">The type of the t result.</typeparam>
        /// <param name="sources">The sources.</param>
        /// <param name="operation">The operation.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="maxSimultaneous">The maximum simultaneous.</param>
        /// <returns>Task&lt;IList&lt;TResult&gt;&gt;.</returns>
        Task<IList<TResult>> RunOperationsAsync<TSource, TResult>(
            IEnumerable<TSource> sources,
            Func<TSource, Task<TResult>> operation,
            CancellationToken cancellationToken,
            int maxSimultaneous = 5);

        /// <summary>
        /// Executes an operation, retrying the operation if needed.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="shouldRetry">The should retry.</param>
        /// <param name="numAttempts">The number attempts.</param>
        /// <param name="delayBetweenRetries">The delay between retries.</param>
        /// <param name="defaultResultIfFailed">if set to <c>true</c> [default result if failed].</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task.</returns>
        Task RetryOperationIfNeededAsync(
            Func<Task> operation,
            Func<Exception, bool> shouldRetry,
            int numAttempts,
            TimeSpan delayBetweenRetries,
            bool defaultResultIfFailed,
            CancellationToken cancellationToken);

        /// <summary>
        /// Executes an operation, retrying the operation if needed.
        /// </summary>
        /// <typeparam name="TResult">The type of the t result.</typeparam>
        /// <param name="operation">The operation.</param>
        /// <param name="shouldRetry">The should retry.</param>
        /// <param name="numAttempts">The number attempts.</param>
        /// <param name="delayBetweenRetries">The delay between retries.</param>
        /// <param name="defaultResultIfFailed">if set to <c>true</c> [default result if failed].</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task&lt;TResult&gt;.</returns>
        Task<TResult> RetryOperationIfNeededAsync<TResult>(
            Func<Task<TResult>> operation,
            Func<Exception, bool> shouldRetry,
            int numAttempts,
            TimeSpan delayBetweenRetries,
            bool defaultResultIfFailed,
            CancellationToken cancellationToken);

        /// <summary>
        /// Runs a given operation with a timeout. Returns whether the
        /// operation completed within the given amount of time.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="timeout">The timeout.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        Task<bool> RunOperationWithTimeoutAsync(
            Func<Task> operation,
            TimeSpan timeout,
            CancellationToken cancellationToken);
    }
}