using System;

namespace PureActive.Network.Services.Sockets
{
    /// <summary>
    /// <see cref="SocketChannel"/> have sent or received a message.
    /// </summary>
    /// <param name="exception">Channel that did the work</param>
    /// <param name="exception">Message. depends on which encoder/decoder was used.</param>
    /// <remarks>We uses delegates instead of regular events to make sure that there are only one subscriber and that it's configured once.</remarks>
    public delegate void ErrorHandler(object sender, Exception exception);
}