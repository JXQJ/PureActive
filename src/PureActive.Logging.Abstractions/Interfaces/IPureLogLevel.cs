using Microsoft.Extensions.Logging;

namespace PureActive.Logging.Abstractions.Interfaces
{
    public interface IPureLogLevel
    {
        LogLevel MinimumLogLevel { get; set; }

        LogLevel InitialLogLevel { get; set; }
    }
}