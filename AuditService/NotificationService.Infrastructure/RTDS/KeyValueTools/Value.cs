using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.RTDS.KeyValueTools
{
    public class Value
    {

        protected readonly string keyName;

        public Value(string keyName)
        {
            this.keyName = keyName;
        }

        public string GetKeyName()
        {
            return keyName;
        }

    }
}
