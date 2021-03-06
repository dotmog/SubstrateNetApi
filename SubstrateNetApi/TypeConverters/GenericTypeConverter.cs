﻿using System;
using Newtonsoft.Json;
using SubstrateNetApi.Model.Types;

namespace SubstrateNetApi.TypeConverters
{
    public class GenericTypeConverter<T> : JsonConverter<T>, ITypeConverter where T : IType, new()
    {
        public string TypeName { get; } = new T().Name();

        public object Create(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;

            var baseType = new T();
            baseType.Create(value);
            return baseType;
        }

        public override T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var baseType = new T();
            baseType.CreateFromJson((string) reader.Value);
            return baseType;
        }

        public override void WriteJson(JsonWriter writer, T value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }
}