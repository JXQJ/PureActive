using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using PureActive.Hosting.Abstractions.Types;
using PureActive.Hosting.Hosting;
using PureActive.Logging.Extensions.Extensions;
using PureActive.Network.Abstractions.CommonNetworkServices;
using PureActive.Network.Abstractions.DhcpService.Events;
using PureActive.Network.Abstractions.DhcpService.Interfaces;
using PureActive.Network.Abstractions.DhcpService.Types;
using PureActive.Network.Abstractions.Extensions;
using PureActive.Network.Core.Sockets;
using PureActive.Network.Services.DhcpService.Events;
using PureActive.Network.Services.DhcpService.Session;

namespace PureActive.Network.Services.DhcpService
{
    public class DhcpService : HostedServiceInternal<DhcpService>, IDhcpService, IDhcpSessionMgr
    {
        public ICommonNetworkServices CommonNetworkServices { get; }

        #region Private Properties

        private readonly HostedSocketService _hostedSocketService;

        private UdpListener _listenerDhcpServer;

        private readonly Dictionary<PhysicalAddress, DhcpSession> _dhcpSessions = new Dictionary<PhysicalAddress, DhcpSession>();

        private readonly object _sessionsLock = new object();

        #endregion Private Properties

        #region Public Properties

        /// <summary>
        /// Interface IP address.
        /// </summary>
        public IPAddress InterfaceAddress
        {
            get => _hostedSocketService.InterfaceAddress;
            set => _hostedSocketService.InterfaceAddress = value;
        }

        public IDhcpSessionMgr DhcpSessionMgr => this;

        #endregion Public Properties

        public DhcpService(ICommonNetworkServices commonNetworkServices, IApplicationLifetime applicationLifetime = null):
            base(commonNetworkServices?.CommonServices, applicationLifetime, ServiceHost.DhcpService)
        {
            CommonNetworkServices = commonNetworkServices ?? throw new ArgumentNullException(nameof(commonNetworkServices));
            _hostedSocketService = new HostedSocketService(commonNetworkServices.CommonServices?.LoggerFactory?.CreatePureLogger<SocketService>());
        }

        #region Methods

    public override Task StartAsync(CancellationToken cancellationToken)
        {
            ServiceHostStatus = ServiceHostStatus.StartPending;

            _listenerDhcpServer = new UdpListener(CommonServices.LoggerFactory)
            {
                InterfaceAddress = IPAddress.Any,
                BufferSize = DhcpConstants.DhcpMaxMessageSize,
                ReceiveTimeout = DhcpConstants.DhcpReceiveTimeout,
                SendTimeout = DhcpConstants.DhcpSendTimeout
            };

            _listenerDhcpServer.ClientConnected += OnDhcpServerClientConnect;
            _listenerDhcpServer.ClientDisconnected += OnDhcpServerClientDisconnect;

            var retVal = _listenerDhcpServer.Start(DhcpConstants.DhcpServicePort, true);

            if (retVal)
                Logger?.LogInformation("DhcpService started listening on port {DhcpServicePort}", DhcpConstants.DhcpServicePort);
            else
                Logger?.LogDebug("DhcpService failed to start listening on port {DhcpServicePort}", DhcpConstants.DhcpServicePort);

            ServiceHostStatus = ServiceHostStatus.Running;

            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            ServiceHostStatus = ServiceHostStatus.StopPending;

            try
            {
                var retVal = _listenerDhcpServer.Stop();

                if (retVal)
                    Logger?.LogInformation("DhcpService stopped listening on port {DhcpServicePort}",
                        DhcpConstants.DhcpServicePort);
                else
                    Logger?.LogDebug("DhcpService failed to stop listening on port {DhcpServicePort}",
                        DhcpConstants.DhcpServicePort);

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, "DhcpService failed to stop listening on port {DhcpServicePort}",
                    DhcpConstants.DhcpServicePort);

                return Task.FromException(ex);
            }
            finally
            {
                ServiceHostStatus = ServiceHostStatus.Stopped;
            }
        }

        /// <summary>
        ///  Remote client connects and makes a request.
        /// </summary>
        private void OnDhcpServerClientConnect(object sender, ClientConnectedEventArgs args)
        {
            SocketBuffer channelBuffer = args.ChannelBuffer;

            if (channelBuffer != null
                && args.Channel.IsConnected
                && channelBuffer.BytesTransferred >= DhcpConstants.DhcpMinMessageSize
                && channelBuffer.BytesTransferred <= DhcpConstants.DhcpMaxMessageSize)
            {
                Logger?.LogTrace("DHCP PACKET received on {LocalEndPoint} with channel id {ChannelId} was received from {RemoteEndPoint} and queued for processing...",
                    args.Channel.Socket.LocalEndPoint,
                    args.Channel.ChannelId,
                    args.Channel.RemoteEndpoint);

                DhcpMessageEventArgs messageArgs = new DhcpMessageEventArgs(this, args.Channel, args.ChannelBuffer);
                OnDhcpMessageReceived(this, messageArgs);

                ThreadPool.QueueUserWorkItem(ProcessRequest, messageArgs);
            }
        }


        public DhcpSession this[PhysicalAddress physicalAddress]
        {
            get
            {
                lock (_sessionsLock)
                {
                    return _dhcpSessions[physicalAddress];
                }
            }
        }

