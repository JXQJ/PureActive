// ***********************************************************************
// Assembly         : PureActive.Network.Core
// Author           : SteveBu
// Created          : 11-05-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="SocketEventArgs.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.IO;
using System.Net.Sockets;
using PureActive.Logging.Abstractions.Interfaces;

namespace PureActive.Network.Core.Sockets
{
    /// <summary>
    /// Used by <see cref="SocketListener.ClientConnected" />.
    /// Implements the <see cref="System.EventArgs" />
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class SocketEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientConnectedEventArgs" /> class.
        /// </summary>
        /// <param name="socket">The channel.</param>
        /// <param name="logger">Logger</param>
        /// <exception cref="ArgumentNullException">socket</exception>
        public SocketEventArgs(Socket socket, IPureLogger logger)
        {
            if (socket == null) throw new ArgumentNullException("socket");

            Channel = new SocketChannel(logger);
            ChannelBuffer = new SocketBuffer();
            Channel.Assign(socket);
        }

        /// <summary>
        /// Channel for the connected client
        /// </summary>
        /// <value>The channel.</value>
        public SocketChannel Channel { get; private set; }

        /// <summary>
        /// Buffer for the connected client
        /// </summary>
        /// <value>The channel buffer.</value>
        public SocketBuffer ChannelBuffer { get; set; }

        /// <summary>
        /// Response (only if the client may not connect)
        /// </summary>
        /// <value>The response.</value>
        public Stream Response { get; set; }
    }
}