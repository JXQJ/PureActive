using System.Net.NetworkInformation;
using PureActive.Network.Abstractions.DeviceInfo;
using PureActive.Network.Abstractions.DhcpService.Types;

namespace PureActive.Network.Abstractions.DhcpService.Interfaces
{
    public interface IDhcpSessionResult
    {
        uint SessionId { get; set; }

        DhcpSessionState DhcpSessionStateCurrent { get; set; }
        DhcpSessionState DhcpSessionStateStart { get; set; }

        IDhcpDiscoveredDevice DhcpDiscoveredDevice { get; }

        IDeviceInfo DeviceInfo { get; }

        INetworkDeviceInfo NetworkDeviceInfo { get; }


        void UpdateSessionState(uint sessionId, DhcpSessionState dhcpSessionState, PhysicalAddress physicalAddress);

        bool IsFullSession();
        bool IsCurrentSession(uint sessionId);

        bool IsDuplicateRequest(IDhcpMessage dhcpMessage);
    }
}