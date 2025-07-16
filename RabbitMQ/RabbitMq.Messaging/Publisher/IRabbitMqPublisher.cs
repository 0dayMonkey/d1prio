using System;
using System.Collections.Generic;

namespace RabbitMq.Messaging.Publisher
{
    public interface IRabbitMqPublisher : IDisposable
    {
        void Publish<T>(T content, string routingKey, string? messageId = null, object? consistentHashing = null, string? correlationId = null, Dictionary<string, object>? additionnalHeaders = null);

        bool PublishBatchConfirm(List<PreparedMessage> message);
    }
}