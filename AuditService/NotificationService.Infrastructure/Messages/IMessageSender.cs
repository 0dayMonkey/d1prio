using NotificationService.Domain.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.Messages
{
    public interface IMessageSender
    {
        Task<bool> Open();

        Task<bool> SendMessage(NotificationMessage message, MetaData? metaData = null);
    }
}
