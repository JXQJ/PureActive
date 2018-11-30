﻿// ***********************************************************************
// Assembly         : PureActive.Network.Devices
// Author           : SteveBu
// Created          : 11-02-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="NetworkGateway.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using Microsoft.Extensions.Logging;
using PureActive.Hosting.Abstractions.Extensions;
using PureActive.Hosting.Abstractions.Types;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Logging.Abstractions.Types;
using PureActive.Logging.Extensions.Types;
using PureActive.Network.Abstractions.CommonNetworkServices;
using PureActive.Network.Abstractions.Network;
using PureActive.Network.Abstractions.Types;

namespace PureActive.Network.Devices.Network
{
    /// <summary>
    /// Class NetworkGateway.
    /// Implements the <see cref="NetworkDeviceBase" />
    /// Implements the <see cref="INetworkGateway" />
    /// </summary>
    /// <seealso cref="NetworkDeviceBase" />
    /// <seealso cref="INetworkGateway" />
    /// <autogeneratedoc />
    public class NetworkGateway : NetworkDeviceBase, INetworkGateway
    {
        /// <summary>
        /// The ip address subnet
        /// </summary>
        /// <autogeneratedoc />
        private IPAddressSubnet _ipAddressSubnet;
        /// <summary>
        /// The physical address
        /// </summary>
        /// <autogeneratedoc />
        private PhysicalAddress _physicalAddress = PhysicalAddress.None;

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkGateway"/> class.
        /// </summary>
        /// <param name="commonNetworkServices">The common network services.</param>
        /// <param name="ipAddressSubnetGateway">The ip address subnet gateway.</param>
        /// <exception cref="ArgumentNullException">ipAddressSubnetGateway</exception>
        /// <autogeneratedoc />
        public NetworkGateway(ICommonNetworkServices commonNetworkServices, IPAddressSubnet ipAddressSubnetGateway) :
            base(commonNetworkServices, DeviceType.NetworkGateway,
                commonNetworkServices?.LoggerFactory.CreatePureLogger<NetworkGateway>())
        {
            _ipAddressSubnet = ipAddressSubnetGateway ??
                               throw new ArgumentNullException(nameof(ipAddressSubnetGateway));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkGateway"/> class.
        /// </summary>
        /// <param name="commonNetworkServices">The common network services.</param>
        /// <param name="ipAddressGateway">The ip address gateway.</param>
        /// <param name="subnetMask">The subnet mask.</param>
        /// <autogeneratedoc />
        public NetworkGateway(ICommonNetworkServices commonNetworkServices, IPAddress ipAddressGateway,
            IPAddress subnetMask) :
            this(commonNetworkServices, new IPAddressSubnet(ipAddressGateway, subnetMask))
        {
        }

        /// <summary>
        /// Gets or sets the ip address subnet.
        /// </summary>
        /// <value>The ip address subnet.</value>
        /// <exception cref="ArgumentNullException">IPAddressSubnet</exception>
        /// <autogeneratedoc />
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

        /// <summary>
        /// Gets the ip address.
        /// </summary>
        /// <value>The ip address.</value>
        /// <autogeneratedoc />
        public IPAddress IPAddress => IPAddressSubnet?.IPAddress;
        /// <summary>
        /// Gets the subnet mask.
        /// </summary>
        /// <value>The subnet mask.</value>
        /// <autogeneratedoc />
        public IPAddress SubnetMask => IPAddressSubnet?.SubnetMask;

        /// <summary>
        /// Gets the physical address.
        /// </summary>
        /// <value>The physical address.</value>
        /// <autogeneratedoc />
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

        /// <summary>
        /// Gets the log property list level.
        /// </summary>
        /// <param name="logLevel">The log level.</param>
        /// <param name="loggableFormat">The loggable format.</param>
        /// <returns>IEnumerable&lt;IPureLogPropertyLevel&gt;.</returns>
        /// <autogeneratedoc />
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