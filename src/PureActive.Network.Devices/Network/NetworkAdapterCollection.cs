using System.Collections;
using System.Collections.Generic;
using PureActive.Network.Abstractions.Network;

namespace PureActive.Network.Devices.Network
{
    public class NetworkAdapterCollection : INetworkAdapterCollection
    {
        private readonly List<INetworkAdapter> _networkAdapters = new List<INetworkAdapter>();
  
        public int Count => _networkAdapters?.Count ?? 0;

        public INetworkAdapter this[int index]
        {
            get => _networkAdapters[index];
            set => _networkAdapters.Insert(index, value);
        }

        public bool Add(INetworkAdapter networkAdapter)
        {
            if (_networkAdapters.Contains(networkAdapter))
                return false;

            _networkAdapters.Add(networkAdapter);

            return true;
        }

        public bool Remove(int index)
        {
            if (index < 0 && index >= _networkAdapters.Count)
                return false;

            _networkAdapters.RemoveAt(index);

            return true;
        }

        public bool Remove(INetworkAdapter networkAdapter)
        {
            return _networkAdapters.Remove(networkAdapter);
        }

        public IEnumerator<INetworkAdapter> GetEnumerator()
        {
            return _networkAdapters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
