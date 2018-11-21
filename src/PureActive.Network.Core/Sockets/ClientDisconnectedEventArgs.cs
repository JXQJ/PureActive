// ***********************************************************************
// Assembly         : PureActive.Network.Core
// Author           : SteveBu
// Created          : 11-05-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="ClientDisconnectedEventArgs.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Net.Sockets;

namespace PureActive.Network.Core.Sockets
{
    /// <summary>
    /// Event arguments for <see cref="SocketListener.ClientDisconnected" />.
    /// Implements the <see cref="System.EventArgs" />
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class ClientDisconnectedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientDisconnectedEventArgs" /> class.
        /// </summary>
        /// <param name="socket">The channel that disconnected.</param>
        /// <param name="exception">The exception that was caught.</param>
        /// <exception cref="ArgumentNullException">
        /// socket
        /// or
        /// exception
        /// </exception>
        public ClientDisconnectedEventArgs(Socket socket, Exception exception)
        {
            Socket = socket ?? throw new ArgumentNullException("socket");
            Exception = exception ?? throw new ArgumentNullException("exception");
        }

        /// <summary>
        /// Channel that was disconnected
        /// </summary>
        /// <value>The socket.</value>
        public Socket Socket { get; private set; }

        /// <summary>
        /// Exception that was caught (is SocketException if the connection failed or if the remote end point disconnected).
        /// </summary>
        /// <value>The exception.</value>
        /// <remarks><c>SocketException</c> with status <c>Success</c> is created for graceful disconnects.</remarks>
        public Exception Exception { get; private set; }
    }
}