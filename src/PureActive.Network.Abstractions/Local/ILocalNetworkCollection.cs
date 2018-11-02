using System.Collections.Generic;
using PureActive.Network.Abstractions.Network;

namespace PureActive.Network.Abstractions.Local
{
    public interface ILocalNetworkCollection : IEnumerable<INetwork>
    {
        int Count { get; }

        INetwork PrimaryNetwork { get;}

        INetwork AddAdapterToNetwork(INetworkAdapter networkAdapter);
        bool RemoveAdapterFromNetwork(INetworkAdapter networkAdapter);

        bool RemoveNetwork(INetwork network);
    }
}
