using System;
using System.Net.NetworkInformation;
using PureActive.Network.Abstractions.CommonNetworkServices;
using PureActive.Network.Abstractions.Local;
using PureActive.Network.Abstractions.Network;
using PureActive.Network.Abstractions.Types;
using PureActive.Network.Devices.Network;

namespace PureActive.Network.Devices.Computer
{
    public class LocalComputer : ComputerBase, ILocalComputer
    {
        private bool _isInitialized;
        public bool IsMonitoring { get; internal set; }

        private readonly NetworkAdapterCollection _networkAdapterCollection;
        private readonly LocalNetworkCollection _localNetworkCollection;

        public ILocalNetworkCollection LocalNetworks => _localNetworkCollection;


        public LocalComputer(ICommonNetworkServices commonNetworkServices, DeviceType deviceType)
            :base(commonNetworkServices, deviceType)
        {
            _networkAdapterCollection = new NetworkAdapterCollection(commonNetworkServices);
            _localNetworkCollection = new LocalNetworkCollection(commonNetworkServices);
        }
     
        public bool StartNetworkEventMonitor()
        {
            if (!IsMonitoring)
            {
                IsMonitoring = true;
                NetworkChange.NetworkAvailabilityChanged += NetworkAvailabilityChanged;
                NetworkChange.NetworkAddressChanged += NetworkAddressChanged;
            }

            return IsMonitoring;
        }

        public bool StopNetworkEventMonitor()
        {
            if (IsMonitoring)
            {
                IsMonitoring = false;
                NetworkChange.NetworkAvailabilityChanged -= NetworkAvailabilityChanged;
                NetworkChange.NetworkAddressChanged -= NetworkAddressChanged;
            }

            return IsMonitoring;
        }


        public static void NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {

        }

        public static void NetworkAddressChanged(object sender, EventArgs e)
        {

        }

        public INetworkAdapterCollection NetworkAdapters
        {
            get
            {
                if (_isInitialized) return _networkAdapterCollection;

                foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces())
                {
                    var networkAdapter = new NetworkAdapter(CommonNetworkServices, networkInterface);

                    _localNetworkCollection.AddAdapterToNetwork(networkAdapter);
                    _networkAdapterCollection.Add(networkAdapter);
                }

                _isInitialized = true;

                return _networkAdapterCollection;
            }
        }

        public INetwork PrimaryNetwork
        {
            get
            {
                if (!_isInitialized)
                {
                    // Initialize NetworkAdapters which will populate Local Networks
                    var networkAdapter = NetworkAdapters;
                }

                return _localNetworkCollection.PrimaryNetwork;
            }
        }
    }
}
