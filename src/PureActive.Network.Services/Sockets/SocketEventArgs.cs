using System;
using System.IO;
using System.Net.Sockets;
using PureActive.Logging.Abstractions.Interfaces;

namespace PureActive.Network.Services.Sockets
{
    /// <summary>
    ///     Used by <see cref="SocketListener.ClientConnected"/>.
    /// </summary>
    public class SocketEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ClientConnectedEventArgs" /> class.
        /// </summary>
        /// <param name="socket">The channel.</param>
        /// <param name="logger">Logger</param>
        public SocketEventArgs(Socket socket, IPureLogger logger)
        {
            if (socket == null) throw new ArgumentNullException("socket");

            Channel = new SocketChannel(logger);
            ChannelBuffer = new SocketBuffer();
            Channel.Assign(socket);
        }

        /// <summary>
        ///     Channel for the connected client
        /// </summary>
        public SocketChannel Channel { get; private set; }

        /// <summary>
        ///     Buffer for the connected client
        /// </summary>
        public SocketBuffer ChannelBuffer { get; set; }

        /// <summary>
        ///     Response (only if the client may not connect)
        /// </summary>
        public Stream Response { get; set; }

    }
}