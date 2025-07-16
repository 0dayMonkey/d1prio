using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using NotificationService.Infrastructure.Dispatcher;
using NotificationService.Infrastructure.Messages;
using NotificationService.Infrastructure.RabbitMQ;
using NotificationService.Infrastructure.RTDS;
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
    public class LocalWorkflowInitializer : IWorkflowInitializer
    {
        private IConfiguration _configuration;
        private bool _multiSite;

        public LocalWorkflowInitializer(IConfiguration configuration, bool multiSite)
        {
            _configuration = configuration;
            _multiSite = multiSite;
        }

        public void AddMessageManagers(IServiceCollection services)
        {
            services.AddSingleton<IMessageSender, AlarmServerProxy>();
            services.AddSingleton<IMessageDispatcher, LocalMessageDispatcher>();
        }

        public void AddRabbitMQHandlers(IServiceCollection services)
        {
            if (_multiSite)
            {
                var siteID = _configuration.GetValue<int>("CFG:CASINO_ID");
                services.AddSingleton<IRBMQProxyContext>(n => new LocalMultiSiteContext(siteID));
            }
            else
            {
                services.AddSingleton<IRBMQProxyContext, LocalContext>();
            }
            var msgBroker = _multiSite ? "MSG_BROKER_HO" : "MSG_BROKER";
            var rbConf = new RabbitMqConfiguration()
            {           
                Uri = new Uri(_configuration.GetValue<string>($"{msgBroker}:ADDRESS")),
                UserName = _configuration.GetValue<string>($"{msgBroker}:USERNAME"),
                Password = _configuration.GetValue<string>($"{msgBroker}:PASSWORD"),
                VirtualHost = "/",
            };

            services.AddSingleton<IRabbitMqConsumer>(x => new RabbitMqConsumer(
                rbConf,
                100,
                true,
                x.GetRequiredService<ILoggerFactory>()));
        }

    }
}
