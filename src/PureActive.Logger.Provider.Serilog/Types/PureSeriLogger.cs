using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using PureActive.Logging.Abstractions.Interfaces;
using ILoggerMsft = Microsoft.Extensions.Logging.ILogger;
using Serilog.Context;
using Serilog.Core;
using Serilog.Core.Enrichers;

namespace PureActive.Logger.Provider.Serilog.Types
{
    public class PureSeriLogger : IPureLogger
    {
        public ILoggerMsft WrappedLogger { get; }

        public PureSeriLogger(ILoggerMsft logger)
        {
            WrappedLogger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Wrap ILogger interface methods
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
            Func<TState, Exception, string> formatter) =>
            WrappedLogger?.Log(logLevel, eventId, state, exception, formatter);

        public bool IsEnabled(LogLevel logLevel) => WrappedLogger.IsEnabled(logLevel);

        public IDisposable BeginScope<TState>(TState state) => WrappedLogger.BeginScope(state);

        public IDisposable PushProperty(string propertyName, object value, bool destructureObjects = false)
        {
            return LogContext.PushProperty(propertyName, value, destructureObjects);
        }

        public IDisposable PushProperty<T>(string propertyName, T value, bool destructureObjects = false)
        {
            return LogContext.PushProperty(propertyName, value, destructureObjects);
        }

        public IDisposable PushLogProperties(IEnumerable<KeyValuePair<string, object>> properties, bool destructureObjects = false)
        {
            if (properties == null) throw new ArgumentNullException(nameof(properties));

            return LogContext.Push
            (
                properties.Select
                    (
                        p => new PropertyEnricher(p.Key, p.Value, destructureObjects)
                    ).Cast<ILogEventEnricher>()
                    .ToArray()
            );
        }

        public IDisposable PushLogProperties(IEnumerable<IPureLogPropertyLevel> logPropertyList, LogLevel minimumLogLevel)
        {
            if (logPropertyList == null) throw new ArgumentNullException(nameof(logPropertyList));

            var logPropertyListFiltered = logPropertyList
                .Where(p => p.MinimumLogLevel.CompareTo(minimumLogLevel) >= 0)
                .Select
                (
                    p => new PropertyEnricher(p.Key, p.Value, p.DestructureObject)
                )
                .Cast<ILogEventEnricher>()
                .ToArray();

            return LogContext.Push
            (
                logPropertyListFiltered
            );
        }

        public IDisposable PushLogProperties(IEnumerable<IPureLogPropertyLevel> logPropertyList, Func<IPureLogPropertyLevel, bool> includeLogProperty)
        {
            return LogContext.Push
            (
                logPropertyList
                    .Where(includeLogProperty)
                    .Select
                    (
                        p => new PropertyEnricher(p.Key, p.Value, p.DestructureObject)
                    )
                    .Cast<ILogEventEnricher>()
                    .ToArray()
            );
        }

        public IDisposable PushLogProperties(IEnumerable<IPureLogProperty> logPropertyList)
        {
            if (logPropertyList == null) throw new ArgumentNullException(nameof(logPropertyList));

            return LogContext.Push
            (
                logPropertyList.Select
                    (
                        p => new PropertyEnricher(p.Key, p.Value, p.DestructureObject)
                    ).Cast<ILogEventEnricher>()
                    .ToArray()
            );
        }
    }

    public class PureSeriLogger<T> : PureSeriLogger, IPureLogger<T>
    {
        public PureSeriLogger(ILoggerMsft logger) :base (logger)
        {

        }
    }


}
