using System;
using PureActive.Hosting.Abstractions.System;
using PureActive.Network.Abstractions.CommonNetworkServices;
using PureActive.Network.Abstractions.DhcpService.Events;

namespace PureActive.Network.Abstractions.DhcpService.Interfaces
{
    public interface IDhcpService : IHostedServiceInternal
    {
        IDhcpSessionMgr DhcpSessionMgr { get; }

        ICommonNetworkServices CommonNetworkServices { get; }

        event EventHandler<DhcpDiscoveredDeviceEvent> OnDhcpDiscoveredDevice;
    }
}