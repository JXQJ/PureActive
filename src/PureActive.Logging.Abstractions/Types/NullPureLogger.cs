using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Abstractions.Internal;
using PureActive.Logging.Abstractions.Interfaces;

namespace PureActive.Logging.Abstractions.Types
{
    public class NullPureLogger : IPureLogger
    {
       public static NullPureLogger Instance { get; } = new NullPureLogger();

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            NullLogger.Instance.Log(logLevel, eventId, state, exception, formatter);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return NullLogger.Instance.IsEnabled(logLevel);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return NullLogger.Instance.BeginScope(state);
        }

        public IDisposable PushProperty(string propertyName, object value, bool destructureObjects = false)
        {
            return BeginScope(new Dictionary<string, object> { { propertyName, value } });
        }

        public IDisposable PushProperty<T>(string propertyName, T value, bool destructureObjects = false)
        {
            return BeginScope(new Dictionary<string, T> { { propertyName, value } });
        }

        private IDisposable PushEmptyLogProperty() => NullScope.Instance;

        public IDisposable PushLogProperties(IEnumerable<IPureLogProperty> logPropertyList) => PushEmptyLogProperty();

        public IDisposable PushLogProperties(IEnumerable<KeyValuePair<string, object>> properties, bool destructureObjects = false) => PushEmptyLogProperty();

        public IDisposable PushLogProperties(IEnumerable<IPureLogPropertyLevel> logPropertyList, LogLevel minimumLogLevel) => PushEmptyLogProperty();

        public IDisposable PushLogProperties(IEnumerable<IPureLogPropertyLevel> logPropertyList, Func<IPureLogPropertyLevel, bool> includeLogProperty) => PushEmptyLogProperty();
    }

    public class NullPureLogger<TCategory> : NullPureLogger, IPureLogger<TCategory>
    {

    }
}
