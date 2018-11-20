using System;
using PureActive.Network.Abstractions.DhcpService.Interfaces;

namespace PureActive.Network.Abstractions.DhcpService.Events
{
    public class DhcpDiscoveredDeviceEvent
    {
        public DhcpDiscoveredDeviceEvent(IDhcpService dhcpService, IDhcpDiscoveredDevice dhcpDiscoveredDevice)
        {
            DhcpService = dhcpService ?? throw new ArgumentNullException(nameof(dhcpService));
            DhcpDiscoveredDevice =
                dhcpDiscoveredDevice ?? throw new ArgumentNullException(nameof(dhcpDiscoveredDevice));
        }

        public IDhcpService DhcpService { get; }
        public IDhcpDiscoveredDevice DhcpDiscoveredDevice { get; }
    }
}