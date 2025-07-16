using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.RTDS.KeyValueTools
{
    public interface IValue
    {
        Dictionary<string, string> GetFlatRepresentation(string prefix);

        bool SetForAllItemValue(string key, string value);

        int? AsInt();

        long? AsLong();

        string AsString();

        KVValue? AsKeyValue();

        StringValue? AsStringValue();

        IValue? LookForValue(string key);

        List<string>? AsStringList();

        string GetKeyName();
        HashSet<string> GetSubKeys();
    }
}
