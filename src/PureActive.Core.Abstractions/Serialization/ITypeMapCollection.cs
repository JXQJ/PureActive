// ***********************************************************************
// Assembly         : PureActive.Core.Abstractions
// Author           : SteveBu
// Created          : 11-02-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-02-2018
// ***********************************************************************
// <copyright file="ITypeMapCollection.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;

namespace PureActive.Core.Abstractions.Serialization
{
    /// <summary>
    /// Represents a set of abstract types permitted to be deserialized,
    /// along with their concrete types available to deserialize.
    /// Implements the IReadOnlyDictionary" />
    /// </summary>
    public interface ITypeMapCollection : IReadOnlyDictionary<Type, IReadOnlyDictionary<string, Type>>
    {
    }
}