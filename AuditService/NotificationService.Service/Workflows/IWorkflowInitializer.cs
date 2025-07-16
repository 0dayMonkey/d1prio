using Microsoft.Extensions.DependencyInjection;
using NotificationService.Infrastructure.Dispatcher;
using NotificationService.Infrastructure.Messages;
using RabbitMq.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.Workflows
{
    public interface IWorkflowInitializer
    {
        void AddMessageManagers(IServiceCollection services);
        void AddRabbitMQHandlers(IServiceCollection services);
    }
}
