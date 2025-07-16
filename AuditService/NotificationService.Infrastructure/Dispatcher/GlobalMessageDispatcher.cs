using Microsoft.Extensions.Logging;
using NotificationService.Domain.Messages;
using NotificationService.Infrastructure.Messages;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.Dispatcher
{
    public class GlobalMessageDispatcher : MessageDispatcherBase, IMessageDispatcher
    {
        private ILogger<GlobalMessageDispatcher> _logger;

        public GlobalMessageDispatcher(IMessageSender messageSender, ILogger<GlobalMessageDispatcher> logger) : base(messageSender)
        {
            _logger = logger;
        }

        public async Task DispatchMessage(NotificationMessage message, string routingKey)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            var siteId = message?.KeyValues?.FirstOrDefault(n => "SITEID".Equals(n.Key))?.Value;
            if (siteId == null)
            {
                _logger.LogError($"Message {routingKey} has no SITEID, cannot forward this message");
                return;
            }
            var exchangeName = $"galaxis.topics.{siteId}";
            _logger.LogInformation($"Send to {exchangeName}");
            await _messageSender.SendMessage(message!, new RabbitMQMetaData(routingKey, exchangeName));
        }

    }
}
