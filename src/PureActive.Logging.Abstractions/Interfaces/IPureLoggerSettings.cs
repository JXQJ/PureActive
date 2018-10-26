using Microsoft.Extensions.Logging;

namespace PureActive.Logging.Abstractions.Interfaces
{
    public interface IPureLoggerSettings
    {
        LogLevel DefaultLogLevel { get; set; }
        IPureLogLevel RegisterLogLevel(string key, LogLevel logLevel);
        IPureLogLevel GetLogLevel(string key);

        IPureLogLevel GetOrRegisterLogLevel(string key, LogLevel logLevel);
        IPureLogLevel GetOrRegisterDefaultLogLevel(string key);
    }
}
