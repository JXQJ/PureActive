using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.Extensions.Logging;
using PureActive.Logging.Abstractions.Interfaces;

namespace PureActive.Network.Core.Sockets
{
    /// <summary>
    ///     A class that listen for TCP packets from remote clients.
    /// </summary>
    public class TcpListener : SocketListener
    {
        public TcpListener(IPureLoggerFactory loggerFactory) : base(loggerFactory.CreatePureLogger<SocketListener>())
        {
            if (loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));
        }

        #region Methods

        /// <summary>
        ///     Starts the service listener if it is in a stopped state.
        /// </summary>
        /// <param name="servicePort">The port used to listen on.</param>
        public bool Start(int servicePort)
        {
            if (servicePort > IPEndPoint.MaxPort || servicePort < IPEndPoint.MinPort)
                throw new ArgumentOutOfRangeException(nameof(servicePort),
                    "Port must be less then " + IPEndPoint.MaxPort + " and more then " + IPEndPoint.MinPort);

            if (IsActive)
                throw new InvalidOperationException(
                    "Tcp listener is already active and must be stopped before starting");

            if (InterfaceAddress == null)
            {
                var ex = new InvalidOperationException("Unable to set interface address");
                Logger?.LogDebug(ex, "Unable to set interface address for TCP {Port}", servicePort);
                throw ex;
            }

            try
            {
                Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
                Socket.Bind(new IPEndPoint(InterfaceAddress, servicePort));
                Socket.ReceiveTimeout = ReceiveTimeout;
                Socket.SendTimeout = SendTimeout;
                Socket.Listen(ListenBacklog);

                IsActive = true;
                Thread = new Thread(StartTcpListening);
                Thread.Start();

                Logger?.LogInformation("Listener started for TCP requests on {LocalEndPoint}", Socket.LocalEndPoint);
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, "Listener failed to to start on TCP {ServicePort}", servicePort);
                return false;
            }

            return true;
        }

        /// <summary>
        ///     Listener thread
        /// </summary>
        private void StartTcpListening()
        {
            while (IsActive)
            {
                using (Socket tcpSocket = Socket.Accept())
                {
                    try
                    {
                        OnSocket(tcpSocket);
                    }
                    catch (Exception ex)
                    {
                        OnClientDisconnected(tcpSocket, ex);
                    }
                }
            }

            Socket.Close();
        }

        #endregion Methods
    }
}