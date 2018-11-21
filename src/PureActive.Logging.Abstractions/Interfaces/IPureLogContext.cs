// ***********************************************************************
// Assembly         : PureActive.Logging.Abstractions
// Author           : SteveBu
// Created          : 11-05-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-05-2018
// ***********************************************************************
// <copyright file="IPureLogContext.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;

namespace PureActive.Logging.Abstractions.Interfaces
{
    /// <summary>
    /// Allows for adding additional properties to all log messages
    /// logged within newly created scopes.
    /// </summary>
    public interface IPureLogContext
    {
        /// <summary>
        /// Creates a new log scope, with the given properties.
        /// </summary>
        /// <param name="properties">The properties.</param>
        /// <returns>IDisposable.</returns>
        IDisposable CreateLogScope(IList<KeyValuePair<string, string>> properties);
    }
}