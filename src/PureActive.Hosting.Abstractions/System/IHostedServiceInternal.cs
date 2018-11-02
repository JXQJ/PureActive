using Microsoft.Extensions.Hosting;
using PureActive.Hosting.Abstractions.Types;
using PureActive.Logging.Abstractions.Interfaces;

namespace PureActive.Hosting.Abstractions.System
{
    public interface IHostedServiceInternal : IHostedService, IPureLoggable
    {
        ServiceHostStatus ServiceHostStatus { get; }
    }
}
