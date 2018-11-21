// ***********************************************************************
// Assembly         : PureActive.Logging.Abstractions
// Author           : SteveBu
// Created          : 11-03-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-03-2018
// ***********************************************************************
// <copyright file="IOperationIdProvider.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace PureActive.Logging.Abstractions.Interfaces
{
    /// <summary>
    /// Provides the operation ID of the current request.
    /// </summary>
    public interface IOperationIdProvider
    {
        /// <summary>
        /// The operation ID of the current request.
        /// </summary>
        /// <value>The operation identifier.</value>
        string OperationId { get; }
    }
}