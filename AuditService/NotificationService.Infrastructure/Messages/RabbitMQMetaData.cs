using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.Messages
{
    public class RabbitMQMetaData : MetaData
    {
        public RabbitMQMetaData(string routingKey, string echangeName)
        {
            RoutingKey = routingKey;
            EchangeName = echangeName;
        }

        public string RoutingKey { get; set; }
        public string EchangeName { get; set; }
    }
}
