using System;

namespace PureActive.Network.Services.Sockets
{
    /// <summary>
    /// <see cref="SocketChannel"/> was disconnected
    /// </summary>
    /// <param name="exception">Channel which got disconnected</param>
    /// <param name="exception">Exception  (<c>SocketException</c> for TCP/IP errors)</param>
    /// <seealso cref="SocketChannel"/>
    public delegate void DisconnectHandler(object sender, Exception exception);
}