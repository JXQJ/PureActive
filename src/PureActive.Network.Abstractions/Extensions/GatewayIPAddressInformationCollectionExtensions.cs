using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace PureActive.Network.Abstractions.Extensions
{
    public static class GatewayIPAddressInformationCollectionExtensions
    {
        // ReSharper disable once InconsistentNaming
        public static GatewayIPAddressInformation IPv4OrDefault(
            this GatewayIPAddressInformationCollection gatewayIPAddressInformationCollection)
        {
            GatewayIPAddressInformation gatewayIPAddressInformationReturn = null;

            foreach (var gatewayIPAddressInformation in gatewayIPAddressInformationCollection)
            {
                // Looking for an IPv4 Gateway Address
                if (gatewayIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
                    return gatewayIPAddressInformation;

                gatewayIPAddressInformationReturn = gatewayIPAddressInformation;
            }

            return gatewayIPAddressInformationReturn;
        }
    }
}