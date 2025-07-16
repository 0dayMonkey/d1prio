using System.Text.Json;
using System.Text.Json.Serialization;

namespace Framework.Messages
{
    public static class MessagesJsonSerializationOptions
    {
        public static JsonSerializerOptions Create() 
        {
            var res = new JsonSerializerOptions { 
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            res.Converters.Add(new UtcDateTimeConverter());
            res.Converters.Add(new DateOnlyJsonConverter());
            res.Converters.Add(new JsonStringEnumConverter());

            return res;
        }
    }
}
