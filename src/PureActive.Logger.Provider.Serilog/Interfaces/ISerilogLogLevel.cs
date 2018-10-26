using PureActive.Logging.Abstractions.Interfaces;
using Serilog.Core;
using Serilog.Events;

namespace PureActive.Logger.Provider.Serilog.Interfaces
{
    public interface ISerilogLogLevel : IPureLogLevel
    {
        LoggingLevelSwitch LoggingLevelSwitch { get; }

        LogEventLevel MinimumLevel { get; set; }

        LogEventLevel InitialLevel { get; set; }
    }
}
