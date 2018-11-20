namespace PureActive.Core.Abstractions.Queue
{
    /// <summary>
    ///     The state of a job in the queue.
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
        ///  Job completed
        /// </summary>
        Completed,
    }
}