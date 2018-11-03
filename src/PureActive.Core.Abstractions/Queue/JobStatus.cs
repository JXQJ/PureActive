using System;

namespace PureActive.Core.Abstractions.Queue
{
    /// <summary>
    ///     The status of a job in the queue.
    /// </summary>
    public class JobStatus
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        public JobStatus(JobState state, DateTimeOffset enteredState)
        {
            State = state;
            EnteredState = enteredState;
        }

        /// <summary>
        ///     The state of the job.
        /// </summary>
        public JobState State { get; }

        /// <summary>
        ///     The time at which the job entered
        ///     its current state.
        /// </summary>
        public DateTimeOffset EnteredState { get; }
    }
}