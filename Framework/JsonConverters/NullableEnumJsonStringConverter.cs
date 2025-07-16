using System.Text.Json;
using System.Text.Json.Serialization;

namespace Framework.JsonConverters
{
    public class NullableEnumJsonStringConverter<T> : JsonConverter<T?> where T : struct, Enum
    {
        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null; // Return null for null values
            }

            if (reader.TokenType == JsonTokenType.Number)
            {
                var number = reader.GetInt32();
                if (Enum.IsDefined(typeof(T), number))
                {
                    return (T)Enum.ToObject(typeof(T), number);
                }
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                string? enumValue = reader.GetString();
                if (Enum.TryParse(enumValue, true, out T result))
                {
                    return result;
                }
            }

            return null;
        }

        public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                writer.WriteStringValue(value.Value.ToString());
            }
            else
            {
                writer.WriteNullValue(); // Write null for null values
            }
        }
    }
}
