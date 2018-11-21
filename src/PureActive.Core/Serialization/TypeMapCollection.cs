// ***********************************************************************
// Assembly         : PureActive.Core
// Author           : SteveBu
// Created          : 11-02-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-02-2018
// ***********************************************************************
// <copyright file="TypeMapCollection.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using PureActive.Core.Abstractions.Serialization;

namespace PureActive.Core.Serialization
{
    /// <summary>
    /// A collection of type maps for the JSON serializer.
    /// Implements the <see cref="PureActive.Core.Abstractions.Serialization.ITypeMapCollection" />
    /// </summary>
    /// <seealso cref="PureActive.Core.Abstractions.Serialization.ITypeMapCollection" />
    public class TypeMapCollection : Dictionary<Type, IReadOnlyDictionary<string, Type>>, ITypeMapCollection
    {
    }
}