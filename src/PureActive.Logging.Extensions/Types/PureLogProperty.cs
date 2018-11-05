using System.Collections.Generic;
using PureActive.Logging.Abstractions.Interfaces;

namespace PureActive.Logging.Extensions.Types
{
    public class PureLogProperty : IPureLogProperty
    {
        public KeyValuePair<string, object> KeyValuePair { get; }

        public string Key => KeyValuePair.Key;
        public object Value => KeyValuePair.Value;

        public bool DestructureObject { get; }

        public PureLogProperty(KeyValuePair<string, object> keyValuePair, bool destructureObjects = false)
        {
            this.KeyValuePair = keyValuePair;
            this.DestructureObject = destructureObjects;
        }

        public PureLogProperty(string key, object value, bool destructureObjects = false) : this(new KeyValuePair<string, object>(key, value), destructureObjects)
        {
        }
    }
}