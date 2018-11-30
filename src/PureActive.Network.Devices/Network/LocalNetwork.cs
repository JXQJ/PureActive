﻿// ***********************************************************************
// Assembly         : PureActive.Network.Devices
// Author           : SteveBu
// Created          : 11-02-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="LocalNetwork.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Linq;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Network.Abstractions.CommonNetworkServices;
using PureActive.Network.Abstractions.Local;
using PureActive.Network.Abstractions.Network;
using PureActive.Network.Abstractions.Types;

namespace PureActive.Network.Devices.Network
{
    /// <summary>
    /// Class LocalNetwork.
    /// Implements the <see cref="NetworkDeviceBase" />
    /// Implements the <see cref="ILocalNetwork" />
    /// </summary>
    /// <seealso cref="NetworkDeviceBase" />
    /// <seealso cref="ILocalNetwork" />
    /// <autogeneratedoc />
    public class LocalNetwork : NetworkDeviceBase, ILocalNetwork
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalNetwork"/> class.
        /// </summary>
        /// <param name="commonNetworkServices">The common network services.</param>
        /// <param name="networkAdapter">The network adapter.</param>
        /// <param name="logger">The logger.</param>
        /// <autogeneratedoc />
        public LocalNetwork(ICommonNetworkServices commonNetworkServices, INetworkAdapter networkAdapter,
            IPureLogger logger = null) :
            base(commonNetworkServices, DeviceType.LocalNetwork, logger)
        {
            NetworkAdapterCollection = new NetworkAdapterCollection();
            NetworkGateway = _DiscoverNetworkGateway();

            NetworkIPAddressSubnet = networkAdapter.NetworkAddressSubnet;

            AddAdapterToNetwork(networkAdapter);
        }

        /// <summary>
        /// Gets the network ip address subnet.
        /// </summary>
        /// <value>The network ip address subnet.</value>
        /// <autogeneratedoc />
        public IPAddressSubnet NetworkIPAddressSubnet { get; }
        /// <summary>
        /// Gets the network adapter collection.
        /// </summary>
        /// <value>The network adapter collection.</value>
        /// <autogeneratedoc />
        public INetworkAdapterCollection NetworkAdapterCollection { get; }
        /// <summary>
        /// Gets the network gateway.
        /// </summary>
        /// <value>The network gateway.</value>
        /// <autogeneratedoc />
        public INetworkGateway NetworkGateway { get; }

        /// <summary>
        /// Gets the adapter count.
        /// </summary>
        /// <value>The adapter count.</value>
        /// <autogeneratedoc />
        public int AdapterCount => NetworkAdapterCollection?.Count ?? 0;

        /// <summary>
        /// Adds the adapter to network.
        /// </summary>
        /// <param name="networkAdapter">The network adapter.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <autogeneratedoc />
        public bool AddAdapterToNetwork(INetworkAdapter networkAdapter)
        {
            return NetworkAdapterCollection.Add(networkAdapter);
        }

        /// <summary>
        /// Removes the adapter from network.
        /// </summary>
        /// <param name="networkAdapter">The network adapter.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <autogeneratedoc />
        public bool RemoveAdapterFromNetwork(INetworkAdapter networkAdapter)
        {
            return NetworkAdapterCollection.Remove(networkAdapter);
        }

        /// <summary>
        /// Adapters the connected to network.
        /// </summary>
        /// <param name="networkAdapter">The network adapter.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <autogeneratedoc />
        public bool AdapterConnectedToNetwork(INetworkAdapter networkAdapter)
        {
            return NetworkAdapterCollection.Contains(networkAdapter);
        }

        /// <summary>
        /// Discovers the network gateway.
        /// </summary>
        /// <returns>INetworkGateway.</returns>
        /// <autogeneratedoc />
        private INetworkGateway _DiscoverNetworkGateway()
        {
            var ipAddressGatewaySubnet = CommonNetworkServices.NetworkingService.GetDefaultGatewayAddressSubnet();

            return new NetworkGateway(CommonNetworkServices, ipAddressGatewaySubnet);
        }
    }
}