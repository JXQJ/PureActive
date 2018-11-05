
namespace PureActive.Network.Abstractions.DhcpService.Types
{
    public enum DhcpSessionState
    {
        Init = 0,
        Discover,
        Offer,
        Request,
        Ack,
        Queued,
        Retry,
        Completed
    }
}
