// ***********************************************************************
// Assembly         : PureActive.Network.Core
// Author           : SteveBu
// Created          : 11-05-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-17-2018
// ***********************************************************************
// <copyright file="ClientConnectedEventArgs.cs" company="BushChang Corporation">
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
    /// Used by <see cref="SocketListener.OnClientConnected" />.
    /// Implements the <see cref="System.EventArgs" />
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class ClientConnectedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientConnectedEventArgs" /> class.
        /// </summary>
        /// <param name="socket">The channel.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException">socket</exception>
        public ClientConnectedEventArgs(Socket socket, IPureLogger logger)
        {
            if (socket == null) throw new ArgumentNullException("socket");

            AllowConnect = true;

            Channel = new SocketChannel(logger);
            ChannelBuffer = new SocketBuffer();
            Channel.Assign(socket);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientConnectedEventArgs" /> class.
        /// </summary>
        /// <param name="socket">The channel.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="messageBuffer">The message buffer.</param>
        /// <exception cref="ArgumentNullException">socket</exception>
        public ClientConnectedEventArgs(Socket socket, IPureLogger logger, int messageBuffer)
        {
            if (socket == null) throw new ArgumentNullException(nameof(socket));

            AllowConnect = true;

            Channel = new SocketChannel(logger);
            ChannelBuffer = new SocketBuffer(messageBuffer);
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

        /// <summary>
        /// Determines if the client may connect.
        /// </summary>
        /// <value><c>true</c> if [allow connect]; otherwise, <c>false</c>.</value>
        public bool AllowConnect { get; set; }

        /// <summary>
        /// Cancel connection, will make the listener close it.
        /// </summary>
        public void CancelConnection()
        {
            AllowConnect = false;
        }

        /// <summary>
        /// Close the listener, but send a response (you are yourself responsible of encoding it to a message)
        /// </summary>
        /// <param name="response">Stream with encoded message (which can be sent as-is).</param>
        /// <exception cref="ArgumentNullException">response</exception>
        public void CancelConnection(Stream response)
        {
            Response = response ?? throw new ArgumentNullException(nameof(response));
            AllowConnect = false;
        }
    }
}