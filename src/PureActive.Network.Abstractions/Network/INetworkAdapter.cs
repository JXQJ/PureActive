using System.Net.NetworkInformation;
using PureActive.Network.Abstractions.Types;

namespace PureActive.Network.Abstractions.Network
{
    public interface INetworkAdapter
    {
        NetworkInterface NetworkInterface { get; }

        OperationalStatus OperationalStatus { get; }
        NetworkInterfaceType NetworkInterfaceType { get; }

        IPInterfaceProperties IPProperties { get; }

        IPAddressSubnet PrimaryAddressSubnet { get; }

        IPAddressSubnet NetworkAddressSubnet { get; }
    }
}