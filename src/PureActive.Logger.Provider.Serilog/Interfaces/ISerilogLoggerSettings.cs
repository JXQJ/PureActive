using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PureActive.Logging.Abstractions.Interfaces;
using Serilog.Events;

namespace PureActive.Logger.Provider.Serilog.Interfaces
{
    public interface ISerilogLoggerSettings : IPureLoggerSettings
    {
        LogEventLevel DefaultLogEventLevel { get; set; }

        IPureLogLevel RegisterLogLevel(string key, LogEventLevel logEventLevel);

        ISerilogLogLevel GetSerilogLogLevel(string key);

        IConfiguration Configuration { get; }

        ISerilogLogLevel RegisterSerilogLogLevel(string key, LogEventLevel logEventLevel);
        ISerilogLogLevel RegisterSerilogLogLevel(string key, LogLevel logLevel);

        ISerilogLogLevel GetOrRegisterSerilogLogLevel(string key, LogLevel logLevel);
        ISerilogLogLevel GetOrRegisterSerilogLogLevel(string key, LogEventLevel logEventLevel);

        ISerilogLogLevel GetOrRegisterSerilogLogDefaultLevel(string key);
    }
}
