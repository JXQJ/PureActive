using System.Collections;
using System.Collections.Generic;
using System.Net;
using PureActive.Network.Abstractions.Network;
using PureActive.Network.Abstractions.Types;
using PureActive.Network.Extensions.Enumeration;

namespace PureActive.Network.Extensions.Network
{
    public class NetworkEnumerator : INetworkEnumerable
    {
        public NetworkEnumerator(INetworkGateway networkGateway)
        {
            IPAddressRange =
                new IPAddressRange(new IPAddressSubnet(networkGateway.IPAddress, networkGateway.SubnetMask));
        }

        public NetworkEnumerator(IPAddressSubnet ipAddressSubnet)
        {
            IPAddressRange = new IPAddressRange(ipAddressSubnet);
        }

        public IPAddressRange IPAddressRange { get; internal set; }

        public IEnumerator<IPAddress> GetEnumerator()
        {
            return IPAddressRange.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}