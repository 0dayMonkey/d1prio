using System;
using System.Collections.Generic;

namespace RabbitMq.Messaging.Consumer
{
    public interface IRabbitMqConsumer : IDisposable
    {
        void AddExchangeMapping(string sourceExchangeName, IEnumerable<string> routingKeys, string exchangeName, string exchangeType, bool durable, bool autoDelete);

        void AddQueue(string queueName, string exchangeName, IEnumerable<ISubscription> subscriptions, bool durable, bool exclusive, bool autoDelete, bool autoAck, string? hashWeight = null, EchangeCreationType createExchange = EchangeCreationType.DoNotCreate);

        void Ack(MessageEnvelope envelope);

        void NAck(MessageEnvelope envelope, bool requeue);
    }
}