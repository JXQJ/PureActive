using Microsoft.Extensions.Logging;

namespace PureActive.Logging.Abstractions.Interfaces
{
    public interface IPureLogger : ILogger
    {

    }

    /// <summary>
    /// A generic interface for logging where the category name is derived from the specified
    /// <typeparamref name="TCategoryName" /> type name.
    /// Generally used to enable activation of a named <see cref="T:Microsoft.Extensions.Logging.ILogger" /> from dependency injection.
    /// </summary>
    /// <typeparam name="TCategoryName">The type who's name is used for the logger category name.</typeparam>
    public interface IPureLogger<out TCategoryName> : IPureLogger, ILogger<TCategoryName>
    {

    }
}
