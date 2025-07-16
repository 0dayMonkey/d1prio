using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ApiTools.Converters
{
    public class DateOnlyConverter : JsonConverter<DateTime>
    {
        private readonly string serializationFormat;

        public DateOnlyConverter() : this(null)
        {
        }

        public DateOnlyConverter(string? serializationFormat)
        {
            this.serializationFormat = serializationFormat ?? "yyyy-MM-dd";
        }

        public override DateTime Read(ref Utf8JsonReader reader,
                                Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return DateTime.Parse(value!);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value,
                                            JsonSerializerOptions options)
            => writer.WriteStringValue(value.ToString(serializationFormat));
    }
}
