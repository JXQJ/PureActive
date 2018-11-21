// ***********************************************************************
// Assembly         : PureActive.Core.Abstractions
// Author           : SteveBu
// Created          : 11-02-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-02-2018
// ***********************************************************************
// <copyright file="IRandomNumberProvider.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace PureActive.Core.Abstractions.Utilities
{
    /// <summary>
    /// A random number generator.
    /// </summary>
    public interface IRandomNumberProvider
    {
        /// <summary>
        /// Returns a random integer.
        /// </summary>
        /// <returns>System.Int32.</returns>
        int NextInt();
    }
}