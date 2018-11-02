using PureActive.Network.Abstractions.Network;
using PureActive.Network.Abstractions.NetworkDevice;

namespace PureActive.Network.Abstractions.Local
{
    public interface ILocalNetworkDevice : INetworkDevice
    {
        INetworkAdapterCollection NetworkAdapters { get; }
        ILocalNetworkCollection LocalNetworks { get; }
    }
}
