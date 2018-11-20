using PureActive.Network.Abstractions.Types;

namespace PureActive.Network.Abstractions.Network
{
    public interface INetwork
    {
        IPAddressSubnet NetworkIPAddressSubnet { get; }
        INetworkAdapterCollection NetworkAdapterCollection { get; }
        INetworkGateway NetworkGateway { get; }

        int AdapterCount { get; }

        bool AddAdapterToNetwork(INetworkAdapter networkAdapter);
        bool RemoveAdapterFromNetwork(INetworkAdapter networkAdapter);
        bool AdapterConnectedToNetwork(INetworkAdapter networkAdapter);
    }
}