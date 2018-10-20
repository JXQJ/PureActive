using Microsoft.Extensions.Logging;

namespace PureActive.Logging.Abstractions.Interfaces
{
    public interface ILoggable
    {

        string ToString(LogLevel logLevel);

        string ToString(LogLevel logLevel, LoggableFormat loggableFormat);
    }
}
