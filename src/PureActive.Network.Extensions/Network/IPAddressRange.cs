using System.Collections;
using System.Collections.Generic;
using System.Net;
using PureActive.Network.Abstractions.Extensions;
using PureActive.Network.Abstractions.Types;

namespace PureActive.Network.Extensions.Network
{
    public class IPAddressRange : IEnumerable<IPAddress>
    {
        public IPAddress IpAddressLowerInclusive { get; set; }
        public IPAddress IpAddressUpperInclusive { get; set; }

        public IPAddressRange(IPAddress ipAddressLowerInclusive, IPAddress ipAddressUpperInclusive)
        {
            IpAddressLowerInclusive = ipAddressLowerInclusive;
            IpAddressUpperInclusive = ipAddressUpperInclusive;
        }

        public IPAddressRange(IPAddressSubnet ipAddressSubnet)
        {
            IpAddressLowerInclusive = ipAddressSubnet.NetworkAddress;
            IpAddressUpperInclusive = ipAddressSubnet.BroadcastAddress;
        }

        public bool IsInRange(IPAddress address)
        {
            if (address.AddressFamily != IpAddressLowerInclusive.AddressFamily)
            {
                return false;
            }

            byte[] lowerBytes = IpAddressLowerInclusive.GetAddressBytes();
            byte[] upperBytes = IpAddressUpperInclusive.GetAddressBytes();
            byte[] addressBytes = address.GetAddressBytes();
            bool lowerBoundary = true, upperBoundary = true;

            for (var i = 0; i < lowerBytes.Length && (lowerBoundary || upperBoundary); i++)
            {
                if (lowerBoundary && addressBytes[i] < lowerBytes[i] ||
                    upperBoundary && addressBytes[i] > upperBytes[i])
                {
                    return false;
                }

                lowerBoundary &= addressBytes[i] == lowerBytes[i];
                upperBoundary &= addressBytes[i] == upperBytes[i];
            }

            return true;
        }

        public IEnumerator<IPAddress> GetEnumerator()
        {
            var ipAddress = IpAddressLowerInclusive;
            var ipAddressStop = IpAddressUpperInclusive.Increment();

            while (!ipAddress.Equals(ipAddressStop))
            {
                yield return ipAddress;
                ipAddress = ipAddress.Increment();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
