using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.States;
using Hangfire.Storage;
using Hangfire.Storage.Monitoring;
using Microsoft.Extensions.Logging;
using PureActive.Core.Abstractions.Queue;
using PureActive.Core.Extensions;
using PureActive.Logging.Abstractions.Interfaces;

namespace PureActive.Queue.Hangfire.Queue
{
    /// <summary>
    ///     A client to manage jobs in a job queue.
    /// </summary>
    public class JobQueueClient : IJobQueueClient
    {
        /// <summary>
        ///     The background job client.
        /// </summary>
        private readonly IBackgroundJobClient _backgroundJobClient;

        /// <summary>
        ///     The monitoring API.
        /// </summary>
        private readonly IMonitoringApi _monitoringApi;

        /// <summary>
        ///     Constructor.
        /// </summary>
        public JobQueueClient(
            IBackgroundJobClient backgroundJobClient,
            JobStorage jobStorage)
        {
            _backgroundJobClient = backgroundJobClient;
            _monitoringApi = jobStorage.GetMonitoringApi();
        }

        /// <summary>
        ///     Enqueues a job.
        /// </summary>
        /// <typeparam name="TInterface">An interface containing the method to run.</typeparam>
        /// <param name="job">
        ///     A call to a method on the given interface.
        ///     All parameters to the method must be serializable.
        /// </param>
        /// <returns>The job ID.</returns>
        public Task<string> EnqueueAsync<TInterface>(Expression<Func<TInterface, Task>> job)
        {
            // TODO: Make this actually asynchronous once HangFire supports it.

            var jobId = _backgroundJobClient.Enqueue(job);

            return Task.FromResult(jobId);
        }

        /// <summary>
        ///     Returns the status of the job with the given ID.
        /// </summary>
        public Task<JobStatus> GetJobStatusAsync(string jobId)
        {
            // TODO: Make this actually asynchronous once HangFire supports it.

            var jobDetails = _monitoringApi.JobDetails(jobId)
                .History
                .OrderByDescending(h => h.CreatedAt)
                .FirstOrDefault();

            var jobState = GetJobState(jobDetails);
            var enteredState = jobDetails?.CreatedAt ?? DateTime.MinValue;

            return Task.FromResult(new JobStatus(jobState, enteredState));
        }


        /// <summary>
        /// Waits for the Job to Complete
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="timeout"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public JobStatus WaitForJobToComplete(string jobId, int timeout, CancellationToken cancellationToken)
        {
        
        }


   
        /// <summary>
        ///     Returns the job state for the given job.
        /// </summary>
        private JobState GetJobState(StateHistoryDto jobDetails)
        {
            var stateName = jobDetails?.StateName;

            if (stateName == EnqueuedState.StateName)
                return JobState.NotStarted;

            if (stateName == SucceededState.StateName)
                return JobState.Succeeded;

            return stateName == ProcessingState.StateName ? JobState.InProgress : JobState.Unknown;
        }
    }
}