using System;
using System.Collections.Generic;

namespace RabbitMq.Messaging.Publisher
{
    public class PreparedMessage
    {
        internal ulong DeliveryTag { get; set; } = 0;

        internal string MessageId { get; init; }

        internal string RoutingKey { get; init; }

        internal string Content { get; init; }

        public string CorrelationId { get; init; }

        public Dictionary<string, object>? AdditionnalHeaders { get; init; }

        private readonly object? _consistentHashing;

        public PreparedMessage(string content, string routingKey, string? messageId, object? consistentHashing, string? correlationId, Dictionary<string, object>? additionnalHeaders)
        {
            RoutingKey = routingKey;
            Content = content;
            CorrelationId = correlationId ?? string.Empty;
            MessageId = messageId ?? Guid.NewGuid().ToString();
            AdditionnalHeaders = additionnalHeaders;

            _consistentHashing = consistentHashing;
        }

        internal object GetConsistentHash()
        {
            return _consistentHashing ?? RoutingKey;
        }

        internal void ResetDeliveryTag()
        {
            DeliveryTag = 0;
        }

        internal bool IsMatching(ulong ackdeliveryTag, bool multiple)
        {
            if (DeliveryTag == 0)
            {
                return false;
            }
            return multiple ? DeliveryTag <= ackdeliveryTag : DeliveryTag == ackdeliveryTag;
        }
    }
}
