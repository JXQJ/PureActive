using System;
using PureActive.Logging.Abstractions.Interfaces;

namespace PureActive.Network.Core.Sockets
{
    public class HostedSocketService : SocketService
    {
        public HostedSocketService(IPureLogger<SocketService> logger) : base(logger)
        {

        }

        public override bool Start()
        {
            throw new NotImplementedException();
        }

        public override bool Stop()
        {
            throw new NotImplementedException();
        }
    }
}
