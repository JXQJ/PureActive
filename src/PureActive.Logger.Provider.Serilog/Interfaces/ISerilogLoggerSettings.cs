using Microsoft.Extensions.Configuration;

namespace PureActive.Logger.Provider.Serilog.Interfaces
{
    public interface ISerilogLoggerSettings
    {
        ISerilogLogLevel Default { get; }
        ISerilogLogLevel File { get; }
        ISerilogLogLevel Console { get; }
        ISerilogLogLevel TestConsole { get; }

        ISerilogLogLevel TestCorrelator { get; }

        ISerilogLogLevel AppInsights { get; }

        IConfiguration Configuration { get; }
    }
}
