using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.RTDS.KeyValueTools
{
    public class StringItem
    {
        private readonly string stValue;
        private readonly char separator;

        public StringItem(string stValue, char separator)
        {
            this.stValue = stValue;
            this.separator = separator;
        }

        public string GetStValue()
        {
            return stValue;
        }

        public char GetSeparator()
        {
            return separator;
        }

        public override string ToString()
        {
            return $"{separator}{stValue}{separator}";
        }

    }
}
