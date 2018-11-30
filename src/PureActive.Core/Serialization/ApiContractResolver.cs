// ***********************************************************************
// Assembly         : PureActive.Core
// Author           : SteveBu
// Created          : 11-02-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-02-2018
// ***********************************************************************
// <copyright file="ApiContractResolver.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using PureActive.Core.Abstractions.Serialization;

namespace PureActive.Core.Serialization
{
    /// <summary>
    /// Serializes and deserializes json objects for APIs.
    /// Implements the <see cref="DefaultContractResolver" />
    /// </summary>
    /// <seealso cref="DefaultContractResolver" />
    public class ApiContractResolver : DefaultContractResolver
    {
        /// <summary>
        /// A list of type maps, keyed by base type. Each entry in a
        /// type map consists of a string representation of a sub type,
        /// along with the actual subclass type.
        /// </summary>
        private readonly ITypeMapCollection _typeMaps;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="typeMaps">A list of type maps, keyed by base type.
        /// Each entry in a typemap consists of a string representation of
        /// a sub type, along with the actual subclass type.</param>
        public ApiContractResolver(ITypeMapCollection typeMaps)
        {
            NamingStrategy = new CamelCaseNamingStrategy();
            _typeMaps = typeMaps ?? new TypeMapCollection();
        }

        /// <summary>
        /// Returns a custom json converter for abstract classes.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>The contract's default <see cref="T:Newtonsoft.Json.JsonConverter" />.</returns>
        protected override JsonConverter ResolveContractConverter(Type objectType)
        {
            var topLevelBaseType = GetInheritanceHierarchy(objectType).First();

            _typeMaps.TryGetValue(topLevelBaseType, out var typeMap);

            if (typeMap == null)
                return base.ResolveContractConverter(objectType);

            return new BaseClassJsonConverter(topLevelBaseType, typeMap);
        }

        /// <summary>
        /// Returns a list of json Properties, where
        /// * Base class properties appear before derived class properties
        /// * Enum values are serialized as strings
        /// </summary>
        /// <param name="type">The type to create properties for.</param>
        /// <param name="memberSerialization">The member serialization mode for the type.</param>
        /// <returns>Properties for the given <see cref="T:Newtonsoft.Json.Serialization.JsonContract" />.</returns>
        protected override IList<JsonProperty> CreateProperties(
            Type type,
            MemberSerialization memberSerialization)
        {
            return base.CreateProperties(type, memberSerialization)
                ?.OrderBy(p => GetInheritanceHierarchy(p.DeclaringType).Count)
                .Select(CamelCaseEnumValues)
                .ToList();
        }

        /// <summary>
        /// Ensures that enum values are serialized as camel-cased strings.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>JsonProperty.</returns>
        private static JsonProperty CamelCaseEnumValues(JsonProperty property)
        {
            if (property.PropertyType.GetTypeInfo().IsEnum)
                property.Converter = new StringEnumConverter(new CamelCaseNamingStrategy());

            return property;
        }

        /// <summary>
        /// Returns the inheritance hierarchy, from the top-most base class on down.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>List&lt;Type&gt;.</returns>
        private static List<Type> GetInheritanceHierarchy(Type objectType)
        {
            var hierarchy = new List<Type> {objectType};
            var baseType = objectType.GetTypeInfo().BaseType;

            while (baseType != null && baseType != typeof(object))
            {
                hierarchy.Add(baseType);
                baseType = baseType.GetTypeInfo().BaseType;
            }

            hierarchy.Reverse();

            return hierarchy;
        }
    }
}