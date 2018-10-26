using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Logging.Abstractions.Types;
using Serilog.Events;

namespace PureActive.Logger.Provider.Serilog.Interfaces
{
    public interface ISerilogLoggerSettings : IPureLoggerSettings
    {
        IConfiguration Configuration { get; }

        IPureLogLevel RegisterLogLevel(string key, LogEventLevel logEventLevel);

        //ISerilogLogLevel GetSerilogLogLevel(string key);

        //ISerilogLogLevel RegisterSerilogLogLevel(string key, LogEventLevel logEventLevel);
        //ISerilogLogLevel RegisterSerilogLogLevel(string key, LogLevel logLevel);

        //ISerilogLogLevel GetOrRegisterSerilogLogLevel(string key, LogLevel logLevel);
        //ISerilogLogLevel GetOrRegisterSerilogLogLevel(string key, LogEventLevel logEventLevel);

        //ISerilogLogLevel GetOrRegisterSerilogLogDefaultLevel(string key);

        IPureLogLevel RegisterLogLevel(LoggingOutputFlags loggingOutputFlag, LogEventLevel logEventLevel);

        ISerilogLogLevel GetSerilogLogLevel(LoggingOutputFlags loggingOutputFlag);

        ISerilogLogLevel RegisterSerilogLogLevel(LoggingOutputFlags loggingOutputFlag, LogEventLevel logEventLevel);
        ISerilogLogLevel RegisterSerilogLogLevel(LoggingOutputFlags loggingOutputFlag, LogLevel logLevel);

        ISerilogLogLevel GetOrRegisterSerilogLogLevel(LoggingOutputFlags loggingOutputFlag, LogLevel logLevel);
        ISerilogLogLevel GetOrRegisterSerilogLogLevel(LoggingOutputFlags loggingOutputFlag, LogEventLevel logEventLevel);

        ISerilogLogLevel GetOrRegisterSerilogLogDefaultLevel(LoggingOutputFlags loggingOutputFlag);
    }
}
