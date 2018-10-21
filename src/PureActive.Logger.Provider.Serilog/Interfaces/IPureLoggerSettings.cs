using Microsoft.Extensions.Configuration;
using PureActive.Logging.Abstractions.Interfaces;

namespace PureActive.Logger.Provider.Serilog.Interfaces
{
    public interface IPureLoggerSettings
    {
        ILogProviderSettings Default { get; }
        ILogProviderSettings File { get; }
        ILogProviderSettings Console { get; }
        ILogProviderSettings Test { get; }

        ILogProviderSettings AppInsights { get; }

        IConfiguration Configuration { get; }
    }
}
