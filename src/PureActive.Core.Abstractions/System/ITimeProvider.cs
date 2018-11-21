// ***********************************************************************
// Assembly         : PureActive.Core.Abstractions
// Author           : SteveBu
// Created          : 10-31-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-01-2018
// ***********************************************************************
// <copyright file="ITimeProvider.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace PureActive.Core.Abstractions.System
{
    /// <summary>
    /// Provides the current time.
    /// </summary>
    public interface ITimeProvider
    {
        /// <summary>
        /// The current date/time in UTC.
        /// </summary>
        /// <value>The UTC now.</value>
        DateTime UtcNow { get; }
    }
}