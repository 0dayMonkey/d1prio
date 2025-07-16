using NotificationService.Domain.Messages;
using NotificationService.Infrastructure.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.Dispatcher
{
    public class LocalMessageDispatcher : MessageDispatcherBase, IMessageDispatcher
    {
        public LocalMessageDispatcher(IMessageSender messageSender) : base(messageSender)
        {
        }


        public async Task DispatchMessage(NotificationMessage message, string RoutingKey)
        {
            if (message.Channels.AlarmMessage != null) await _messageSender.SendMessage(message);
        }

    }
}
