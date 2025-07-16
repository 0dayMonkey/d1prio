using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.RTDS.KeyValueTools
{

    public enum Operator
    {
        and,
        or,
        user
    }

    public class Filter
    {
        public List<(string, string)> Arguments { get; private set; }

        public Operator Operator { get; private set; }

        public IFilterDelegate? FilterDelegate { get; set; }

        public Filter(List<(string, string)> Arguments, Operator Operator)
        {
            this.Arguments = Arguments ?? throw new ArgumentNullException(nameof(Arguments));
            this.Operator = Operator;
        }

    }
}
