using System;
using System.Net;
using System.Net.NetworkInformation;

namespace PureActive.Network.Abstractions.ArpService
{
    public class ArpItem
    {
        public ArpItem(PhysicalAddress physicalAddress, IPAddress ipAddress, DateTimeOffset createdTimestamp)
        {
            PhysicalAddress = physicalAddress;
            IPAddress = ipAddress;
            CreatedTimestamp = createdTimestamp;
        }

        public ArpItem(PhysicalAddress physicalAddress, IPAddress ipAddress) : this(physicalAddress, ipAddress,
            DateTimeOffset.Now)
        {
        }

        public PhysicalAddress PhysicalAddress { get; set; }
        public IPAddress IPAddress { get; set; }
        public DateTimeOffset CreatedTimestamp { get; set; }
    }
}