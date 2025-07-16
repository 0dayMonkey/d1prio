using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NotificationService.Domain.Messages
{
    public class NotificationMessage
    {
        public string Subject { get; set; } = string.Empty;
        public string Template { get; set; } = string.Empty;
        public KeyValue[] KeyValues { get; set; } = Array.Empty<KeyValue>();
        public Severity Severity { get; set; } = Severity.Unset;
        [JsonPropertyName("Timestamp")]
        public DateTime MessageTimestamp { get; set; } = new DateTime(2022, 1, 1);
        public Channels Channels { get; set; } = new Channels();
    }
}
