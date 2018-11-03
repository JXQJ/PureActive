namespace PureActive.Logging.Abstractions.Interfaces
{
    /// <summary>
    ///     Provides the operation ID of the current request.
    /// </summary>
    public interface IOperationIdProvider
    {
        /// <summary>
        ///     The operation ID of the current request.
        /// </summary>
        string OperationId { get; }
    }
}