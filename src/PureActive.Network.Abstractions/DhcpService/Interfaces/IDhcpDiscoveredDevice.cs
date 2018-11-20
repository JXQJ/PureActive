using System.Net.NetworkInformation;
using PureActive.Network.Abstractions.DeviceInfo;

namespace PureActive.Network.Abstractions.DhcpService.Interfaces
{
    public interface IDhcpDiscoveredDevice : INetworkDeviceInfo
    {
        PhysicalAddress PhysicalAddress { get; }

        string HostName { get; }
        string VendorClassId { get; }
        uint DhcpSessionId { get; set; }
    }
}