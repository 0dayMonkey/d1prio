using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NotificationService.Domain.Messages;
using NotificationService.Infrastructure.Dispatcher;
using NotificationService.Infrastructure.Messages;
using RabbitMq.Messaging.Consumer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;


namespace NotificationService.Infrastructure.RabbitMQ
{
    public class RabbitMQProxy : BackgroundWorker, IMessageConsumer
    {

        private readonly ILogger<RabbitMQProxy> _logger;
        private readonly IRabbitMqConsumer _consumer;
        private readonly IMessageDispatcher _messageDispatcher;
        private readonly IRBMQProxyContext _RBMQProxyContext;

        public RabbitMQProxy(
            ILogger<RabbitMQProxy> logger, 
            IRabbitMqConsumer consumer, 
            IMessageDispatcher messageDispatcher,
            IRBMQProxyContext context)
        {
            _logger = logger;
            _consumer = consumer;
            _messageDispatcher = messageDispatcher;
            _RBMQProxyContext = context;
        }

        public void Subscribe()
        {
#if DEBUG
            var queueName = $"NotificationService_{Environment.MachineName}";
#else
            var queueName = "NotificationService";
#endif
            queueName = $"{queueName}_{_RBMQProxyContext.QueueNameSuffix}";
            _logger.LogInformation($"queue name {queueName}");

            var exchangeName = _RBMQProxyContext.ExchangeName;
            _logger.LogInformation($"Subcribe to AML events [{exchangeName}]");
            _consumer.AddQueue(queueName: queueName, exchangeName: exchangeName,
                subscriptions: new List<ISubscription>
                {
                    new Subscription<NotificationMessage>("Notification.*.*", NotificationMessage)
                }, durable: false, exclusive: false, autoDelete: false, autoAck: true, createExchange: EchangeCreationType.CreateDurable);
        }

        private void NotificationMessage(IRabbitMqConsumer consumer, NotificationMessage message, MessageEnvelope envelope)
        {
            if(envelope.RoutingKey == null)
            {
                _logger.LogError("No routing-key received with the RMQ message");
                throw new InvalidOperationException("No routing key received with the RMQ message");
            }
            _messageDispatcher.DispatchMessage(message, envelope.RoutingKey).Wait();
        }

    }
}
