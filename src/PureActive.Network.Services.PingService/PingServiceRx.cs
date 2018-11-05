using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Reactive.Threading.Tasks;
using PureActive.Hosting.Abstractions.System;
using PureActive.Network.Abstractions.PingService;

namespace PureActive.Network.Services.PingService
{
    public class PingServiceRx
    {
        private readonly ICommonServices _commonServices;
        private readonly IPingTask _pingTask;

        public PingServiceRx(ICommonServices commonServices)
        {
            _commonServices = commonServices ?? throw new ArgumentNullException(nameof(commonServices));
            _pingTask = new PingService.PingTaskImpl(commonServices);
        }

        public IObservable<PingReply> PingRequestAsync(IPAddress ipAddress)
        {
            return _pingTask.PingIpAddressAsync(ipAddress).ToObservable();
        }
    }
}
