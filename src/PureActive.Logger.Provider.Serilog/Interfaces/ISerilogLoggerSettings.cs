using Microsoft.Extensions.Configuration;

namespace PureActive.Logger.Provider.Serilog.Interfaces
{
    public interface ISerilogLoggerSettings
    {
        ISerilogLogProviderSettings Default { get; }
        ISerilogLogProviderSettings File { get; }
        ISerilogLogProviderSettings Console { get; }
        ISerilogLogProviderSettings TestConsole { get; }

        ISerilogLogProviderSettings TestCorrelator { get; }

        ISerilogLogProviderSettings AppInsights { get; }

        IConfiguration Configuration { get; }
    }
}
