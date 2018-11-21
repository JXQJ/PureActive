// ***********************************************************************
// Assembly         : PureActive.Network.Core
// Author           : SteveBu
// Created          : 11-05-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="ErrorHandler.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace PureActive.Network.Core.Sockets
{
    /// <summary>
    /// <see cref="SocketChannel" /> have sent or received a message.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="exception">Channel that did the work</param>
    /// <remarks>We uses delegates instead of regular events to make sure that there are only one subscriber and that it's
    /// configured once.</remarks>
    public delegate void ErrorHandler(object sender, Exception exception);
}