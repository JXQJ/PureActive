using System.Collections.Generic;

namespace PureActive.Network.Abstractions.Network
{
    public interface INetworkAdapterCollection :IEnumerable<INetworkAdapter>
    {
        bool Add(INetworkAdapter networkAdapter);
        bool Remove(int index);
        bool Remove(INetworkAdapter networkAdapter);
        int Count { get; }
    }
}
