using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace PureActive.Logging.Abstractions.Interfaces
{
    public interface IPureLogger : ILogger
    {
        IDisposable PushProperty(string propertyName, object value, bool destructureObjects = false);
        IDisposable PushProperty<T>(string propertyName, T value, bool destructureObjects = false);

        IDisposable PushLogProperties(IEnumerable<IPureLogProperty> logPropertyList);

        IDisposable PushLogProperties(IEnumerable<KeyValuePair<string, object>> properties,
            bool destructureObjects = false);

        IDisposable PushLogProperties(IEnumerable<IPureLogPropertyLevel> logPropertyList, LogLevel minimumLogLevel);


        IDisposable PushLogProperties(IEnumerable<IPureLogPropertyLevel> logPropertyList,
            Func<IPureLogPropertyLevel, bool> includeLogProperty);
    }

    /// <summary>
    ///     A generic interface for logging where the category name is derived from the specified
    ///     <typeparamref name="TCategoryName" /> type name.
    ///     Generally used to enable activation of a named <see cref="T:Microsoft.Extensions.Logging.ILogger" /> from
    ///     dependency injection.
    /// </summary>
    /// <typeparam name="TCategoryName">The type who's name is used for the logger category name.</typeparam>
    public interface IPureLogger<out TCategoryName> : IPureLogger, ILogger<TCategoryName>
    {
    }
}