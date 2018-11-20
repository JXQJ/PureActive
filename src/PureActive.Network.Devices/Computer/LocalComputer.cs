using System;
using System.Net.NetworkInformation;
using Microsoft.Extensions.Logging;
using PureActive.Network.Abstractions.CommonNetworkServices;
using PureActive.Network.Abstractions.Local;
using PureActive.Network.Abstractions.Network;
using PureActive.Network.Abstractions.Types;
using PureActive.Network.Devices.Network;

namespace PureActive.Network.Devices.Computer
{
    public class LocalComputer : ComputerBase, ILocalComputer
    {
        private readonly LocalNetworkCollection _localNetworkCollection;

        private readonly NetworkAdapterCollection _networkAdapterCollection;
        private bool _isInitialized;


        public LocalComputer(ICommonNetworkServices commonNetworkServices, DeviceType deviceType)
            : base(commonNetworkServices, deviceType)
        {
            _networkAdapterCollection = new NetworkAdapterCollection();
            _localNetworkCollection = new LocalNetworkCollection(commonNetworkServices);
        }

        public bool IsMonitoring { get; internal set; }

        public INetwork PrimaryNetwork
        {
            get
            {
                if (!_isInitialized)
                {
                    // Initialize NetworkAdapters which will populate Local Networks
                    var networkAdapter = NetworkAdapters;

                    if (networkAdapter != null)
                    {
                        Logger.LogInformation("NetworkAdapters Initialized");
                    }
                }

                return _localNetworkCollection.PrimaryNetwork;
            }
        }

        public ILocalNetworkCollection LocalNetworks => _localNetworkCollection;

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
    }
}