﻿// ***********************************************************************
// Assembly         : PureActive.Network.Abstractions
// Author           : SteveBu
// Created          : 11-02-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="DeviceInfo.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using PureActive.Network.Abstractions.DeviceInfo;

namespace PureActive.Network.Abstractions.Types
{
    /// <summary>
    /// Class DeviceInfo.
    /// Implements the <see cref="IDeviceInfo" />
    /// </summary>
    /// <seealso cref="IDeviceInfo" />
    /// <autogeneratedoc />
    public abstract class DeviceInfo : IDeviceInfo
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        /// <autogeneratedoc />
        public string Id { get; protected set; }
        /// <summary>
        /// Gets the model.
        /// </summary>
        /// <value>The model.</value>
        /// <autogeneratedoc />
        public string Model { get; protected set; }
        /// <summary>
        /// Gets the manufacturer.
        /// </summary>
        /// <value>The manufacturer.</value>
        /// <autogeneratedoc />
        public string Manufacturer { get; protected set; }
        /// <summary>
        /// Gets the name of the device.
        /// </summary>
        /// <value>The name of the device.</value>
        /// <autogeneratedoc />
        public string DeviceName { get; protected set; }
        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>The version.</value>
        /// <autogeneratedoc />
        public string Version { get; protected set; }
        /// <summary>
        /// Gets the version number.
        /// </summary>
        /// <value>The version number.</value>
        /// <autogeneratedoc />
        public Version VersionNumber { get; protected set; }
        /// <summary>
        /// Gets the application version.
        /// </summary>
        /// <value>The application version.</value>
        /// <autogeneratedoc />
        public string AppVersion { get; protected set; }
        /// <summary>
        /// Gets the application build.
        /// </summary>
        /// <value>The application build.</value>
        /// <autogeneratedoc />
        public string AppBuild { get; protected set; }
        /// <summary>
        /// Gets the device platform.
        /// </summary>
        /// <value>The device platform.</value>
        /// <autogeneratedoc />
        public DevicePlatform DevicePlatform { get; protected set; }
        /// <summary>
        /// Gets the type of the device.
        /// </summary>
        /// <value>The type of the device.</value>
        /// <autogeneratedoc />
        public DeviceType DeviceType { get; protected set; }
        /// <summary>
        /// Gets a value indicating whether this instance is device.
        /// </summary>
        /// <value><c>true</c> if this instance is device; otherwise, <c>false</c>.</value>
        /// <autogeneratedoc />
        public bool IsDevice { get; protected set; }
    }
}