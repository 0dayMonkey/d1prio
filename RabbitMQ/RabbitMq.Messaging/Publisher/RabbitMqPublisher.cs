using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMq.Messaging.Publisher
{

    public delegate void MessagesPublished(string[] messageIds);

    /// <summary>
    /// RabbitMq publisher class
    /// </summary>
    public class RabbitMqPublisher : RabbitMqClientCommunication, IRabbitMqPublisher
    {
        private readonly List<PreparedMessage> _messages = new List<PreparedMessage>();
        private readonly string _exchangeName;
        private readonly string _exchangeType;
        private readonly bool _durable;
        private readonly bool _autoDelete;
        private readonly bool _reliable;

        public event MessagesPublished? OnMessagesPublished;

        public RabbitMqPublisher(RabbitMqConfiguration conf, string exchangeName, string exchangeType, bool durable, bool reliable, bool autoDelete, ILogger<RabbitMqPublisher> logger) : base(conf, logger)
        {
            _exchangeName = exchangeName;
            _exchangeType = exchangeType;
            _durable = durable;
            _reliable = reliable;
            _autoDelete = autoDelete;
            CreateConnectionAsync(conf);
        }

        protected override void ChannelRegistration(IModel channel)
        {
            channel.ModelShutdown += Channel_ModelShutdown;

            var arguments = _exchangeType.Equals(ExtendedExchangeType.ConsistentHash)
                    ? new Dictionary<string, object> { { "hash-header", "hash-on" } }
                    : null;
            channel.ExchangeDeclare(_exchangeName, _exchangeType, _durable, _autoDelete, arguments);

            channel.BasicAcks += (sender, eventArgs) =>
            {
                lock (_messages)
                {
                    OnMessagesPublished?.Invoke(_messages.Where(m => m.IsMatching(eventArgs.DeliveryTag, eventArgs.Multiple)).Select(m => m.MessageId).ToArray());
                    if (_reliable)
                    {
                        //remove ack messages from the pending ones
                        _messages.RemoveAll(m => m.IsMatching(eventArgs.DeliveryTag, eventArgs.Multiple));
                        Logger.LogTrace("Message(s) with delivery tag {0} {1} aknowledged", eventArgs.Multiple ? "<=" : "=", eventArgs.DeliveryTag);
                    }
                }
            };

            channel.BasicNacks += (sender, eventArgs) =>
            {

                if (_reliable)
                {
                    lock (_messages)
                    {
                        //requeue nack messages
                        _messages.FindAll(m => m.IsMatching(eventArgs.DeliveryTag, eventArgs.Multiple)).ForEach(m => Publish(m));
                        Logger.LogWarning("Message(s) with delivery tag {0} {1} requeued", eventArgs.Multiple ? "<=" : "=", eventArgs.DeliveryTag);
                    }
                }
            };

            channel.ConfirmSelect();
        }

        private void Channel_ModelShutdown(object? sender, ShutdownEventArgs e)
        {
            if (Channel != null)
            {
                while (!Channel.IsOpen && !CancellationTokenSource.Token.IsCancellationRequested)
                {
                    Task.Delay(Configuration.NetworkRecoveryInterval, CancellationTokenSource.Token).Wait();
                }
                if (Channel.IsOpen && _reliable)
                {
                    lock (_messages)
                    {
                        Logger.LogWarning("Channel recovery, {0} messages requeued", _messages.Count);
                        _messages.ForEach(m => m.ResetDeliveryTag());
                        //resend all pending messages
                        foreach (var msg in _messages)
                        {
                            Publish(msg);
                        }
                    }
                }
            }
        }

        public void Publish<T>(T content, string routingKey, string? messageId = null, object? consistentHashing = null, string? correlationId = null, Dictionary<string, object>? additionnalHeaders = null)
        {
            var msg = new PreparedMessage(JsonSerializer.Serialize(content), routingKey, messageId, consistentHashing, correlationId, additionnalHeaders);
            if (_reliable)
            {
                lock (_messages)
                {
                    _messages.Add(msg);
                }
            }
            Publish(msg);
        }

        public bool PublishBatchConfirm(List<PreparedMessage> messages)
        {
            if (Channel == null || !Channel.IsOpen)
            {
                Logger.LogError("Cannot publish {0} messages on exchange {1}, channel is not open", messages.Count, _exchangeName);
                return false;
            }
            lock (Channel)
            {
                try
                {
                    foreach (var msg in messages)
                    {
                        msg.DeliveryTag = Channel.NextPublishSeqNo;

                        var body = Encoding.UTF8.GetBytes(msg.Content);

                        var properties = Channel.CreateBasicProperties();
                        properties.ContentType = "application/json";
                        properties.ContentEncoding = "utf-8";
                        properties.Persistent = _reliable;
                        properties.MessageId = msg.MessageId;
                        properties.CorrelationId = msg.CorrelationId;

                        properties.Headers = msg.AdditionnalHeaders ?? new Dictionary<string, object>();
                        properties.Headers["hash-on"] = msg.GetConsistentHash();

                        Channel.BasicPublish(exchange: _exchangeName,
                                                routingKey: msg.RoutingKey,
                                                mandatory: false,
                                                basicProperties: properties,
                                                body: body);

                    }
                    return Channel.WaitForConfirms(TimeSpan.FromSeconds(5));
                }
                catch (Exception e)
                {
                    Logger.LogError(e, "Cannot publish {0} messages on exchange {1}", messages.Count, _exchangeName);
                    return false;
                }
            }
        }

        private void Publish(PreparedMessage msg)
        {
            if (Channel == null || !Channel.IsOpen)
            {
                Logger.LogError("Cannot publish message {0} on exchange {2}, channel is not open", msg.RoutingKey,_exchangeName);
                return;
            }
            lock (Channel)
            {
                msg.DeliveryTag = Channel.NextPublishSeqNo;
                try
                {
                    var body = Encoding.UTF8.GetBytes(msg.Content);

                    var properties = Channel.CreateBasicProperties();
                    properties.ContentType = "application/json";
                    properties.ContentEncoding = "utf-8";
                    properties.Persistent = _reliable;
                    properties.MessageId = msg.MessageId;
                    properties.CorrelationId = msg.CorrelationId;

                    properties.Headers = msg.AdditionnalHeaders ?? new Dictionary<string, object>();
                    properties.Headers["hash-on"] = msg.GetConsistentHash();

                    Channel.BasicPublish(exchange: _exchangeName,
                                            routingKey: msg.RoutingKey,
                                            mandatory: false,
                                            basicProperties: properties,
                                            body: body);
                }
                catch (Exception e)
                {
                    Logger.LogError(e, "Cannot publish message {0} ({1}) on exchange {2}", msg.RoutingKey, msg.DeliveryTag, _exchangeName);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing && _autoDelete)
                {
                    Channel.ExchangeDelete(_exchangeName);
                }
            }
            base.Dispose(disposing);
        }

    }
}
