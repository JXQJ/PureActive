using System.Net;
using System.Net.NetworkInformation;
using PureActive.Network.Abstractions.DeviceInfo;
using PureActive.Network.Abstractions.DhcpService.Interfaces;
using PureActive.Network.Abstractions.DhcpService.Types;

namespace PureActive.Network.Services.DhcpService.Session
{
    public class DhcpSessionResult : IDhcpSessionResult
    {
        private DhcpDiscoveredDevice _dhcpDiscoveredDevice = null;

        public IDhcpDiscoveredDevice DhcpDiscoveredDevice
        {
            get => _dhcpDiscoveredDevice ?? (_dhcpDiscoveredDevice = new DhcpDiscoveredDevice(SessionId, PhysicalAddress.None));
        }

        public uint SessionId { get; set; }

        public DhcpSessionState DhcpSessionStateCurrent { get; set; }
        public DhcpSessionState DhcpSessionStateStart { get; set; }

        public PhysicalAddress PhysicalAddress
        {
            get => DhcpDiscoveredDevice.PhysicalAddress;
            set
            {
                if (_dhcpDiscoveredDevice != null) _dhcpDiscoveredDevice.PhysicalAddress = value;
            }
        }

        public string HostName
        {
            get => DhcpDiscoveredDevice.HostName;
            set
            {
                if (_dhcpDiscoveredDevice != null) _dhcpDiscoveredDevice.HostName = value;
            }
        }

        public string VendorClassId
        {
            get => DhcpDiscoveredDevice.VendorClassId;
            set
            {
                if (_dhcpDiscoveredDevice != null) _dhcpDiscoveredDevice.VendorClassId = value;
            }
        }

        public IPAddress IpAddress
        {
            get => DhcpDiscoveredDevice.IpAddress;
            set
            {
                if (_dhcpDiscoveredDevice != null) _dhcpDiscoveredDevice.IpAddress = value;
            }
        }

        public IDeviceInfo DeviceInfo => DhcpDiscoveredDevice;

        public INetworkDeviceInfo NetworkDeviceInfo => DhcpDiscoveredDevice;

        public DhcpSessionResult(PhysicalAddress physicalAddress, uint sessionId = 0)
        {
            Init(sessionId, DhcpSessionState.Init, physicalAddress);
        }

        public void UpdateSessionState(uint sessionId, DhcpSessionState dhcpSessionState, PhysicalAddress physicalAddress)
        {
            // Are we in a new Session?
            if (sessionId != SessionId)
            {
                Init(sessionId, dhcpSessionState, physicalAddress);
            }
            else
            {
                // Update Current Session State
                DhcpSessionStateCurrent = dhcpSessionState;

                // Handle Updating Discovered Device Physical Address
                if (!IsSamePhysicalAddress(physicalAddress))
                {

                    _dhcpDiscoveredDevice = new DhcpDiscoveredDevice(sessionId, physicalAddress);
                }
            }
        }

        private bool IsSamePhysicalAddress(PhysicalAddress physicalAddress)
        {
            return (DhcpDiscoveredDevice?.PhysicalAddress != null &&
                    DhcpDiscoveredDevice.PhysicalAddress.Equals(physicalAddress));
        }

        private void Init(uint sessionId, DhcpSessionState dhcpSessionState, PhysicalAddress physicalAddress)
        {
            SessionId = sessionId;
            DhcpSessionStateStart = dhcpSessionState;

            DhcpSessionStateCurrent = dhcpSessionState;

            // Reset discovered Device
            _dhcpDiscoveredDevice = new DhcpDiscoveredDevice(sessionId, physicalAddress);
        }


        public bool IsDuplicateRequest(IDhcpMessage dhcpMessage)
        {
            if (SessionId == dhcpMessage.SessionId &&
                IsSamePhysicalAddress(dhcpMessage.ClientHardwareAddress) &&
                DhcpSessionStateCurrent >= DhcpSessionState.Request)
            {
                return true;
            }

            return false;
        }

        public bool IsFullSession() => DhcpSessionStateStart == DhcpSessionState.Discover;

        public bool IsCurrentSession(uint sessionId) => sessionId == SessionId;
    }
}
