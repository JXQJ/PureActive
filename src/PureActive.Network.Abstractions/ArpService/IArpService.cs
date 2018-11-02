using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using PureActive.Hosting.Abstractions.System;

namespace PureActive.Network.Abstractions.ArpService
{
    public interface IArpService : IHostedServiceInternal, IEnumerable<ArpItem>
    {
        TimeSpan Timeout { get; set; }

        int Count { get; }

        ArpRefreshStatus LastArpRefreshStatus { get; }
        DateTimeOffset LastUpdated { get; }

        void ClearArpCache();

        Task RefreshArpCacheAsync(CancellationToken stoppingToken, bool clearCache);
        Task RefreshArpCacheAsync(bool clearCache);

        Task<ArpItem> GetArpItemAsync(IPAddress ipAddress, CancellationToken cancellationToken);
        Task<ArpItem> GetArpItemAsync(IPAddress ipAddress);

        Task<PhysicalAddress> GetPhysicalAddressAsync(IPAddress ipAddress, CancellationToken cancellationToken);
        Task<PhysicalAddress> GetPhysicalAddressAsync(IPAddress ipAddress);

        PhysicalAddress GetPhysicalAddress(IPAddress ipAddress);

        IPAddress GetIPAddress(PhysicalAddress physicalAddress, bool refreshCache = false);
    }
}