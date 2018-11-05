
namespace PureActive.Network.Abstractions.DhcpService.Types
{
    public enum DhcpSessionState : int
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
