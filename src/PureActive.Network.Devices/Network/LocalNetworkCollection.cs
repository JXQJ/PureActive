using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Microsoft.Extensions.Logging;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Network.Abstractions.CommonNetworkServices;
using PureActive.Network.Abstractions.Local;
using PureActive.Network.Abstractions.Network;
using PureActive.Network.Abstractions.Types;

namespace PureActive.Network.Devices.Network
{
    public class LocalNetworkCollection : ILocalNetworkCollection
    {
        private readonly Dictionary<IPAddressSubnet, INetwork> _networks = new Dictionary<IPAddressSubnet, INetwork>();
        private INetwork _primaryNetwork;
        
        public INetwork PrimaryNetwork
        {
            get => _primaryNetwork ?? (_primaryNetwork = GetPrimaryNetworkInternal());
            internal set => _primaryNetwork = value;
        }

        public int Count => _networks.Count;

        private readonly ICommonNetworkServices _commonNetworkServices;
        private readonly IPureLogger _logger;
        
        public LocalNetworkCollection(ICommonNetworkServices commonNetworkServices)
        {
            _commonNetworkServices = commonNetworkServices ?? throw new ArgumentNullException(nameof(commonNetworkServices));

            _logger = commonNetworkServices.LoggerFactory?.CreatePureLogger<LocalNetworkCollection>();
        }

        private INetwork GetPrimaryNetworkInternal()
        {
            return _networks.FirstOrDefault().Value;
        }

        public INetwork AddAdapterToNetwork(INetworkAdapter networkAdapter)
        {
            // Is the network up and have a Network Gateway
            if (networkAdapter.NetworkInterface.OperationalStatus == OperationalStatus.Up &&
                networkAdapter.NetworkInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                networkAdapter.PrimaryAddressSubnet != null)
            {
                var networkAddressSubnet = networkAdapter.NetworkAddressSubnet;

                if (!_networks.TryGetValue(networkAddressSubnet, out var localNetwork))
                {
                    localNetwork = new LocalNetwork(_commonNetworkServices, networkAdapter);
                    _networks.Add(networkAddressSubnet, localNetwork);
                }

                localNetwork.AddAdapterToNetwork(networkAdapter);

                return localNetwork;
            }

            _logger?.LogTrace("AddAdapterToNetwork: NetworkAdapter ignored:\n{NetworkAdapter}", networkAdapter);

            return null;
        }

        public bool RemoveNetwork(INetwork network)
        {
            return _networks.Remove(network.NetworkIPAddressSubnet);
        }

        public bool RemoveAdapterFromNetwork(INetworkAdapter networkAdapter)
        {
            var networkAddressSubnet = networkAdapter.NetworkAddressSubnet;

            if (_networks.TryGetValue(networkAddressSubnet, out var localNetwork))
            {
                if (localNetwork.RemoveAdapterFromNetwork(networkAdapter))
                {
                    return localNetwork.AdapterCount != 0 || RemoveNetwork(localNetwork);
                }
            }
            
            _logger?.LogDebug("RemoveAdapterFromNetwork: networkAdapter not found");

            return false;
        }

        public IEnumerator<INetwork> GetEnumerator()
        {
            return _networks.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}