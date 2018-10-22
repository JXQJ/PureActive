using Microsoft.Extensions.Configuration;

namespace PureActive.Logger.Provider.Serilog.Interfaces
{
    public interface ISerilogLoggerSettings
    {
        ISerilogLogProviderSettings Default { get; }
        ISerilogLogProviderSettings File { get; }
        ISerilogLogProviderSettings Console { get; }
        ISerilogLogProviderSettings Test { get; }

        ISerilogLogProviderSettings AppInsights { get; }

        IConfiguration Configuration { get; }
    }
}
