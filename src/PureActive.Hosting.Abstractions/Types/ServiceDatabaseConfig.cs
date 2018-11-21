// ***********************************************************************
// Assembly         : PureActive.Hosting.Abstractions
// Author           : SteveBu
// Created          : 11-02-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="ServiceDatabaseConfig.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace PureActive.Hosting.Abstractions.Types
{
    /// <summary>
    /// Service database configuration hosted on localhost or Docker container.
    /// </summary>
    public enum ServiceDatabaseConfig
    {
        /// <summary>
        /// Database service hosted on localhost
        /// </summary>
        LocalHost,
        /// <summary>
        /// Database service hosted in a Docker container
        /// </summary>
        Docker
    }
}