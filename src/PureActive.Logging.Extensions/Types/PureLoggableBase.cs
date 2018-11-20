using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions.Internal;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Logging.Abstractions.Types;

namespace PureActive.Logging.Extensions.Types
{
    public abstract class PureLoggableBase<T> : IPureLoggable
    {
        public PureLoggableBase(IPureLoggerFactory loggerFactory, IPureLogger logger = null)
        {
            LoggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            Logger = logger ?? loggerFactory.CreatePureLogger<T>();
        }

        public IPureLoggerFactory LoggerFactory { get; }
        public IPureLoggerSettings LoggerSettings => LoggerFactory.PureLoggerSettings;
        public IPureLogger Logger { get; protected set; }

        /// <summary>
        ///     Converts derived class into string object.
        /// </summary>
        public virtual string ToString(LogLevel logLevel)
        {
            return ToString(logLevel, LoggableFormat.ToString);
        }

        public virtual string ToString(LogLevel logLevel, LoggableFormat loggableFormat)
        {
            var sb = new StringBuilder();

            return FormatLogString(sb, logLevel, loggableFormat).ToString();
        }

        public virtual StringBuilder FormatLogString(StringBuilder sb, LogLevel logLevel, LoggableFormat loggableFormat)
        {
            PureLogPropertyLevel.FormatPropertyList(sb, loggableFormat,
                GetLogPropertyListLevel(logLevel, loggableFormat), logLevel);

            return sb;
        }


        public virtual IEnumerable<IPureLogPropertyLevel> GetLogPropertyListLevel(LogLevel logLevel,
            LoggableFormat loggableFormat)
        {
            var logProperties = new List<IPureLogPropertyLevel>
            {
                new PureLogPropertyLevel("ObjectType", TypeNameHelper.GetTypeDisplayName(GetType()),
                    LogLevel.Information)
            };

            return logProperties.Where(p => p.MinimumLogLevel.CompareTo(logLevel) >= 0);
        }

        /// <summary>
        ///     Converts derived class into string object.
        /// </summary>
        public override string ToString()
        {
            return ToString(LogLevel.Debug);
        }
    }
}