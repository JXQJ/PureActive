using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using Microsoft.Extensions.Logging;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Logging.Abstractions.Types;
using PureActive.Logging.Extensions.Types;
using PureActive.Network.Abstractions.CommonNetworkServices;
using PureActive.Network.Abstractions.Extensions;
using PureActive.Network.Abstractions.Network;
using PureActive.Network.Abstractions.Types;

namespace PureActive.Network.Devices.Network
{
    public class NetworkGateway : NetworkDeviceBase, INetworkGateway
    {
        private IPAddressSubnet _ipAddressSubnet;
        private PhysicalAddress _physicalAddress = PhysicalAddress.None;

        public NetworkGateway(ICommonNetworkServices commonNetworkServices, IPAddressSubnet ipAddressSubnetGateway) :
            base(commonNetworkServices, DeviceType.NetworkGateway,
                commonNetworkServices?.LoggerFactory.CreatePureLogger<NetworkGateway>())
        {
            _ipAddressSubnet = ipAddressSubnetGateway ??
                               throw new ArgumentNullException(nameof(ipAddressSubnetGateway));
        }

        public NetworkGateway(ICommonNetworkServices commonNetworkServices, IPAddress ipAddressGateway,
            IPAddress subnetMask) :
            this(commonNetworkServices, new IPAddressSubnet(ipAddressGateway, subnetMask))
        {
        }

        public IPAddressSubnet IPAddressSubnet
        {
            get => _ipAddressSubnet;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(IPAddressSubnet));

                if (_ipAddressSubnet.Equals(value)) return;

                _ipAddressSubnet = value;
                _physicalAddress = PhysicalAddress.None;
            }
        }

        public IPAddress IPAddress => IPAddressSubnet?.IPAddress;
        public IPAddress SubnetMask => IPAddressSubnet?.SubnetMask;

        public PhysicalAddress PhysicalAddress
        {
            get
            {
                if (_physicalAddress.Equals(PhysicalAddress.None))
                {
                    _physicalAddress = CommonNetworkServices.ArpService.GetPhysicalAddress(IPAddress);
                }

                return _physicalAddress;
            }
        }

        public override IEnumerable<IPureLogPropertyLevel> GetLogPropertyListLevel(LogLevel logLevel,
            LoggableFormat loggableFormat)
        {
            var logPropertyLevels = loggableFormat.IsWithParents()
                ? base.GetLogPropertyListLevel(logLevel, loggableFormat)?.ToList()
                : new List<IPureLogPropertyLevel>();

            if (logLevel > LogLevel.Information) return logPropertyLevels;

            logPropertyLevels?.Add(new PureLogPropertyLevel("GatewayIPAddressSubnet", IPAddressSubnet,
                LogLevel.Information));
            logPropertyLevels?.Add(new PureLogPropertyLevel("GatewayPhysicalAddress", PhysicalAddress.ToDashString(),
                LogLevel.Information));

            return logPropertyLevels;
        }
    }
}