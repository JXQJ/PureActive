﻿// ***********************************************************************
// Assembly         : PureActive.Network.Abstractions
// Author           : SteveBu
// Created          : 11-02-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="GatewayIPAddressInformationCollectionExtensions.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace PureActive.Hosting.Abstractions.Extensions
{
    /// <summary>
    /// Class GatewayIPAddressInformationCollectionExtensions.
    /// </summary>
    /// <autogeneratedoc />
    public static class GatewayIPAddressInformationCollectionExtensions
    {
        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Is the IPAddress IPv4 or default.
        /// </summary>
        /// <param name="gatewayIPAddressInformationCollection">The gateway ip address information collection.</param>
        /// <returns>GatewayIPAddressInformation.</returns>
        /// <autogeneratedoc />
        public static GatewayIPAddressInformation IPv4OrDefault(this GatewayIPAddressInformationCollection gatewayIPAddressInformationCollection)
        {
            foreach (var gatewayIPAddressInformation in gatewayIPAddressInformationCollection)
            {
                // Looking for an IPv4 Gateway Address
                if (gatewayIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
                    return gatewayIPAddressInformation;
            }

            return null;
        }
    }
}