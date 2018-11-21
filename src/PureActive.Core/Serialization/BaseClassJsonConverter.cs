// ***********************************************************************
// Assembly         : PureActive.Core
// Author           : SteveBu
// Created          : 11-02-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-02-2018
// ***********************************************************************
// <copyright file="BaseClassJsonConverter.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PureActive.Core.Serialization
{
    /// <summary>
    /// A converter that reads and writes a custom type.
    /// Implements the <see cref="Newtonsoft.Json.JsonConverter" />
    /// </summary>
    /// <seealso cref="Newtonsoft.Json.JsonConverter" />
    public class BaseClassJsonConverter : JsonConverter
    {
        /// <summary>
        /// The type property name.
        /// </summary>
        private const string TypePropertyName = "type";

        /// <summary>
        /// Whether or not to bypass this converter on the next read or write.
        /// This is an unfortunate hack that is necessary to work around the
        /// infinite recursion caused by JSON.NET when attempting to convert
        /// to and from JObjects with the same converter.
        /// </summary>
        [ThreadStatic] private static bool _bypassOnNextOperation;

        /// <summary>
        /// The base class type.
        /// </summary>
        private readonly Type _baseClassType;

        /// <summary>
        /// The mapping from type value to object type.
        /// </summary>
        private readonly IReadOnlyDictionary<string, Type> _typeMap;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="baseClassType">The base class type.</param>
        /// <param name="typeMap">The map from type string to subclass type.</param>
        public BaseClassJsonConverter(Type baseClassType, IReadOnlyDictionary<string, Type> typeMap)
        {
            _baseClassType = baseClassType;
            _typeMap = typeMap;
        }

        /// <summary>
        /// Returns whether or not this converter should be used for reading.
        /// </summary>
        /// <value><c>true</c> if this <see cref="T:Newtonsoft.Json.JsonConverter" /> can read JSON; otherwise, <c>false</c>.</value>
        public override bool CanRead
        {
            get
            {
                if (_bypassOnNextOperation)
                {
                    _bypassOnNextOperation = false;
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Returns whether or not this converter should be used for writing.
        /// </summary>
        /// <value><c>true</c> if this <see cref="T:Newtonsoft.Json.JsonConverter" /> can write JSON; otherwise, <c>false</c>.</value>
        public override bool CanWrite
        {
            get
            {
                if (_bypassOnNextOperation)
                {
                    _bypassOnNextOperation = false;
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Returns whether or not this converter can be used for the given type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns><c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.</returns>
        public override bool CanConvert(Type objectType)
        {
            return _baseClassType == objectType
                   || objectType.GetTypeInfo().IsSubclassOf(_baseClassType);
        }

        /// <summary>
        /// Reads the json value.
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        /// <exception cref="InvalidOperationException">
        /// Object does not have required type property.
        /// or
        /// Object has unknown type.
        /// </exception>
        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            var obj = JObject.Load(reader);
            var type = (string) obj[TypePropertyName];

            if (string.IsNullOrEmpty(type))
                throw new InvalidOperationException("Object does not have required type property.");

            if (!_typeMap.ContainsKey(type))
                throw new InvalidOperationException("Object has unknown type.");

            try
            {
                _bypassOnNextOperation = true;
                return obj.ToObject(_typeMap[type], serializer);
            }
            finally
            {
                _bypassOnNextOperation = false;
            }
        }

        /// <summary>
        /// Writes the json value.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The serializer.</param>
        /// <exception cref="InvalidOperationException">The object type is not registered in the type map.</exception>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JObject obj;

            try
            {
                _bypassOnNextOperation = true;
                obj = JObject.FromObject(value, serializer);
            }
            finally
            {
                _bypassOnNextOperation = false;
            }

            var type = _typeMap
                .Where(kvp => kvp.Value == value.GetType())
                .Select(kvp => kvp.Key)
                .FirstOrDefault();

            if (type == null)
                throw new InvalidOperationException("The object type is not registered in the type map.");

            obj.AddFirst(new JProperty(TypePropertyName, type));

            obj.WriteTo(writer);
        }
    }
}