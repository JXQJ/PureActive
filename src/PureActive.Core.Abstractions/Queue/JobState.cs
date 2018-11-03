namespace PureActive.Core.Abstractions.Queue
{
    /// <summary>
    ///     The state of a job in the queue.
    /// </summary>
    public enum JobState
    {
        NotFound,
        NotStarted,
        InProgress,
        Completed,
        Unknown
    }
}