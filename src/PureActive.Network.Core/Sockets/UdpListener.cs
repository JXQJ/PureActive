using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.Extensions.Logging;
using PureActive.Logging.Abstractions.Interfaces;

namespace PureActive.Network.Core.Sockets
{
    /// <summary>
    /// A class that listen for UDP packets from remote clients.
    /// </summary>
    public class UdpListener : SocketListener
    {
        public UdpListener(IPureLoggerFactory loggerFactory) : base(loggerFactory?.CreatePureLogger<SocketListener>())
        {
            if (loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));
        }

        #region Methods

        /// <summary>
        ///  Starts the service listener if it is in a stopped state.
        /// </summary>
        /// <param name="servicePort">The port used to listen on.</param>
        /// <param name="allowBroadcast">Allows the listener to accept broadcast packets.</param>
        public bool Start(int servicePort, bool allowBroadcast)
        {
            if (servicePort > IPEndPoint.MaxPort || servicePort < IPEndPoint.MinPort)
                throw new ArgumentOutOfRangeException(nameof(servicePort), "Port must be less then " + IPEndPoint.MaxPort + " and more then " + IPEndPoint.MinPort);

            if (IsActive)
                throw new InvalidOperationException("Udp listener is already active and must be stopped before starting");

            if (InterfaceAddress == null)
            {
                var ex = new InvalidOperationException("Unable to set interface address");
                Logger?.LogDebug(ex, "Unable to set interface address for Udp port {ServicePort}", servicePort);
                throw ex;
            }

            try
            {
                Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);

                if (allowBroadcast)
                    Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);

                Socket.Bind(new IPEndPoint(InterfaceAddress, servicePort));
                Socket.ReceiveTimeout = ReceiveTimeout;
                Socket.SendTimeout = SendTimeout;

                IsActive = true;
                Thread = new Thread(StartUdpListening);
                Thread.Start();

                Logger?.LogInformation("Listener started for UDP requests on {LocalEndPoint}", Socket.LocalEndPoint);
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, "Listener failed to start on UDP port {ServicePort}", servicePort);
                return false;
            }

            return true;
        }

        /// <summary>
        ///  Listener thread
        /// </summary>
        private void StartUdpListening()
        {
            while (IsActive)
            {
                try
                {
                    OnSocket(Socket);
                }
                catch (Exception ex)
                {
                    OnClientDisconnected(Socket, ex);
                }
            }
            //_socket.Close();
        }

        #endregion Methods
    }
}
