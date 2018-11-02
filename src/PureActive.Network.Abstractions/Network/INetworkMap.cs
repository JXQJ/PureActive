using System;
using PureActive.Hosting.Abstractions.System;
using PureActive.Network.Abstractions.Local;
using PureActive.Network.Abstractions.NetworkDevice;

namespace PureActive.Network.Abstractions.Network
{
    public interface INetworkMap : INetworkDevice, IHostedServiceInternal
    {
        ILocalNetworkCollection LocalNetworks { get;}
        INetwork PrimaryNetwork { get; }

        ILocalNetworkDevice LocalNetworkDevice { get; }
        DateTimeOffset UpdatedTimestamp { get; }

        DateTimeOffset UpdateTimestamp();
    }
}
