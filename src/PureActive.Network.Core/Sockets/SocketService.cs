﻿// ***********************************************************************
// Assembly         : PureActive.Network.Core
// Author           : SteveBu
// Created          : 11-05-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="SocketService.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Net;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Network.Abstractions.Networking;

namespace PureActive.Network.Core.Sockets
{
    /// <summary>
    /// Implementation of <see cref="SocketService" /> that serves socket requests from remote clients.
    /// Implements the <see cref="IDisposable" />
    /// </summary>
    /// <seealso cref="IDisposable" />
    public abstract class SocketService : IDisposable
    {
        #region Private Properties

        /// <summary>
        /// The interface address
        /// </summary>
        /// <autogeneratedoc />
        private IPAddress _interfaceAddress;
        /// <summary>
        /// The service port
        /// </summary>
        /// <autogeneratedoc />
        private int _servicePort;

        /// <summary>
        /// The logger.
        /// </summary>
        /// <value>The logger.</value>
        public IPureLogger Logger { get; }

        #endregion Private Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the ip address for receiving data
        /// </summary>
        /// <value>The interface address.</value>
        public IPAddress InterfaceAddress
        {
            get { return _interfaceAddress; }
            set { _interfaceAddress = value; }
        }

        /// <summary>
        /// Gets or sets the port for receiving data
        /// </summary>
        /// <value>The service port.</value>
        public int ServicePort
        {
            get { return _servicePort; }
            set { _servicePort = value; }
        }

        #endregion Public Properties

        #region Constructors / Deconstructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketService" /> class.
        /// </summary>
        /// <param name="networkingService">The networking service</param>
        /// <param name="logger">The logger.</param>
        protected SocketService(INetworkingService networkingService, IPureLogger<SocketService> logger)
        {
            if (networkingService == null) throw new ArgumentNullException(nameof(networkingService));

            Logger = logger;

            _interfaceAddress = networkingService.GetDefaultLocalAddress();
        }

        /// <summary>
        /// Handles object cleanup for GC finalization.
        /// </summary>
        ~SocketService()
        {
            Dispose(false);
        }

        /// <summary>
        /// Handles object cleanup.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Handles object cleanup
        /// </summary>
        /// <param name="disposing">True if called from Dispose(); false if called from GC finalization.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }

        #endregion  Constructors / Deconstructors

        #region Methods

        /// <summary>
        /// Starts the service listener if it is in a stopped state.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public abstract bool Start();

        /// <summary>
        /// Stops the service listener if in started state.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public abstract bool Stop();

        /// <summary>
        /// Restarts the service listener if in a started state.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Restart()
        {
            Stop();
            return Start();
        }

        #endregion Methods
    }
}