namespace PureActive.Hosting.Abstractions.System
{
    public enum ServiceHostStatus
    {
        Unknown = 0,
        ContinuePending = 5,
        Paused = 7,
        PausePending = 6,
        Running = 4,
        StartPending = 2,
        Stopped = 1,
        StopPending = 3,
    }
}
