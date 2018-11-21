// ***********************************************************************
// Assembly         : PureActive.Network.Core
// Author           : SteveBu
// Created          : 11-05-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="DisconnectHandler.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace PureActive.Network.Core.Sockets
{
    /// <summary>
    /// <see cref="SocketChannel" /> was disconnected
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="exception">Channel which got disconnected</param>
    /// <seealso cref="SocketChannel" />
    public delegate void DisconnectHandler(object sender, Exception exception);
}