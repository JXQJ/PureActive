using System;
using System.Collections.Generic;

namespace PureActive.Logging.Abstractions.Interfaces
{
    /// <summary>
    ///     Allows for adding additional properties to all log messages
    ///     logged within newly created scopes.
    /// </summary>
    public interface IPureLogContext
    {
        /// <summary>
        ///     Creates a new log scope, with the given properties.
        /// </summary>
        IDisposable CreateLogScope(IList<KeyValuePair<string, string>> properties);
    }
}