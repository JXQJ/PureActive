using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using PureActive.Hosting.Abstractions.System;
using PureActive.Network.Abstractions.PingService.Events;
using PureActive.Network.Abstractions.Types;

namespace PureActive.Network.Abstractions.PingService
{
    public interface IPingService : IHostedServiceInternal
    {
        event PingReplyEventHandler OnPingReply;
        bool EnableLogging { get; set; }
        Task PingNetworkAsync(IPAddressSubnet ipAddressSubnet, CancellationToken cancellationToken, int timeout, int pingCallLimit, bool shuffle);

        Task<PingReply> PingIpAddressAsync(IPAddress ipAddress, int timeout);
        Task<PingReply> PingIpAddressAsync(IPAddress ipAddress);


    }
}