        public IDhcpSession FindOrCreateDhcpSession(PhysicalAddress physicalAddress)
        {
            lock (_sessionsLock)
            {
                if (_dhcpSessions.TryGetValue(physicalAddress, out var dhcpSession)) return dhcpSession;

                dhcpSession = new DhcpSession(this, CommonServices.LoggerFactory.CreatePureLogger<DhcpSession>(), physicalAddress);
                _dhcpSessions.Add(physicalAddress, dhcpSession);

                return dhcpSession;
            }
        }


        /// <summary>
        ///  Remote client is disconnected.
        /// </summary>
        private void OnDhcpServerClientDisconnect(object sender, ClientDisconnectedEventArgs args)
        {
            Logger?.LogDebug(args.Exception, "DhcpServer client was disconnected");
        }

        private LogLevel LogLevelFromDhcpMessageProcessed(DhcpMessageProcessed dhcpMessageProcessed)
        {
            switch (dhcpMessageProcessed)
            {
                case DhcpMessageProcessed.Duplicate:
                    break;
                case DhcpMessageProcessed.Success:
                    return LogLevel.Information;
                case DhcpMessageProcessed.Unknown:
                case DhcpMessageProcessed.Ignored:
                case DhcpMessageProcessed.Failed:
                    return LogLevel.Debug;
            }

            return LogLevel.Trace;
        }

        /// <summary>
        ///  Process boot REQUEST and send reply back to remote client.
        /// </summary>
        private void ProcessRequest(Object state)
        {
            DhcpMessageEventArgs args = (DhcpMessageEventArgs)state;

            if (args.RequestMessage.Operation == OperationCode.BootRequest ||
                args.RequestMessage.Operation == OperationCode.BootReply)
            {
                DhcpMessageProcessed dhcpMessageProcessed = DhcpMessageProcessed.Unknown;

                var dhcpSession = DhcpSessionMgr?.FindOrCreateDhcpSession(args.RequestMessage.ClientHardwareAddress);

                if (dhcpSession != null)
                {
                    // classify the client message type
                    switch (args.MessageType)
                    {
                        case MessageType.Discover:
                            dhcpMessageProcessed = dhcpSession.ProcessDiscover(args.RequestMessage);
                            break;

                        case MessageType.Request:
                            {
                                dhcpMessageProcessed = dhcpSession.ProcessRequest(args.RequestMessage);

                                if (dhcpMessageProcessed == DhcpMessageProcessed.Success)
                                {
                                    var dhcpDiscoveredDevice = dhcpSession.DhcpDiscoveredDevice;

                                    if (dhcpDiscoveredDevice != null)
                                    {
                                        // Fix up IPAddress by calling ArpService
                                        if (dhcpDiscoveredDevice.IpAddress.Equals(IPAddress.None))
                                        {
                                            dhcpDiscoveredDevice.IpAddress =
                                                dhcpDiscoveredDevice.PhysicalAddress.Equals(PhysicalAddress.None)
                                                    ? IPAddress.None
                                                    : CommonNetworkServices.ArpService.GetIPAddress(
                                                        dhcpDiscoveredDevice.PhysicalAddress, true);
                                        }
                                        OnDhcpDiscoveredDevice(this, new DhcpDiscoveredDeviceEvent(this, dhcpDiscoveredDevice));
                                    }
                                }

                                break;
                            }

                        case MessageType.Decline:
                            dhcpMessageProcessed = dhcpSession.ProcessDecline(args.RequestMessage);
                            break;

                        case MessageType.Release:
                            dhcpMessageProcessed = dhcpSession.ProcessRelease(args.RequestMessage);
                            break;

                        case MessageType.Inform:
                            dhcpMessageProcessed = dhcpSession.ProcessInform(args.RequestMessage);
                            break;

                        case MessageType.Ack:
                            dhcpMessageProcessed = dhcpSession.ProcessAck(args.RequestMessage);
                            break;

                        case MessageType.Nak:
                        case MessageType.Offer:
                            break;
                    }
                }

                var msgLogLevel = LogLevelFromDhcpMessageProcessed(dhcpMessageProcessed);

                // log that the packet was successfully parsed
                using (args.RequestMessage.PushLogProperties(msgLogLevel))
                {
                    Logger?.Log(msgLogLevel,
                        "DHCP {DhcpMessageType} message with session id {DhcpSessionId} from client {ClientHardwareAddress} with status {DhcpMessageProcessed} on thread #{ThreadId}",
                        args.MessageType, args.RequestMessage.SessionId, 
                        args.RequestMessage.ClientHardwareAddress.ToColonString(),
                        DhcpMessageProcessedString.GetName(dhcpMessageProcessed),
                        Thread.CurrentThread.ManagedThreadId);
                }
            }
            else
            {
                using (args.RequestMessage.PushLogProperties(LogLevel.Debug))
                {
                    Logger?.LogDebug("UNKNOWN operation code {OperationCode} received and ignored",
                        args.RequestMessage.Operation);
                }
            }
        }
        #endregion Methods

        #region Events

        /// <summary>
        ///     A dhcp message was received and processed.
        /// </summary>
        public event DhcpMessageEventHandler OnDhcpMessageReceived = delegate { };

        /// <summary>
        ///     A dhcp message was received and processed.
        /// </summary>
        public event EventHandler<DhcpDiscoveredDeviceEvent> OnDhcpDiscoveredDevice = delegate { };

        #endregion Events
    }
}
