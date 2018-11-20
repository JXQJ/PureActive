using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Network.Abstractions.CommonNetworkServices;
using PureActive.Network.Abstractions.Extensions;
using PureActive.Network.Abstractions.Network;
using PureActive.Network.Abstractions.Types;

namespace PureActive.Network.Devices.Network
{
    public class NetworkAdapter : NetworkDeviceBase, INetworkAdapter
    {
        public NetworkAdapter(ICommonNetworkServices commonNetworkServices, NetworkInterface networkInterface,
            IPureLogger logger = null) :
            base(commonNetworkServices, DeviceType.NetworkAdapter, logger)
        {
            NetworkInterface = networkInterface;
        }

        public NetworkInterface NetworkInterface { get; }

        public OperationalStatus OperationalStatus => NetworkInterface.OperationalStatus;
        public NetworkInterfaceType NetworkInterfaceType => NetworkInterface.NetworkInterfaceType;
        public IPInterfaceProperties IPProperties => NetworkInterface.GetIPProperties();

        public IPAddressSubnet NetworkAddressSubnet => PrimaryAddressSubnet?.NetworkAddressSubnet;

        public IPAddressSubnet PrimaryAddressSubnet
        {
            get
            {
                if (IPProperties.GatewayAddresses.IPv4OrDefault() == null) return null;

                foreach (var ipAddress in IPProperties.UnicastAddresses)
                {
                    if (ipAddress.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return new IPAddressSubnet(ipAddress.Address, ipAddress.IPv4Mask);
                    }
                }

                return null;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder()
                .Append("Network Adapter: ").AppendLine(NetworkInterface.Name)
                .Append("\tId: ").AppendLine(NetworkInterface.Id)
                .Append("\tPrimary Address Subnet: ").AppendLine(PrimaryAddressSubnet?.ToString() ?? "null")
                .Append("\tOperational Status: ").AppendLine(NetworkInterface.OperationalStatus.ToString())
                .Append("\tNetwork Interface Type: ").AppendLine(NetworkInterface.NetworkInterfaceType.ToString())
                .Append("\tDescription: ").AppendLine(NetworkInterface.Description);

            return sb.ToString();
        }
    }
}