using System;
using System.Collections.Generic;
using PureActive.Core.Abstractions.Serialization;

namespace PureActive.Core.Serialization
{
    /// <summary>
    ///     A collection of type maps for the JSON serializer.
    /// </summary>
    public class TypeMapCollection : Dictionary<Type, IReadOnlyDictionary<string, Type>>, ITypeMapCollection
    {
    }
}