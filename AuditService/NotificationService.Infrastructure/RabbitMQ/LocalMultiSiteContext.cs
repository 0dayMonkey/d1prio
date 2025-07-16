using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.RabbitMQ
{
    public class LocalMultiSiteContext : IRBMQProxyContext
    {
        private int _siteId;

        public LocalMultiSiteContext(int siteId)
        {
            this._siteId = siteId;
        }

        public string ExchangeName => $"galaxis.topics.{_siteId}";

        public string QueueNameSuffix => $"local_{_siteId}";
    }
}
