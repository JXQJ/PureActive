﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PureActive.Core.Serialization
{
    /// <summary>
    ///     A converter that reads and writes a custom type.
    /// </summary>
    public class BaseClassJsonConverter : JsonConverter
    {
        /// <summary>
        ///     The type property name.
        /// </summary>
        private const string TypePropertyName = "type";

        /// <summary>
        ///     Whether or not to bypass this converter on the next read or write.
        ///     This is an unfortunate hack that is necessary to work around the
        ///     infinite recursion caused by JSON.NET when attempting to convert
        ///     to and from JObjects with the same converter.
        /// </summary>
        [ThreadStatic] private static bool _bypassOnNextOperation;

        /// <summary>
        ///     The base class type.
        /// </summary>
        private readonly Type _baseClassType;

        /// <summary>
        ///     The mapping from type value to object type.
        /// </summary>
        private readonly IReadOnlyDictionary<string, Type> _typeMap;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="baseClassType">The base class type.</param>
        /// <param name="typeMap">The map from type string to subclass type.</param>
        public BaseClassJsonConverter(Type baseClassType, IReadOnlyDictionary<string, Type> typeMap)
        {
            _baseClassType = baseClassType;
            _typeMap = typeMap;
        }

        /// <summary>
        ///     Returns whether or not this converter should be used for reading.
        /// </summary>
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
        ///     Returns whether or not this converter should be used for writing.
        /// </summary>
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
        ///     Returns whether or not this converter can be used for the given type.
        /// </summary>
        public override bool CanConvert(Type objectType)
        {
            return _baseClassType == objectType
                   || objectType.GetTypeInfo().IsSubclassOf(_baseClassType);
        }

        /// <summary>
        ///     Reads the json value.
        /// </summary>
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
        ///     Writes the json value.
        /// </summary>
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