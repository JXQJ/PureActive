using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.Extensions.Logging;
using PureActive.Logging.Abstractions.Interfaces;

namespace PureActive.Network.Services.Sockets
{
    /// <summary>
    /// A class that listen for remote clients.
    /// </summary>
    public abstract class SocketListener : IDisposable
    {
        #region Private Properties

        internal Socket Socket;
        internal Thread Thread;

        private IPAddress _interfaceAddress = IPAddress.Any;
        private byte[] sendBuffer = new byte[1460];
        private int _receiveTimeout = -1;
        private int _sendTimeout = -1;
        private int _listenBacklog = 10;
        private int _bufferSize = 65535;
        private bool _isActive;

        /// <summary>
        ///     The logger.
        /// </summary>
        protected readonly IPureLogger Logger;

        #endregion Private Properties

        #region Public Properties

        /// <summary>
        ///     Port that the server is listening on.
        /// </summary>
        /// <remarks>
        ///     You can use port <c>0</c> to let the OS assign a port. This method will then give you the
        ///     assigned port.
        /// </remarks>
        public int ActivePort
        {
            get
            {
                if (Socket == null)
                    return -1;

                return ((IPEndPoint)Socket.LocalEndPoint).Port;
            }
        }

        /// <summary>
        ///   Gets or sets the ip address for receiving data
        /// </summary>
        public IPAddress InterfaceAddress
        {
            get { return _interfaceAddress; }
            set { _interfaceAddress = value; }
        }

        /// <summary>
        ///   Gets or sets the timeout for receiving data.
        /// </summary>
        public int ReceiveTimeout
        {
            get { return _receiveTimeout; }
            set { _receiveTimeout = value; }
        }

        /// <summary>
        ///   Gets or sets the timeout for sending data.
        /// </summary>
        public int SendTimeout
        {
            get { return _sendTimeout; }
            set { _sendTimeout = value; }
        }

        /// <summary>
        ///   Gets or sets the socket listener backlog.
        /// </summary>
        public int ListenBacklog
        {
            get { return _listenBacklog; }
            set { _listenBacklog = value; }
        }

        /// <summary>
        ///   Gets or sets the socket listener buffer size.
        /// </summary>
        public int BufferSize
        {
            get { return _bufferSize; }
            set { _bufferSize = value; }
        }

        /// <summary>
        ///   Gets or sets the socket listener active status.
        /// </summary>
        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        #endregion Public Properties

        #region Constructors / Deconstructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketListener"/> class.
        /// </summary>
        public SocketListener(IPureLogger<SocketListener> logger)
        {
            Logger = logger;
        }

        /// <summary>
        /// Handles object cleanup for GC finalization.
        /// </summary>
        ~SocketListener()
        {
            Dispose(false);
        }

        /// <summary>
        /// Handles object cleanup.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Handles object cleanup
        /// </summary>
        /// <param name="disposing">True if called from Dispose(); false if called from GC finalization.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (IsActive)
                    Stop();
            }

            Thread = null;
        }

        #endregion  Constructors / Deconstructors

        #region Methods

        /// <summary>
        ///  Stops the service listener if in started state.
        /// </summary>
        public bool Stop()
        {
            try
            {
                if (!IsActive)
                    throw new InvalidOperationException("SocketListener is not active and must be started before stopping");
                
                IsActive = false;
                
                Logger?.LogInformation("SocketListener stopped listening for requests on {LocalEndPoint}", Socket.LocalEndPoint);
                
                return true;
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, "SocketLister stop failed");
            }        
            return false;
        }

        /// <summary>
        /// Reads socket and processes packet
        /// </summary>
        /// <param name="socket">The active socket.</param>
        protected virtual void OnSocket(Socket socket)
        {
            try
            {
                if (socket.Poll(-1, SelectMode.SelectRead))
                {
                    var args = new ClientConnectedEventArgs(socket, Logger, _bufferSize);

                    EndPoint remoteEndPoint = new  IPEndPoint(0, 0);

                    if (socket.Available == 0)
                        return;

                    args.ChannelBuffer.BytesTransferred = socket.ReceiveFrom(args.ChannelBuffer.Buffer, SocketFlags.None, ref remoteEndPoint);
                    args.Channel.RemoteEndpoint = remoteEndPoint;

                    if (args.ChannelBuffer.BytesTransferred > 0)
                    {
                        OnClientConnected(args);

                        if (args.AllowConnect == false)
                        {
                            if (args.Response != null)
                            {
                                int sentBytes;
                                while ((sentBytes = args.Response.Read(sendBuffer, 0, sendBuffer.Length)) > 0)
                                {
                                    socket.Send(sendBuffer, sentBytes, SocketFlags.None);
                                }
                            }

                           Logger?.LogDebug("PACKET request on {LocalEndPoint} from {RemoteEndPoint} with channel id {ChannelId} was denied access to connect.", 
                               socket.LocalEndPoint,
                               args.Channel.RemoteEndpoint,
                               args.Channel.ChannelId);
                        }
                    }
                }
                else
                {
                    Thread.Sleep(10);
                }
            }
            catch (SocketException ex)
            {
                if (ex.ErrorCode == (int)SocketError.ConnectionReset)
                    return;
            }
            catch (Exception ex)
            {
                HandleDisconnect(SocketError.SocketError, ex);
            }
        }

        /// <summary>
        ///     A client has connected (nothing have been sent or received yet)
        /// </summary>
        /// <returns></returns>
        protected virtual void OnClientConnected(ClientConnectedEventArgs args)
        {
            ClientConnected(this, args);
        }

        /// <summary>
        ///     A client has disconnected
        /// </summary>
        /// <param name="socket">Channel representing the client that disconnected</param>
        /// <param name="exception">
        ///     Exception which was used to detect disconnect (<c>SocketException</c> with status
        ///     <c>Success</c> is created for graceful disconnects)
        /// </param>
        protected virtual void OnClientDisconnected(Socket socket, Exception exception)
        {
            ClientDisconnected(this, new ClientDisconnectedEventArgs(socket, exception));
        }

        /// <summary>
        /// Detected a disconnect
        /// </summary>
        /// <param name="socketError">ProtocolNotSupported = decoder failure.</param>
        /// <param name="exception">Why socket got disconnected</param>
        protected void HandleDisconnect(SocketError socketError, Exception exception)
        {
            try
            {
                Socket.Close();
            }
            catch (Exception ex)
            {
                HandleDisconnect(SocketError.ConnectionReset, ex);
            }
        }

        #endregion Methods

        #region Events

        /// <summary>
        ///     A client has connected (nothing have been sent or received yet)
        /// </summary>
        public event ClientConnectedEventHandler ClientConnected = delegate { };

        /// <summary>
        ///     A client has connected (nothing have been sent or received yet)
        /// </summary>
        public delegate void ClientConnectedEventHandler(object sender, ClientConnectedEventArgs args);

        /// <summary>
        ///     A client has disconnected
        /// </summary>
        public event ClientDisconnectedEventHandler ClientDisconnected = delegate { };

        /// <summary>
        ///     A client has disconnected
        /// </summary>
        public delegate void ClientDisconnectedEventHandler(object sender, ClientDisconnectedEventArgs args);

        /// <summary>
        ///     An internal error occured
        /// </summary>
        public event ErrorHandler ListenerError = delegate { };

        #endregion Events

    }
}
