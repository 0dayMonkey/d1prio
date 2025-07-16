using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NotificationService.Infrastructure.Dispatcher;
using NotificationService.Infrastructure.Messages;
using NotificationService.Infrastructure.RabbitMQ;
using RabbitMq.Messaging;
using RabbitMq.Messaging.Consumer;
using RabbitMq.Messaging.Publisher;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.Workflows
{
    public class GlobalWorkflowInitializer : IWorkflowInitializer
    {

        private IConfiguration _configuration;

        public GlobalWorkflowInitializer(IConfiguration congiguration)
        {
            _configuration = congiguration;
        }

        public void AddMessageManagers(IServiceCollection services)
        {
            
            services.AddSingleton<IMessageSender>(n => new RabbitMQMultiSender(
                new RabbitMqConfiguration()
                {
                    Uri = new Uri(_configuration.GetValue<string>("MSG_BROKER:ADDRESS")),
                    UserName = _configuration.GetValue<string>("MSG_BROKER:USERNAME"),
                    Password = _configuration.GetValue<string>("MSG_BROKER:PASSWORD"),
                    VirtualHost = "/",
                },
                n.GetRequiredService<ILoggerFactory>().CreateLogger<RabbitMqPublisher>(),
                n.GetRequiredService<ILoggerFactory>().CreateLogger<RabbitMQMultiSender>()
            )); 
            services.AddSingleton<IMessageDispatcher, GlobalMessageDispatcher>();
        }

        public void AddRabbitMQHandlers(IServiceCollection services)
        {
            services.AddSingleton<IRBMQProxyContext, LocalContext>();
            var rbConfHO = new RabbitMqConfiguration()
            {
                Uri = new Uri(_configuration.GetValue<string>("MSG_BROKER:ADDRESS")),
                UserName = _configuration.GetValue<string>("MSG_BROKER:USERNAME"),
                Password = _configuration.GetValue<string>("MSG_BROKER:PASSWORD"),
                VirtualHost = "/",
            };
            services.AddSingleton<IRabbitMqConsumer>(x => new RabbitMqConsumer(
                rbConfHO,
                100,
                true,
                x.GetRequiredService<ILoggerFactory>()));
        }

    }
}
