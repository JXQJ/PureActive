// ***********************************************************************
// Assembly         : PureActive.Core.Abstractions
// Author           : SteveBu
// Created          : 11-02-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-02-2018
// ***********************************************************************
// <copyright file="IJsonSerializer.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Newtonsoft.Json.Linq;

namespace PureActive.Core.Abstractions.Serialization
{
    /// <summary>
    /// Serializes and deserializes objects using Json.
    /// </summary>
    public interface IJsonSerializer
    {
        /// <summary>
        /// Serializes a Json object.
        /// </summary>
        /// <typeparam name="TObject">The type of the t object.</typeparam>
        /// <param name="obj">The object.</param>
        /// <returns>System.String.</returns>
        string Serialize<TObject>(TObject obj);

        /// <summary>
        /// Serializes a Json object.
        /// </summary>
        /// <typeparam name="TObject">The type of the t object.</typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="writeTypesForAllSubclasses">if set to <c>true</c> [write types for all subclasses].</param>
        /// <returns>System.String.</returns>
        string Serialize<TObject>(TObject obj, bool writeTypesForAllSubclasses);

        /// <summary>
        /// Serializes an object to a JObject.
        /// </summary>
        /// <typeparam name="TObject">The type of the t object.</typeparam>
        /// <param name="obj">The object.</param>
        /// <returns>JObject.</returns>
        JObject SerializeToJObject<TObject>(TObject obj);

        /// <summary>
        /// Deserializes a Json object.
        /// </summary>
        /// <typeparam name="TObject">The type of the t object.</typeparam>
        /// <param name="str">The string.</param>
        /// <returns>TObject.</returns>
        TObject Deserialize<TObject>(string str);
    }
}