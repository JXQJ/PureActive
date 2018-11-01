using Microsoft.Extensions.Logging;
using PureActive.Logging.Abstractions.Types;

namespace PureActive.Logging.Abstractions.Interfaces
{
    public interface IPureLoggerSettings
    {
        LoggingOutputFlags LoggingOutputFlags { get; set; }

        IPureLogLevel RegisterLogLevel(string key, LogLevel logLevel);
        IPureLogLevel GetLogLevel(string key);

        IPureLogLevel GetOrRegisterLogLevel(string key, LogLevel logLevel);
        IPureLogLevel GetOrRegisterDefaultLogLevel(string key);

        IPureLogLevel RegisterLogLevel(LoggingOutputFlags loggingOutputFlag, LogLevel logLevel);
        IPureLogLevel GetLogLevel(LoggingOutputFlags loggingOutputFlag);

        IPureLogLevel GetOrRegisterLogLevel(LoggingOutputFlags loggingOutputFlag, LogLevel logLevel);
        IPureLogLevel GetOrRegisterDefaultLogLevel(LoggingOutputFlags loggingOutputFlag);

        string LogFolderPath { get; }

        string TestLogFolderPath { get; }

    }
}
