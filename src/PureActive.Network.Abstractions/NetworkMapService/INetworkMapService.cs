using PureActive.Hosting.Abstractions.System;
using PureActive.Network.Abstractions.CommonNetworkServices;
using PureActive.Network.Abstractions.Network;

namespace PureActive.Network.Abstractions.NetworkMapService
{
    public interface INetworkMapService : IHostedServiceInternal
    {
        ICommonServices CommonServices { get; }

        ICommonNetworkServices CommonNetworkServices { get; }

        INetworkMap NetworkMap { get; }
    }
}