﻿// ***********************************************************************
// Assembly         : PureActive.Network.Abstractions
// Author           : SteveBu
// Created          : 11-02-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="DeviceType.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace PureActive.Network.Abstractions.Types
{
    /// <summary>
    /// Enum DeviceType
    /// </summary>
    /// <autogeneratedoc />
    [Flags]
    public enum DeviceType : ulong
    {
        /// <summary>
        /// The unknown device
        /// </summary>
        /// <autogeneratedoc />
        UnknownDevice = 0,
        /// <summary>
        /// The local device
        /// </summary>
        /// <autogeneratedoc />
        LocalDevice = 1 << 1,
        /// <summary>
        /// The network device
        /// </summary>
        /// <autogeneratedoc />
        NetworkDevice = 1 << 2,
        /// <summary>
        /// The computer
        /// </summary>
        /// <autogeneratedoc />
        Computer = 1 << 3,
        /// <summary>
        /// The phone
        /// </summary>
        /// <autogeneratedoc />
        Phone = 1 << 4,
        /// <summary>
        /// The tablet
        /// </summary>
        /// <autogeneratedoc />
        Tablet = 1 << 4,
        /// <summary>
        /// The media device
        /// </summary>
        /// <autogeneratedoc />
        MediaDevice = 1 << 5,
        /// <summary>
        /// The smart tv
        /// </summary>
        /// <autogeneratedoc />
        SmartTv = 1 << 6,
        /// <summary>
        /// The game console
        /// </summary>
        /// <autogeneratedoc />
        GameConsole = 1 << 6,
        /// <summary>
        /// The car
        /// </summary>
        /// <autogeneratedoc />
        Car = 1 << 7,
        /// <summary>
        /// The watch
        /// </summary>
        /// <autogeneratedoc />
        Watch = 1 << 8,
        /// <summary>
        /// The storage
        /// </summary>
        /// <autogeneratedoc />
        Storage = 1 << 7,
        /// <summary>
        /// The printer
        /// </summary>
        /// <autogeneratedoc />
        Printer = 1 << 8,
        /// <summary>
        /// The gateway
        /// </summary>
        /// <autogeneratedoc />
        Gateway = 1 << 9,
        /// <summary>
        /// The access point
        /// </summary>
        /// <autogeneratedoc />
        AccessPoint = 1 << 10,
        /// <summary>
        /// The network
        /// </summary>
        /// <autogeneratedoc />
        Network = 1 << 11,

        /// <summary>
        /// The local network device
        /// </summary>
        /// <autogeneratedoc />
        LocalNetworkDevice = LocalDevice | NetworkDevice,

        /// <summary>
        /// The local computer
        /// </summary>
        /// <autogeneratedoc />
        LocalComputer = LocalNetworkDevice | Computer,
        /// <summary>
        /// The remote computer
        /// </summary>
        /// <autogeneratedoc />
        RemoteComputer = NetworkDevice | Computer,

        /// <summary>
        /// The local cell phone
        /// </summary>
        /// <autogeneratedoc />
        LocalCellPhone = LocalNetworkDevice | Phone,
        /// <summary>
        /// The roaming cell phone
        /// </summary>
        /// <autogeneratedoc />
        RoamingCellPhone = NetworkDevice | Phone,

        /// <summary>
        /// The network gateway
        /// </summary>
        /// <autogeneratedoc />
        NetworkGateway = NetworkDevice | Gateway,
        /// <summary>
        /// The network access point
        /// </summary>
        /// <autogeneratedoc />
        NetworkAccessPoint = NetworkDevice | AccessPoint,

        /// <summary>
        /// The local network
        /// </summary>
        /// <autogeneratedoc />
        LocalNetwork = LocalDevice | Network,
        /// <summary>
        /// The internet
        /// </summary>
        /// <autogeneratedoc />
        Internet = Network + 1,

        /// <summary>
        /// The network adapter
        /// </summary>
        /// <autogeneratedoc />
        NetworkAdapter = LocalNetworkDevice + 1
    }
}