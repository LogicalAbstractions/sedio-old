using System;
using Newtonsoft.Json;

namespace Sedio.Core.Converters
{
    public abstract class AbstractStringJsonConverter<T> : JsonConverter<T>
    {
        public override T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (typeof(T).IsClass && reader.TokenType == JsonToken.Null)
            {
                return default(T);
            }

            if (reader.TokenType == JsonToken.String)
            {
                if (OnFromString((string)reader.Value, out var result))
                {
                    return result;
                }

                throw new JsonSerializationException($"Unable to parse ${typeof(T).Name}, syntax error: ${reader.Value}");
            }

            throw new JsonSerializationException($"Unable to parse ${typeof(T).Name}, wrong token type: ${reader.TokenType}");
        }

        public override void WriteJson(JsonWriter writer, T value, JsonSerializer serializer)
        {
            if (typeof(T).IsClass)
            {
                if (value == null)
                {
                    writer.WriteNull();
                    return;
                }
            }

            writer.WriteValue(OnToString(value));
        }

        protected abstract bool OnFromString(string value, out T result);

        protected virtual string OnToString(T value)
        {
            return value.ToString();
        }
    }
}