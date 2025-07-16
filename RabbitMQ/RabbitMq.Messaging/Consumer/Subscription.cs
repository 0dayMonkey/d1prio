using System;
using System.Text.RegularExpressions;

namespace RabbitMq.Messaging.Consumer
{
    public interface ISubscription
    {
        Type MessageType { get; }

        string RoutingKey { get; }

        bool IsRoutingKeyMatching(string routingKey);

        void Raise(IRabbitMqConsumer consumer, object message, MessageEnvelope envelope);
    }

    public class Subscription<T> : ISubscription
    {
        public Type MessageType { get; private set; }

        public string RoutingKey { get; private set; }

        public Regex RoutingKeyRegex { get; init; }

        public Action<IRabbitMqConsumer, T, MessageEnvelope> CallBack { get; init; }

        public Subscription(string routingKey, Action<IRabbitMqConsumer, T, MessageEnvelope> callback)
        {
            RoutingKey = routingKey;

            var pattern = routingKey
                .Replace("*", "([^\\.]+)")
                .Replace(".#", "(\\.[^\\.]+)*")
                .Replace("#.", "([^\\.]+\\.)*")
                .Replace("#", ".*");

            RoutingKeyRegex = new Regex(pattern);
            MessageType = typeof(T);
            CallBack = callback; // (IRabbitMqConsumer consumer, T message, ulong deliveryTag) => { handler.HandleMessage(consumer, message, deliveryTag); };
        }

        public bool IsRoutingKeyMatching(string routingKey)
        {
            return RoutingKeyRegex.IsMatch(routingKey);
        }

        public void Raise(IRabbitMqConsumer consumer, object message, MessageEnvelope envelope)
        {
            CallBack.DynamicInvoke(consumer, message, envelope);
        }

    }
}
