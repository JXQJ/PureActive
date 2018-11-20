using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using PureActive.Logging.Abstractions.Types;

namespace PureActive.Logging.Abstractions.Interfaces
{
    public interface IPureLoggable
    {
        IPureLoggerFactory LoggerFactory { get; }

        IPureLogger Logger { get; }

        IPureLoggerSettings LoggerSettings { get; }
        string ToString(LogLevel logLevel);

        string ToString(LogLevel logLevel, LoggableFormat loggableFormat);

        StringBuilder FormatLogString(StringBuilder sb, LogLevel logLevel, LoggableFormat loggableFormat);

        IEnumerable<IPureLogPropertyLevel> GetLogPropertyListLevel(LogLevel logLevel, LoggableFormat loggableFormat);
    }
}