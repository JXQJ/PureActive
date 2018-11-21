// ***********************************************************************
// Assembly         : PureActive.Hosting
// Author           : SteveBu
// Created          : 11-02-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-02-2018
// ***********************************************************************
// <copyright file="WebAppHost.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace PureActive.Hosting.Settings
{
    /// <summary>
    /// The host for the site.
    /// (This is a separate type to enable dependency injection.)
    /// </summary>
    public class WebAppHost
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="hostName">Name of the host.</param>
        public WebAppHost(string hostName)
        {
            HostName = hostName;
        }

        /// <summary>
        /// The host name of the service from an external source.
        /// </summary>
        /// <value>The name of the host.</value>
        public string HostName { get; }
    }
}