namespace RabbitMq.Messaging.Consumer
{
    public class MessageEnvelope
    {
        public ulong DeliveryTag { get; init; }

        public string? MessageId { get; init; }

        public string? RoutingKey { get; init; }

        public string? CorrelationId { get; init; }
        public string? SiteOriginId { get; set; }
    }
}
