using Microsoft.Extensions.Logging;
using NotificationService.Domain.Messages;
using NotificationService.Infrastructure.Messages;
using RabbitMq.Messaging;
using RabbitMq.Messaging.Publisher;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.RabbitMQ
{
    public class RabbitMQMultiSender : IMessageSender
    {
        private Dictionary<string, IRabbitMqPublisher> _publisherPool = new();
        private RabbitMqConfiguration _configuration;
        private ILogger<RabbitMqPublisher> _rbmqLogger;
        private ILogger<RabbitMQMultiSender> _logger;

        public RabbitMQMultiSender(RabbitMqConfiguration configuration, ILogger<RabbitMqPublisher> rbmqLogger, ILogger<RabbitMQMultiSender> logger)
        {
            _configuration = configuration;
            _rbmqLogger = rbmqLogger;
            _logger = logger;
        }

        public Task<bool> Open()
        {
            return Task.FromResult(true);
        }

        public Task<bool> SendMessage(NotificationMessage message, MetaData? metaData = null)
        {
            var rabbitMQMetaData = metaData as RabbitMQMetaData;
            if (rabbitMQMetaData == null) throw new InvalidOperationException("Meta data need to be a RabbitMQMetaData and not null");
            try
            {
                if (rabbitMQMetaData.EchangeName == null) return Task.FromResult(false);
                IRabbitMqPublisher? publisher;
                if (!_publisherPool.TryGetValue(rabbitMQMetaData.EchangeName, out publisher))
                {
                    publisher = new RabbitMqPublisher(
                        _configuration,
                        rabbitMQMetaData.EchangeName,
                        ExchangeType.Topic,
                        true,
                        false,
                        false,
                       _rbmqLogger
                        );
                    _publisherPool.Add(rabbitMQMetaData.EchangeName, publisher);
                }
                publisher?.Publish(message, rabbitMQMetaData.RoutingKey);
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cannot send message");
                throw;
            }
        }
    }
}
