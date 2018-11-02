using System;
using System.Collections.Generic;

namespace PureActive.Core.Abstractions.Serialization
{
    /// <summary>
    ///     Represents a set of abstract types permitted to be deserialized,
    ///     along with their concrete types available to deserialize.
    /// </summary>
    public interface ITypeMapCollection : IReadOnlyDictionary<Type, IReadOnlyDictionary<string, Type>>
    {
    }
}