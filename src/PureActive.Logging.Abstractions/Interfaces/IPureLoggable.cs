using System.Text;
using Microsoft.Extensions.Logging;
using PureActive.Logging.Abstractions.Types;

namespace PureActive.Logging.Abstractions.Interfaces
{
    public interface IPureLoggable
    {
        string ToString(LogLevel logLevel);

        string ToString(LogLevel logLevel, LoggableFormat loggableFormat);

        StringBuilder FormatLogString(StringBuilder sb, LogLevel logLevel, LoggableFormat loggableFormat);

        // TODO: Fix ILogPropertyLevel
        // IEnumerable<ILogPropertyLevel> GetLogPropertyListLevel(LogLevel logLevel, LoggableFormat loggableFormat);

        IPureLoggerFactory LoggerFactory { get; }

        IPureLogger Logger { get; }

        IPureLoggerSettings LoggerSettings { get; }
    }
}
