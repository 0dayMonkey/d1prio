using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ApiTools.Serialization
{
    public class EnumModeConverter<T> : JsonConverter<T> where T : struct
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return Enum.Parse<T>(reader.GetString()!, true);
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
