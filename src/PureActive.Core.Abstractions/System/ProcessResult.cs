namespace PureActive.Core.Abstractions.System
{
    /// <summary>
    ///     The result of executing a process.
    /// </summary>
    public class ProcessResult
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        public ProcessResult(bool completed, string output)
        {
            Completed = completed;
            Output = output;
        }

        /// <summary>
        ///     Whether or not the process successfully completed without timing out.
        /// </summary>
        public bool Completed { get; }

        /// <summary>
        ///     The combined contents of the stdout/stderr streams of the process.
        /// </summary>
        public string Output { get; }
    }
}