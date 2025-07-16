using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NotificationService.Infrastructure.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NotificationService.Service
{
    internal class InitNotificationService: BackgroundService
    {
        private readonly IMessageConsumer _messageConsummer;
        private readonly IMessageSender _messageSender;
        private readonly ILogger<InitNotificationService> _logger;

        public InitNotificationService(IMessageConsumer messageConsummer, IMessageSender messageSender, ILogger<InitNotificationService> logger)
        {
            _messageConsummer = messageConsummer;
            _messageSender = messageSender;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _messageConsummer.Subscribe();
            var messageSenderResult = await _messageSender.Open();
            _logger.LogInformation($"message sender (Alarm-Server) connected : {messageSenderResult}");
        }
    }
}
