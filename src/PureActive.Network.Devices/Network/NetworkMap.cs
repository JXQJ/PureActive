using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PureActive.Hosting.Abstractions.Types;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Network.Abstractions.CommonNetworkServices;
using PureActive.Network.Abstractions.Local;
using PureActive.Network.Abstractions.Network;
using PureActive.Network.Abstractions.Types;
using PureActive.Network.Devices.Computer;

namespace PureActive.Network.Devices.Network
{
    public class NetworkMap : NetworkDeviceBase, INetworkMap
    {
        private readonly IPureLogger _logger;

        public NetworkMap(ICommonNetworkServices commonNetworkServices) : base(commonNetworkServices)
        {
            if (commonNetworkServices == null) throw new ArgumentNullException(nameof(commonNetworkServices));

            _logger = commonNetworkServices.LoggerFactory?.CreatePureLogger<NetworkMap>();

            UpdateTimestamp();
        }

        public ILocalNetworkDevice LocalNetworkDevice { get; private set; }
        public ILocalNetworkCollection LocalNetworks => LocalNetworkDevice?.LocalNetworks;
        public DateTimeOffset UpdatedTimestamp { get; private set; }
        public ServiceHostStatus ServiceHostStatus { get; internal set; } = ServiceHostStatus.Stopped;

        public INetwork PrimaryNetwork => LocalNetworks?.PrimaryNetwork;

        public DateTimeOffset UpdateTimestamp()
        {
            return UpdatedTimestamp = DateTimeOffset.Now;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            ServiceHostStatus = ServiceHostStatus.StartPending;
            LocalNetworkDevice = DiscoverLocalNetworkDevice();

            _logger?.LogInformation("NetworkMap Service Started on Primary Network: {PrimaryIPAddressSubnet}",
                LocalNetworkDevice?.LocalNetworks?.PrimaryNetwork?.NetworkIPAddressSubnet);

            ServiceHostStatus = ServiceHostStatus.Running;
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            ServiceHostStatus = ServiceHostStatus.Stopped;
            return Task.CompletedTask;
        }

        private ILocalNetworkDevice DiscoverLocalNetworkDevice()
        {
            var localComputer = new LocalComputer(CommonNetworkServices, DeviceType.LocalComputer);

            var networkAdapters = localComputer.NetworkAdapters;

            if (networkAdapters.Count == 0)
                _logger.LogInformation("LocalComputer: No Network Adapters Found");

            return localComputer;
        }
    }
}