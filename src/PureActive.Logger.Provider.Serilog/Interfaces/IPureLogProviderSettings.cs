using Microsoft.Extensions.Logging;
using Serilog.Core;
using Serilog.Events;

namespace PureActive.Logger.Provider.Serilog.Interfaces
{
    public interface ILogProviderSettings
    {
        LoggingLevelSwitch LoggingLevelSwitch { get; }

        LogLevel MinimumLogLevel { get; set; }

        LogEventLevel MinimumLevel { get; set; }
    }
}
