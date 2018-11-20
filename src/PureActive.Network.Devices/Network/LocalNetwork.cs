using System.Linq;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Network.Abstractions.CommonNetworkServices;
using PureActive.Network.Abstractions.Extensions;
using PureActive.Network.Abstractions.Local;
using PureActive.Network.Abstractions.Network;
using PureActive.Network.Abstractions.Types;

namespace PureActive.Network.Devices.Network
{
    public class LocalNetwork : NetworkDeviceBase, ILocalNetwork
    {
        public LocalNetwork(ICommonNetworkServices commonNetworkServices, INetworkAdapter networkAdapter,
            IPureLogger logger = null) :
            base(commonNetworkServices, DeviceType.LocalNetwork, logger)
        {
            NetworkAdapterCollection = new NetworkAdapterCollection();
            NetworkGateway = _DiscoverNetworkGateway();

            NetworkIPAddressSubnet = networkAdapter.NetworkAddressSubnet;

            AddAdapterToNetwork(networkAdapter);
        }

        public IPAddressSubnet NetworkIPAddressSubnet { get; }
        public INetworkAdapterCollection NetworkAdapterCollection { get; }
        public INetworkGateway NetworkGateway { get; }

        public int AdapterCount => NetworkAdapterCollection?.Count ?? 0;

        public bool AddAdapterToNetwork(INetworkAdapter networkAdapter)
        {
            return NetworkAdapterCollection.Add(networkAdapter);
        }

        public bool RemoveAdapterFromNetwork(INetworkAdapter networkAdapter)
        {
            return NetworkAdapterCollection.Remove(networkAdapter);
        }

        public bool AdapterConnectedToNetwork(INetworkAdapter networkAdapter)
        {
            return NetworkAdapterCollection.Contains(networkAdapter);
        }

        private INetworkGateway _DiscoverNetworkGateway()
        {
            var ipAddressGatewaySubnet = IPAddressExtensions.GetDefaultGatewayAddressSubnet();

            return new NetworkGateway(CommonNetworkServices, ipAddressGatewaySubnet);
        }
    }
}