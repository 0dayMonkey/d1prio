using NotificationService.Domain.Messages;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.Dispatcher
{
    public interface IMessageDispatcher
    {
        Task DispatchMessage(NotificationMessage message, string routingKey);
    }
}