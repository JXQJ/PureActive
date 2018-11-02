using PureActive.Network.Abstractions.CommonNetworkServices;
using PureActive.Network.Abstractions.Device;

namespace PureActive.Network.Abstractions.NetworkDevice
{
    public interface INetworkDevice : IDevice
    {
        ICommonNetworkServices CommonNetworkServices { get; }
    }
}
