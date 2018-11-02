using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using PureActive.Network.Extensions.Extensions;

namespace PureActive.Network.Extensions.Types
{
    public class IPAddressSubnet : IComparable<IPAddressSubnet>, IComparable, IEquatable<IPAddressSubnet>
    {
        private IPAddress _iPAddress;
        private IPAddress _subnetMask;
        private IPAddress _networkAddress;
        private IPAddress _broadcastAddress;

        public IPAddress IPAddress => _iPAddress;

        public IPAddress SubnetMask => _subnetMask;

        public IPAddress NetworkAddress => _networkAddress;

        public IPAddress BroadcastAddress => _broadcastAddress;

        public IPAddressSubnet NetworkAddressSubnet => new IPAddressSubnet(NetworkAddress, SubnetMask);

        public static readonly IPAddressSubnet None = new IPAddressSubnet(IPAddress.None, IPAddress.None);

        public bool IsAddressOnSameSubnet(IPAddress address) => IPAddress.IsAddressOnSameSubnet(address, SubnetMask);



        public void UpdateAddress(IPAddress ipAddress, IPAddress subnetMask)
        {
            _iPAddress = ipAddress ?? throw new ArgumentNullException(nameof(ipAddress));
            _subnetMask = subnetMask ?? throw new ArgumentNullException(nameof(subnetMask));

            if (_iPAddress.AddressFamily != AddressFamily.InterNetwork)
                throw new ArgumentException("Only IPv4 addresses are supported", nameof(ipAddress));

            _networkAddress = ipAddress.GetNetworkAddress(subnetMask);
            _broadcastAddress = ipAddress.GetBroadcastAddress(subnetMask);
        }

        public IPAddressSubnet(IPAddress ipAddress, IPAddress subnetMask)
        {
            UpdateAddress(ipAddress, subnetMask);
        }

        public IPAddressSubnet(IPAddress ipAddress) : this(ipAddress, IPAddressExtensions.SubnetClassC)
        {

        }

        public int CompareTo(IPAddressSubnet other)
        {
            return other == null ? 1 : _iPAddress.CompareTo(other._iPAddress);

            // Ignore Subnet Masks, Just compare IPAddress
        }

        public int CompareTo(object obj)
        {
            if (!(obj is IPAddressSubnet))
                throw new ArgumentException("Object must be of type IPAddressSubnet.");

            return CompareTo((IPAddressSubnet) obj);
        }

        public bool Equals(IPAddressSubnet other)
        {
            return CompareTo(other) == 0;
        }

        public override bool Equals(object obj)
        {
            return CompareTo(obj) == 0;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (_iPAddress != null ? _iPAddress.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_subnetMask != null ? _subnetMask.GetHashCode() : 0);
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
