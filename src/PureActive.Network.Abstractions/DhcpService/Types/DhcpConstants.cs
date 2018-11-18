namespace PureActive.Network.Abstractions.DhcpService.Types
{
    /// <summary>
    /// Contains global variables for project.
    /// </summary>
    public static class DhcpConstants
    {
        public static int DhcpServicePort => 67;
        public static int DhcpClientPort => 68;
        public static int DhcpMinMessageSize => 236;
        public static int DhcpMaxMessageSize => 1024;
        public static int DhcpReceiveTimeout => -1;
        public static int DhcpSendTimeout => -1;
        public static uint DhcpOptionsMagicNumber => 1669485411;
        public static uint DhcpWinOptionsMagicNumber => 1666417251;
    }
}
