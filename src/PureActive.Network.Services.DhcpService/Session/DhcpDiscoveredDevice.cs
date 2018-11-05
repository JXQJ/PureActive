using System;
using System.Net;
using System.Net.NetworkInformation;
using PureActive.Network.Abstractions.DhcpService.Interfaces;
using PureActive.Network.Abstractions.Types;

namespace PureActive.Network.Services.DhcpService.Session
{
    public class DhcpDiscoveredDevice : IDhcpDiscoveredDevice
    {
        public DhcpDiscoveredDevice(uint dhcpSessionId, PhysicalAddress physicalAddress, IPAddress ipAddress)
        {
            IpAddress = ipAddress ?? throw new ArgumentNullException(nameof(ipAddress));
            PhysicalAddress = physicalAddress ?? throw new ArgumentNullException(nameof(physicalAddress));
            DhcpSessionId = dhcpSessionId;
        }

        public DhcpDiscoveredDevice(uint dhcpSessionId, PhysicalAddress physicalAddress)
            :this (dhcpSessionId, physicalAddress, IPAddress.None)
        {

        }
  
        // IDeviceInfo
        public string Id { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public string DeviceName { get; set; }
        public string Version { get; set; }
        public Version VersionNumber { get; set; }
        public string AppVersion { get; set; }
        public string AppBuild { get; set; }
        public DevicePlatform DevicePlatform { get; set; }
        public DeviceType DeviceType { get; set; }
        public bool IsDevice { get; set; }

        // INetworkDeviceInfo
        public IPAddress IpAddress { get; set; }
   

        // IDhcpDiscoveredDevice
        public PhysicalAddress PhysicalAddress { get; set; }
        public string HostName { get; set; }
        public string VendorClassId { get; set; }
        public uint DhcpSessionId { get; set; }
    }
}
