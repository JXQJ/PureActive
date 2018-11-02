using System;
using System.Text;
using Microsoft.Extensions.Logging;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Logging.Abstractions.Types;

namespace PureActive.Logging.Extensions.Types
{
    public abstract class LoggableBase<T>: IPureLoggable
    {
        public IPureLoggerFactory LoggerFactory { get; }
        public IPureLoggerSettings LoggerSettings => LoggerFactory.PureLoggerSettings;
        public IPureLogger Logger { get; protected set; }

        public LoggableBase(IPureLoggerFactory loggerFactory, IPureLogger logger = null)
        {
            LoggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            Logger = logger ?? loggerFactory.CreatePureLogger<T>();
        }

        /// <summary>
        /// Converts derived class into string object.
        /// </summary>
        public override string ToString()
        {
            return ToString(LogLevel.Debug);
        }

        /// <summary>
        /// Converts derived class into string object.
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
            // TODO: Fix LogPropertyLevel
            //LogPropertyLevel.FormatPropertyList(sb, loggableFormat, GetLogPropertyListLevel(logLevel, loggableFormat), logLevel);

            return sb;
        }

        // TODO: Fix LogPropertyLevel
        //public virtual IEnumerable<ILogPropertyLevel> GetLogPropertyListLevel(LogLevel logLevel, LoggableFormat loggableFormat)
        //{
        //    var logProperties = new List<ILogPropertyLevel>
        //    {
        //        {new LogPropertyLevel("ObjectType", TypeNameHelper.GetTypeDisplayName(this.GetType()), LogLevel.Information)}
        //    };

        //    return logProperties.Where(p => p.MinimumLogLevel.CompareTo(logLevel) >= 0);
        //}
    }
}
