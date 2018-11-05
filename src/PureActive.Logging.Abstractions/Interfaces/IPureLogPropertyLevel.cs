using Microsoft.Extensions.Logging;

namespace PureActive.Logging.Abstractions.Interfaces
{
    public interface IPureLogPropertyLevel : IPureLogProperty
    {
        LogLevel MinimumLogLevel { get; }
    }
}
