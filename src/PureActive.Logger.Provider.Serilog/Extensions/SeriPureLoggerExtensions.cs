using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Logging.Abstractions.Types;
using Serilog.Context;
using Serilog.Core;
using Serilog.Core.Enrichers;

namespace PureActive.Logger.Provider.Serilog.Extensions
{
    public static class SeriPureLoggerExtensions
    {
        public static IDisposable With(this IPureLogger logger, string propertyName, object value, bool destructureObjects = false)
        {
            return LogContext.PushProperty(propertyName, value, destructureObjects);
        }

        public static IDisposable WithDeconstruct(this IPureLogger logger, string propertyName, object value)
        {
            return LogContext.PushProperty(propertyName, value, true);
        }

        public static IDisposable With(this IPureLogger logger, IEnumerable<KeyValuePair<string, object>> properties, bool destructureObjects = false)
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

        public static IDisposable WithDeconstruct(this IPureLogger logger, IEnumerable<KeyValuePair<string, object>> properties)
        {
            if (properties == null) throw new ArgumentNullException(nameof(properties));

            return With(logger, properties, true);
        }

        public static IDisposable With(this IPureLogger logger, IEnumerable<IPureLogProperty> logPropertyList)
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

        public static IDisposable With(this IPureLogger logger, IEnumerable<IPureLogPropertyLevel> logPropertyList, LogLevel minimumLogLevel)
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


        public static IDisposable With(this IPureLoggable loggable, LogLevel minimumLogLevel, LoggableFormat loggableFormat = LoggableFormat.ToLog)
        {
            if (loggable == null) throw new ArgumentNullException(nameof(loggable));
            if (loggable.Logger == null) throw new ArgumentNullException(nameof(loggable.Logger));

            return With(loggable.Logger, loggable.GetLogPropertyListLevel(minimumLogLevel, loggableFormat),
                minimumLogLevel);
        }

        public static IDisposable WithParents(this IPureLoggable loggable, LogLevel minimumLogLevel) =>
            With(loggable, minimumLogLevel, LoggableFormat.ToLogWithParents);



        public static IDisposable With(this IPureLoggable loggable, IPureLogLevel pureLogLevel, LoggableFormat loggableFormat = LoggableFormat.ToLog)
        {
            if (loggable == null) throw new ArgumentNullException(nameof(loggable));

            return With(loggable, pureLogLevel.MinimumLogLevel, loggableFormat);
        }

        public static IDisposable WithParents(this IPureLoggable loggable, IPureLogLevel pureLogLevel) =>
            With(loggable, pureLogLevel, LoggableFormat.ToLogWithParents);


        public static IDisposable With(this IPureLogger logger, IEnumerable<IPureLogPropertyLevel> logPropertyList, IPureLogLevel pureLogLevel)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            if (logPropertyList == null) throw new ArgumentNullException(nameof(logPropertyList));
            if (pureLogLevel == null) throw new ArgumentNullException(nameof(pureLogLevel));

            return With(logger, logPropertyList, pureLogLevel.MinimumLogLevel);
        }

        public static IDisposable With(this IPureLogger logger, IPureLogPropertyLevelList logPropertyLevelList, LogLevel minimumLogLevel) =>
            With(logger, logPropertyLevelList.GetLogPropertyLevelList(minimumLogLevel));

        public static IDisposable With(this IPureLogger logger, IEnumerable<IPureLogPropertyLevel> logPropertyList, Func<IPureLogPropertyLevel, bool> includeLogProperty)
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
    }
}
