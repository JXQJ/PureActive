using System;
using Microsoft.Extensions.Logging;
using PureActive.Core.Extensions;
using PureActive.Network.Abstractions.DhcpService.Interfaces;
using PureActive.Network.Abstractions.DhcpService.Types;
using PureActive.Network.Core.Sockets;
using PureActive.Network.Services.DhcpService.Message;

namespace PureActive.Network.Services.DhcpService.Events
{
    public class DhcpMessageEventArgs : EventArgs
    {
        public IDhcpService DhcpService { get; private set; }

        /// <summary>
        ///     Channel for the connected client.
        /// </summary>
        public SocketChannel Channel { get; private set; }

        /// <summary>
        ///     Buffer for the connected client.
        /// </summary>
        public SocketBuffer ChannelBuffer { get; private set; }

        /// <summary>
        ///     Requested message for the connected client.
        /// </summary>
        public IDhcpMessage RequestMessage { get; private set; }

        public MessageType MessageType
        {
            get
            {
                // get message type option
                var messageTypeData = RequestMessage.GetOptionData(DhcpOption.MessageType);

                return messageTypeData != null && messageTypeData.Length > 0 ? (MessageType)messageTypeData[0] : MessageType.Unknown;
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DhcpMessageEventArgs"/> class.
        /// </summary>
        /// <param name="dhcpService">Controlling DhcpService interface</param>
        /// <param name="channel">Socket channel request is received on.</param>
        /// <param name="data">Raw data received from socket.</param>
        public DhcpMessageEventArgs(IDhcpService dhcpService, SocketChannel channel, SocketBuffer data)
        {
            Channel = channel ?? throw new ArgumentNullException(nameof(channel));
            ChannelBuffer = data ?? throw new ArgumentNullException(nameof(data));
            DhcpService = dhcpService ?? throw new ArgumentNullException(nameof(DhcpService));

            var logger = dhcpService.Logger;

            try
            {
                // Parse the dhcp message
                RequestMessage = new DhcpMessage(data.Buffer, dhcpService.LoggerFactory, logger);

     
             logger?.LogTrace(
                    "DHCP PACKET with message id {SessionId} successfully parsed from client endpoint {RemoteEndPoint}",
                    RequestMessage.SessionId.ToHexString("0x"), Channel.RemoteEndpoint);
        }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Error parsing DHCP message");
            }
        }
    }
}
