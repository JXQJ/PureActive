// ***********************************************************************
// Assembly         : PureActive.Core
// Author           : SteveBu
// Created          : 10-31-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-01-2018
// ***********************************************************************
// <copyright file="TimeProvider.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using PureActive.Core.Abstractions.System;

namespace PureActive.Core.System
{
    /// <summary>
    /// TimeProvider implementation that returns the current time,
    /// as reported by the system.
    /// Implements the <see cref="ITimeProvider" />
    /// </summary>
    /// <seealso cref="ITimeProvider" />
    public class TimeProvider : ITimeProvider
    {
        /// <summary>
        /// The current time, in UTC.
        /// </summary>
        /// <value>The UTC now.</value>
        public DateTime UtcNow => DateTime.UtcNow;
    }
}