// ***********************************************************************
// Assembly         : PureActive.Core.Abstractions
// Author           : SteveBu
// Created          : 11-03-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-03-2018
// ***********************************************************************
// <copyright file="IJobQueueClient.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace PureActive.Core.Abstractions.Queue
{
    /// <summary>
    /// A client to manage jobs in a job queue.
    /// </summary>
    public interface IJobQueueClient
    {
        /// <summary>
        /// Enqueues a job.
        /// </summary>
        /// <typeparam name="TInterface">An interface containing the method to run.</typeparam>
        /// <param name="job">A call to a method on the given interface.
        /// All parameters to the method must be serializable.</param>
        /// <returns>The job ID.</returns>
        Task<string> EnqueueAsync<TInterface>(Expression<Func<TInterface, Task>> job);

        /// <summary>
        /// Returns the status of the job with the given ID.
        /// </summary>
        /// <param name="jobId">The job identifier.</param>
        /// <returns>Task&lt;JobStatus&gt;.</returns>
        Task<JobStatus> GetJobStatusAsync(string jobId);

        /// <summary>
        /// Waits for a Job to Complete
        /// </summary>
        /// <param name="jobId">ID of job</param>
        /// <param name="timeout">Timeout in milliseconds</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        Task<JobStatus> WaitForJobToComplete(string jobId, int timeout, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        bool DeleteBackgroundJob(string jobId);

        /// <summary>
        /// Removes all recurring jobs from queue
        /// </summary>
        void DeleteAllEnqueuedJobs(string queue = "default");
    }
}