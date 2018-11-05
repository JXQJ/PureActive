
namespace PureActive.Network.Abstractions.DhcpService.Types
{
    public enum DhcpMessageProcessed
    {
        Unknown = 0,
        Success,
        Duplicate,
        Failed,
        Ignored
        
    }

    public static class DhcpMessageProcessedString
    {
        public static string GetName(DhcpMessageProcessed dhcpMessageProcessed)
        {
            switch (dhcpMessageProcessed)
            {
                case DhcpMessageProcessed.Success:
                    return "SUCCESS";
                case DhcpMessageProcessed.Duplicate:
                    return "DUPLICATE";
                case DhcpMessageProcessed.Failed:
                    return "FAILED";
                case DhcpMessageProcessed.Ignored:
                    return "IGNORED";
                default:
                    return "UNKNOWN";
            }
        }
    }
}
