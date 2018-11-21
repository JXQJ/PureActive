// ***********************************************************************
// Assembly         : PureActive.Network.Extensions
// Author           : SteveBu
// Created          : 11-02-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="INetworkEnumerable.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;
using System.Net;

namespace PureActive.Network.Extensions.Enumeration
{
    /// <summary>
    /// A client to manage jobs in a job queue.
    /// Implements the <see cref="System.Collections.Generic.IEnumerable{IPAddress}" />
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEnumerable{IPAddress}" />
    public interface INetworkEnumerable : IEnumerable<IPAddress>
    {
    }
}