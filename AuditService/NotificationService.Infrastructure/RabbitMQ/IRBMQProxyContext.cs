using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.RabbitMQ
{
    public interface IRBMQProxyContext
    {
        string QueueNameSuffix { get; }
        string ExchangeName { get; }
    }
}
