using Microsoft.Extensions.Configuration;

namespace PureActive.Logger.Provider.Serilog.Interfaces
{
    public interface IPureLoggerSettings
    {
        IPureLogProviderSettings Default { get; }
        IPureLogProviderSettings File { get; }
        IPureLogProviderSettings Console { get; }
        IPureLogProviderSettings Test { get; }

        IPureLogProviderSettings AppInsights { get; }

        IConfiguration Configuration { get; }
    }
}
