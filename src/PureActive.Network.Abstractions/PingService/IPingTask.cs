using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using PureActive.Network.Abstractions.PingService.Events;
using PureActive.Network.Abstractions.Types;

namespace PureActive.Network.Abstractions.PingService
{
    public interface IPingTask
    {
        int Timeout { get; set; }
        int DefaultTimeout { get; }

        int WaitBetweenPings { get; set; }
        int Ttl { get; set; }
        bool DoNotFragment { get; set; }

        Task<PingReply> PingIpAddressAsync(IPAddress ipAddress, int timeout, byte[] buffer, PingOptions pingOptions);
        Task<PingReply> PingIpAddressAsync(IPAddress ipAddress, int timeout);
        Task<PingReply> PingIpAddressAsync(IPAddress ipAddress);

        Task PingNetworkAsync(IPAddressSubnet ipAddressSubnet, CancellationToken cancellationToken, int timeout,
            PingOptions pingOptions, int pingCallLimit, bool shuffle);

        Task PingNetworkAsync(IPAddressSubnet ipAddressSubnet, CancellationToken cancellationToken, int timeout,
            int pingCallLimit, bool shuffle);

        event PingReplyEventHandler OnPingReply;
    }
}