using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PureActive.Core.Extensions;
using PureActive.Hosting.Abstractions.System;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Network.Abstractions.PingService;
using PureActive.Network.Abstractions.PingService.Events;
using PureActive.Network.Abstractions.Types;
using PureActive.Network.Extensions.Network;

namespace PureActive.Network.Services.PingService
{
    public partial class PingService
    {
        public class PingTaskImpl : IPingTask
        {
            private readonly ICommonServices _commonServices;
            private const string DefaultDataBuffer = "abcdefghijklmnopqrstuvwxyz012345";

            private const int WindowsDefaultTimeout = 5000; // Wait a default of 5 seconds for a reply (same as Windows Ping)

            public int DefaultTimeout => WindowsDefaultTimeout;

            private readonly Ping _ping;

            private readonly PingOptions _pingOptions;

            public event PingReplyEventHandler OnPingReply;

            private SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1,1);

            public int Timeout { get; set; } = WindowsDefaultTimeout;
            public int WaitBetweenPings { get; set; } = 0; // No wait between pings

            public int Ttl
            {
                get => _pingOptions.Ttl;
                set => _pingOptions.Ttl = value;
            }

            public bool DoNotFragment
            {
                get => _pingOptions.DontFragment;
                set => _pingOptions.DontFragment = value;
            }

            private readonly IPureLogger _logger;

            public PingTaskImpl(ICommonServices commonServices, IPureLogger<PingTaskImpl> logger = null)
            {
                _commonServices = commonServices ?? throw new ArgumentNullException(nameof(commonServices));
                _ping = new Ping();
                _pingOptions = new PingOptions(64, true);

                _logger = logger ?? commonServices.LoggerFactory.CreatePureLogger<PingTaskImpl>();
            }


            public async Task<PingReply> PingIpAddressAsync(IPAddress ipAddress, int timeout, byte[] buffer, PingOptions pingOptions)
            {
                await _semaphoreSlim.WaitAsync();
                PingReply pingReply = null;

                Task pingTask = Task.Run(async () =>
                    {
                        pingReply = await _ping.SendPingAsync(ipAddress, timeout, buffer, pingOptions);
                    });

                await pingTask.ContinueWith(t => { _semaphoreSlim.Release(); });

                return pingReply;
            }

            public async Task<PingReply> PingIpAddressAsync(IPAddress ipAddress, int timeout)
            {
                // Create a buffer of 32 ASCII bytes of data to be transmitted.
                byte[] buffer = Encoding.ASCII.GetBytes(DefaultDataBuffer);

                return await PingIpAddressAsync(ipAddress, timeout, buffer, _pingOptions);
            }

            public async Task<PingReply> PingIpAddressAsync(IPAddress ipAddress) =>
                await PingIpAddressAsync(ipAddress, WindowsDefaultTimeout);


            // NOTE: Underlying Ping Service does not support concurrent Pings
            public async Task PingNetworkAsync(IPAddressSubnet ipAddressSubnet, CancellationToken cancellationToken,
                int timeout, PingOptions pingOptions, int pingCallLimit = int.MaxValue, bool shuffle = true)
            {
                var pingJob = new PingJob(Guid.NewGuid(), 0, ipAddressSubnet, DateTimeOffset.Now);

                var networkEnumerator = new NetworkEnumerator(ipAddressSubnet).ToList();

                // Shuffle Order of Network Addresses
                if (shuffle)
                    networkEnumerator.Shuffle();

                foreach (var ipAddress in networkEnumerator.Take(pingCallLimit))
                {
                    // Return if cancellation is requested
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return;
                    }

                    pingJob.NextTask(ipAddress);

                    // Create a buffer of 32 ASCII bytes of data to be transmitted.

                    byte[] buffer = pingJob.ToBuffer();

                    var pingTask = PingIpAddressAsync(ipAddress, timeout, buffer, pingOptions);
                    var pingReply = pingTask.Result;

                    var pingReplyEventArgs = new PingReplyEventArgs(pingJob, pingReply, cancellationToken);

                    OnPingReply?.Invoke(this, pingReplyEventArgs);

                    // Return if cancellation is requested by Events
                    if (pingReplyEventArgs.CancellationToken.IsCancellationRequested)
                    {
                        return;
                    }

                    if (pingReply.Status != IPStatus.Success) continue;

                    if (WaitBetweenPings > 0)
                    {
                        await Task.Delay(WaitBetweenPings, cancellationToken);
                    }
                }
            }

            public Task PingNetworkAsync(IPAddressSubnet ipAddressSubnet, CancellationToken cancellationToken, int timeout, int pingCallLimit, bool shuffle)
            {
                    return PingNetworkAsync(ipAddressSubnet, cancellationToken, timeout, _pingOptions, pingCallLimit, shuffle);
            }
        }
    }
}
