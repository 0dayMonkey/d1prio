using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.RabbitMQ
{
    public class LocalContext : IRBMQProxyContext
    {

        public string ExchangeName => "galaxis.topics";

        public string QueueNameSuffix => "local";
    }
}
