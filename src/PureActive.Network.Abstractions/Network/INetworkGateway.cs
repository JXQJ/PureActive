using System.Net;
using System.Net.NetworkInformation;
using PureActive.Network.Abstractions.NetworkDevice;
using PureActive.Network.Abstractions.Types;

namespace PureActive.Network.Abstractions.Network
{
    public interface INetworkGateway : INetworkDevice
    {
        IPAddressSubnet IPAddressSubnet { get; set; }

        IPAddress IPAddress { get; }
        IPAddress SubnetMask { get; }
        PhysicalAddress PhysicalAddress { get; }
    }
}
