using NotificationService.Domain.Messages;
using NotificationService.Infrastructure.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.Dispatcher
{
    public abstract class MessageDispatcherBase
    {
        protected readonly IMessageSender _messageSender;

        public MessageDispatcherBase(IMessageSender messageSender)
        {
            _messageSender = messageSender;
        }

    }
}
