using Microsoft.Extensions.Logging;

namespace PureActive.Logging.Abstractions.Interfaces
{
    public interface IPureLoggerFactory : ILoggerFactory
    {
        /// <summary>
        /// Creates a new <see cref="T:PureActive.Logging.Abstractions.Interfaces.IPureLogger" /> instance.
        /// </summary>
        /// <param name="categoryName">The category name for messages produced by the logger.</param>
        /// <returns>The <see cref="T:PureActive.Logging.Abstractions.Interfaces.IPureLogger" />.</returns>
        IPureLogger CreatePureLogger(string categoryName);

        IPureLogger<T> CreatePureLogger<T>();

        ILogger<T> CreateLogger<T>();
    }
}
