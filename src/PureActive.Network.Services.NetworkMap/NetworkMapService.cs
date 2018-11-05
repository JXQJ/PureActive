using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using PureActive.Core.Extensions;
using PureActive.Hosting.Abstractions.Types;
using PureActive.Hosting.Hosting;
using PureActive.Network.Abstractions.CommonNetworkServices;
using PureActive.Network.Abstractions.DhcpService.Interfaces;
using PureActive.Network.Abstractions.Network;
using PureActive.Network.Abstractions.NetworkMapService;

namespace PureActive.Network.Services.NetworkMap
{
    public class NetworkMapService: HostedServiceInternal<NetworkMapService>, INetworkMapService
    {
        private static INetworkMapService _networkMapService;

        private static INetworkMapService Instance
        {
            get => _networkMapService;
            set => NetworkMapService._networkMapService = value ?? throw new ArgumentNullException(nameof(value));
        }

        // Common Services
        public INetworkMap NetworkMap { get; }

        public ICommonNetworkServices CommonNetworkServices => NetworkMap?.CommonNetworkServices;
        
        public IDhcpService DhcpService { get; }

        public NetworkMapService(INetworkMap networkMap, IDhcpService dhcpService, IApplicationLifetime applicationLifetime = null) :
            base(networkMap?.CommonServices, applicationLifetime, ServiceHost.NetworkMap)
        {
            if (_networkMapService != null)
                throw new ArgumentException("NetworkMapService Instance is not null", nameof(_networkMapService));

            NetworkMap = networkMap ?? throw new ArgumentNullException(nameof(networkMap));
            DhcpService = dhcpService ?? throw new ArgumentNullException(nameof(dhcpService));

            _networkMapService = this;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            var tasks = new List<Task>()
            {
                CommonNetworkServices.StartAsync(cancellationToken),
                DhcpService.StartAsync(cancellationToken),
                NetworkMap.StartAsync(cancellationToken),
            };

            var result = tasks.WaitForTasks(cancellationToken, Logger);

            base.StartAsync(cancellationToken);

            return result;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            var tasks = new List<Task>()
            {
                NetworkMap.StopAsync(cancellationToken),
                CommonNetworkServices.StopAsync(cancellationToken),
                DhcpService.StopAsync(cancellationToken),
            };

            var result = tasks.WaitForTasks(cancellationToken, Logger);

            base.StopAsync(cancellationToken);

            return result;
        }

        public Task DiscoverNetworkDevices()
        {


            return Task.CompletedTask;
        }
    }
}
