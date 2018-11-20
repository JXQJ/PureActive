using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Logging.Abstractions.Types;

namespace PureActive.Logging.Extensions.Extensions
{
    public static class PureLoggerExtensions
    {
        public static IDisposable BeginPropertyScope<T>(this IPureLogger logger, string propertyName, T value)
        {
            return logger?.BeginScope(new Dictionary<string, T> {{propertyName, value}});
        }

        public static IDisposable PushLogProperty(this IPureLogger logger, string propertyName, object value,
            bool destructureObjects = false)
        {
            return logger.PushProperty(propertyName, value, destructureObjects);
        }

        public static IDisposable PushLogPropertyDeconstruct(this IPureLogger logger, string propertyName, object value)
        {
            return logger.PushProperty(propertyName, value, true);
        }


        public static IDisposable PushLogPropertyDeconstruct(this IPureLogger logger,
            IEnumerable<KeyValuePair<string, object>> properties)
        {
            if (properties == null) throw new ArgumentNullException(nameof(properties));

            return logger.PushLogProperties(properties, true);
        }


        public static IDisposable PushLogProperties(this IPureLoggable loggable, LogLevel minimumLogLevel,
            LoggableFormat loggableFormat = LoggableFormat.ToLog)
        {
            if (loggable == null) throw new ArgumentNullException(nameof(loggable));
            if (loggable.Logger == null) throw new ArgumentNullException(nameof(loggable.Logger));

            return loggable.Logger.PushLogProperties(loggable.GetLogPropertyListLevel(minimumLogLevel, loggableFormat),
                minimumLogLevel);
        }

        public static IDisposable PushLogPropertiesParents(this IPureLoggable loggable, LogLevel minimumLogLevel) =>
            PushLogProperties(loggable, minimumLogLevel, LoggableFormat.ToLogWithParents);


        public static IDisposable PushLogProperties(this IPureLoggable loggable, IPureLogLevel pureLogLevel,
            LoggableFormat loggableFormat = LoggableFormat.ToLog)
        {
            if (loggable == null) throw new ArgumentNullException(nameof(loggable));

            return PushLogProperties(loggable, pureLogLevel.MinimumLogLevel, loggableFormat);
        }

        public static IDisposable PushLogPropertiesParents(this IPureLoggable loggable, IPureLogLevel pureLogLevel) =>
            PushLogProperties(loggable, pureLogLevel, LoggableFormat.ToLogWithParents);


        public static IDisposable PushLogProperties(this IPureLogger logger,
            IEnumerable<IPureLogPropertyLevel> logPropertyList, IPureLogLevel pureLogLevel)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            if (logPropertyList == null) throw new ArgumentNullException(nameof(logPropertyList));
            if (pureLogLevel == null) throw new ArgumentNullException(nameof(pureLogLevel));

            return logger.PushLogProperties(logPropertyList, pureLogLevel.MinimumLogLevel);
        }

        public static IDisposable PushLogProperties(this IPureLogger logger,
            IPureLogPropertyLevelList logPropertyLevelList, LogLevel minimumLogLevel) =>
            logger.PushLogProperties(logPropertyLevelList.GetLogPropertyLevelList(minimumLogLevel));
    }
}