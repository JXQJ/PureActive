// ***********************************************************************
// Assembly         : PureActive.Core.Abstractions
// Author           : SteveBu
// Created          : 11-02-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-02-2018
// ***********************************************************************
// <copyright file="IModelErrorCollection.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace PureActive.Core.Abstractions.Utilities
{
    /// <summary>
    /// A collection of model errors.
    /// </summary>
    public interface IModelErrorCollection
    {
        /// <summary>
        /// Returns whether or not there are errors.
        /// </summary>
        /// <value><c>true</c> if this instance has errors; otherwise, <c>false</c>.</value>
        bool HasErrors { get; }

        /// <summary>
        /// Adds a model error to the collection.
        /// </summary>
        /// <param name="propertyName">The property name that the error pertains to (if any).</param>
        /// <param name="errorText">The error text.</param>
        void AddError(string propertyName, string errorText);
    }
}