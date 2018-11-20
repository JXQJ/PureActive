using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using PureActive.Network.Abstractions.Extensions;

namespace PureActive.Network.Abstractions.Types
{
    public class IPAddressSubnet : IComparable<IPAddressSubnet>, IComparable, IEquatable<IPAddressSubnet>
    {
        public static readonly IPAddressSubnet None = new IPAddressSubnet(IPAddress.None, IPAddress.None);

        public IPAddressSubnet(IPAddress ipAddress, IPAddress subnetMask)
        {
            IPAddress = ipAddress ?? throw new ArgumentNullException(nameof(ipAddress));
            SubnetMask = subnetMask ?? throw new ArgumentNullException(nameof(subnetMask));

            if (IPAddress.AddressFamily != AddressFamily.InterNetwork)
                throw new ArgumentException("Only IPv4 addresses are supported", nameof(ipAddress));

            NetworkAddress = ipAddress.GetNetworkAddress(subnetMask);
            BroadcastAddress = ipAddress.GetBroadcastAddress(subnetMask);
        }

        public IPAddressSubnet(IPAddress ipAddress) : this(ipAddress, IPAddressExtensions.SubnetClassC)
        {
        }

        public IPAddress IPAddress { get; }
        public IPAddress SubnetMask { get; }
        public IPAddress NetworkAddress { get; }
        public IPAddress BroadcastAddress { get; }
        public IPAddressSubnet NetworkAddressSubnet => new IPAddressSubnet(NetworkAddress, SubnetMask);

        public int CompareTo(object obj)
        {
            if (!(obj is IPAddressSubnet))
                throw new ArgumentException("Object must be of type IPAddressSubnet.");

            return CompareTo((IPAddressSubnet) obj);
        }

        public int CompareTo(IPAddressSubnet other)
        {
            return other == null ? 1 : IPAddress.CompareTo(other.IPAddress);

            // Ignore Subnet Masks, Just compare IPAddress
        }

        public bool Equals(IPAddressSubnet other)
        {
            return CompareTo(other) == 0;
        }

        public bool IsAddressOnSameSubnet(IPAddress address) => IPAddress.IsAddressOnSameSubnet(address, SubnetMask);

        public override bool Equals(object obj)
        {
            return CompareTo(obj) == 0;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = IPAddress != null ? IPAddress.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (SubnetMask != null ? SubnetMask.GetHashCode() : 0);
                return hashCode;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder().Append(IPAddress).Append(" (").Append(SubnetMask).Append(")");

            return sb.ToString();
        }
    }
}