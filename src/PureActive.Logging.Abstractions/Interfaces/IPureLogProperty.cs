using System.Collections.Generic;

namespace PureActive.Logging.Abstractions.Interfaces
{
    public interface IPureLogProperty
    {
        KeyValuePair<string, object> KeyValuePair { get; }

        string Key { get; }
        object Value { get; }

        bool DestructureObject { get; }
    }
}