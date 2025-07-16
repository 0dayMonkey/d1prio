using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace RabbitMq.Messaging.Consumer
{

    public class RabbitMqConsumer : RabbitMqClientCommunication, IRabbitMqConsumer
    {
        private readonly ushort _prefetch;

        private List<string> _autoDeleteQueueNames = new List<string>();
        private List<string>? _processedMessages = new List<string>();

        private const int _maxProcessedMessagesCount = 100000;

        private ILogger _unprocessedMessagesLogger;

        public RabbitMqConsumer(RabbitMqConfiguration conf, ushort prefetch, bool useDeduplication, ILoggerFactory loggerFactory)
            : base(conf, loggerFactory.CreateLogger<RabbitMqConsumer>())
        {
            _unprocessedMessagesLogger = loggerFactory.CreateLogger("RabbitMq.Messaging.Consumer.Unprocessed");
            _prefetch = prefetch;
            CreateConnectionAsync(conf);

            if (useDeduplication)
            {
                _processedMessages = new List<string>();
            }
        }

        int alreadyProcessed = 0;

        public void AddExchangeMapping(string sourceExchangeName, IEnumerable<string> routingKeys, string exchangeName, string exchangeType, bool durable, bool autoDelete)
        {
            if (Channel != null)
            {
                var arguments = exchangeType.Equals(ExtendedExchangeType.ConsistentHash)
                    ? new Dictionary<string, object> { { "hash-header", "hash-on" } }
                    : null;

                Channel.ExchangeDeclare(exchangeName, exchangeType, durable, autoDelete, arguments);
                foreach (var routingKey in routingKeys)
                {
                    Channel.ExchangeBind(exchangeName, sourceExchangeName, routingKey);
                }
            }
        }

        public void AddQueue(string queueName, string exchangeName, IEnumerable<ISubscription> subscriptions, bool durable, bool exclusive, bool autoDelete, bool autoAck, string? hashWeight = null, EchangeCreationType createExchange = EchangeCreationType.DoNotCreate)
        {
            if (Channel != null)
            {
                if (createExchange != EchangeCreationType.DoNotCreate)
                {
                    try
                    {
                        Channel.ExchangeDeclare(exchangeName, ExchangeType.Topic, createExchange == EchangeCreationType.CreateDurable, autoDelete);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex, "Cannot declare exchange");
                    }
                }
                var q = Channel.QueueDeclare(queueName, durable, exclusive, autoDelete);

                if (autoDelete)
                {
                    _autoDeleteQueueNames.Add(queueName);
                }

                List<ISubscription> queueSubscriptions = new List<ISubscription>(subscriptions);

                foreach (var key in queueSubscriptions.Select(s => s.RoutingKey).Distinct())
                {
                    Channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: hashWeight ?? key);
                }
                var consumer = new EventingBasicConsumer(Channel);
                consumer.Received += (model, ea) =>
                {
                    try
                    {
                        var content = Encoding.GetEncoding(ea.BasicProperties.ContentEncoding ?? "UTF-8").GetString(ea.Body.ToArray());
                        Logger.LogTrace("Message {0} received on {1}: {2}", ea.RoutingKey, queueName, content);

                        if (AlreadyProcessed(ea.BasicProperties.MessageId))
                        {
                            Logger.LogTrace("Message {0} already processed. {1}", ea.BasicProperties.MessageId, ++alreadyProcessed);
                            Channel.BasicAck(ea.DeliveryTag, false);
                            return;
                        }

                        object? siteOriginId;

                        var messageEnvelope = new MessageEnvelope
                        {
                            MessageId = ea.BasicProperties.MessageId,
                            DeliveryTag = ea.DeliveryTag,
                            RoutingKey = ea.RoutingKey,
                            CorrelationId = ea.BasicProperties.CorrelationId,
                        };

                        if (ea.BasicProperties.Headers != null && ea.BasicProperties.Headers.TryGetValue("Site-Origin-ID", out siteOriginId) && siteOriginId != null)
                            messageEnvelope.SiteOriginId = siteOriginId.ToString();


                        foreach (var subscription in queueSubscriptions.Where(s => s.IsRoutingKeyMatching(ea.RoutingKey)))
                        {
                            var msgContent = GetContentAs(subscription.MessageType, content);
                            if (msgContent != null)
                            {
                                try
                                {
                                    subscription.Raise(this, msgContent, messageEnvelope);
                                }
                                catch (Exception e)
                                {
                                    Logger.LogError(e, "Error on calling callback for routing key '{0}': {1}", ea.RoutingKey, e.Message);
                                }
                            }
                            else
                            {
                                //Message cannot be deserialized -> NAck
                                Logger.LogError("Message NAck due to deserialization error: {0} {1}", ea.RoutingKey, content);
                                _unprocessedMessagesLogger.LogError("Unprocessed message due to deserialization error: enveloppe = {0}; payload = {1}", JsonSerializer.Serialize(messageEnvelope), content);
                                NAck(messageEnvelope, false);
                            }
                        }
                    }
                    catch(Exception e)
                    {
                        Logger.LogError(e, "Error on calling callback for routing key '{0}': {1}", ea.RoutingKey, e.Message);
                    }
                };

                Channel.BasicConsume(queue: queueName, autoAck, consumer);
            }
        }

        private bool AlreadyProcessed(string messageId)
        {
            return !string.IsNullOrEmpty(messageId) && _processedMessages != null && _processedMessages.Contains(messageId);
        }

        private void AddProcessedMessage(string? messageId)
        {
            if (!string.IsNullOrEmpty(messageId) && _processedMessages != null)
            {
                _processedMessages.Add(messageId);
                if (_processedMessages.Count > _maxProcessedMessagesCount)
                {
                    _processedMessages.RemoveRange(0, _processedMessages.Count - _maxProcessedMessagesCount);
                }
            }
        }

        private object? GetContentAs(Type type, string content)
        {
            try
            {
                return JsonSerializer.Deserialize(content, type, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Error on deserializing {0} as {1}", content, type.Name);
                return null;
            }
        }

        protected override void ChannelRegistration(IModel channel)
        {
            channel.BasicQos(0, _prefetch, false);
        }

        public void Ack(MessageEnvelope envelope)
        {
            AddProcessedMessage(envelope.MessageId);
            if (Channel != null)
            {
                Channel.BasicAck(envelope.DeliveryTag, false);
            }
        }

        public void NAck(MessageEnvelope envelope, bool requeue)
        {
            if (!requeue)
            {
                AddProcessedMessage(envelope.MessageId);
            }
            if (Channel != null)
            {
                Channel.BasicNack(envelope.DeliveryTag, false, requeue);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    foreach (var queueName in _autoDeleteQueueNames)
                        Channel.QueueDelete(queueName);
                }
                _disposedValue = true;
            }
            base.Dispose(disposing);
        }

    }
}
