namespace PureActive.Network.Abstractions.DhcpService.Types
{
    /// <summary>
    /// Contains global variables for project.
    /// </summary>
    public static class DhcpConstants
    {
        public static int DhcpServicePort { get { return 67; } }
        public static int DhcpClientPort { get { return 68; } }
        public static int DhcpMinMessageSize { get { return 236; } }
        public static int DhcpMaxMessageSize { get { return 1024; } }
        public static int DhcpReceiveTimeout { get { return -1; } }
        public static int DhcpSendTimeout { get { return -1; } }
        public static uint DhcpOptionsMagicNumber { get { return 1669485411; } }
        public static uint DhcpWinOptionsMagicNumber { get { return 1666417251; } }
    }
}
