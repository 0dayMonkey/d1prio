using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMq.Messaging.Consumer
{
    public enum EchangeCreationType
    {
        DoNotCreate,
        CreateDurable,
        CreateNonDurable
    }
}
