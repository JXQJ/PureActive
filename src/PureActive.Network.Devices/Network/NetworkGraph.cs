using System;
using PureActive.Network.Abstractions.Device;
using PureActive.Network.Abstractions.Local;
using PureActive.Network.Abstractions.Network;
using PureActive.Network.Devices.PureObjectGraph;

namespace PureActive.Network.Devices.Network
{
    public class NetworkGraph : INetworkGraph
    {

        #region Private Fields

        private PureObjectGraph<IDevice> _deviceGraph;
        private INetworkGateway _networkGateway;
        private ILocalNetworkDevice _localNetworkDevice;
        private readonly object _deviceGraphLock = new object();

        #endregion

        #region public Fields

        public INetworkGateway NetworkGateway
        {
            get => _networkGateway;
            set
            {
                if (value != _networkGateway)
                {
                    lock (_deviceGraph)
                    {
                        _networkGateway = value;
                        _deviceGraph = new PureObjectGraph<IDevice>();
                        _deviceGraph.AddVertex(_networkGateway);
                    }
                }
            }
        }

        public ILocalNetworkDevice LocalNetworkDevice
        {
            get => _localNetworkDevice;
            set
            {
                if (value != _localNetworkDevice)
                {
                    lock (_deviceGraph)
                    {
                        // Remove old local network device
                        if (_localNetworkDevice != null)
                        {
                            // Remove vertex and all edges
                            _deviceGraph.RemoveVertex(_localNetworkDevice.ObjectId);
                        }

                        _localNetworkDevice = value;
                        _deviceGraph.AddEdge(_localNetworkDevice.ObjectId, NetworkGateway.ObjectId);
                    }
                }
            }
        }

        #endregion

        public NetworkGraph(INetworkGateway networkGateway, ILocalNetworkDevice localNetworkDevice)
        {
            NetworkGateway = networkGateway ?? throw new ArgumentNullException(nameof(networkGateway));
            LocalNetworkDevice = localNetworkDevice ?? throw new ArgumentNullException(nameof(localNetworkDevice));
        }

        public bool AddDeviceToGateway(IDevice device)
        {
            lock (_deviceGraphLock)
            {
                if (device.Equals(NetworkGateway))
                {

                }
                else if (device.Equals(LocalNetworkDevice))
                {

                }
                else if (_deviceGraph.AddVertex(device))
                {
                    _deviceGraph.AddEdge(device.ObjectId, NetworkGateway.ObjectId);
                }

            }

            return false;
        }

    }
}
