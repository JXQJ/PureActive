using System;
using System.Net.NetworkInformation;
using System.Threading;

namespace PureActive.Network.Abstractions.PingService.Events
{
    public class PingReplyEventArgs : EventArgs
    {
        public PingJob PingJob { get; }
        public PingReply PingReply { get; }
        public CancellationToken CancellationToken { get; }

    public PingReplyEventArgs(PingJob pingJob, PingReply pingReply, CancellationToken cancellationToken)
    {
            PingJob = pingJob;
            PingReply = pingReply ?? throw new ArgumentNullException(nameof(pingReply));
            CancellationToken = cancellationToken;
        }
    }
}
