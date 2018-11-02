using PureActive.Hosting.Abstractions.System;
using PureActive.Network.Abstractions.ArpService;
using PureActive.Network.Abstractions.PingService;

namespace PureActive.Network.Abstractions.CommonNetworkServices
{
    public interface ICommonNetworkServices : ICommonServices
    {
        ICommonServices CommonServices { get; }

        IPingService PingService { get; }
        IArpService ArpService { get; }
    }
}
