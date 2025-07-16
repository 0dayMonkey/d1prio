using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.RTDS.KeyValueTools
{
    public class KVValue : Value, IValue
    {
        private readonly KeyValueMap _map;

        public KVValue(string keyName, KeyValueMap map)
                : base(keyName)
        {
            _map = map;
        }

        public KeyValueMap GetMap()
        {
            return _map;
        }


        public bool SetForAllItemValue(string key, string value)
        {
            return _map.SetForAllItemValue(key, value);
        }


        public override string ToString()
        {
            return "{" + _map + "}";
        }


        public Dictionary<string, string> GetFlatRepresentation(string prefix)
        {
            var result = new Dictionary<string, string>();

            foreach (var item in _map.GetMap())
            {
                var subValues = item.Value.GetFlatRepresentation(prefix + GetKeyName() + ".");
                foreach (var subValue in subValues)
                {
                    result.Add(subValue.Key, subValue.Value);
                }
            }

            return result;
        }

        public int? AsInt()
        {
            return null;
        }


        public long? AsLong()
        {
            return null;
        }


        public string AsString()
        {
            return "";
        }


        public IValue? LookForValue(string key)
        {
            return _map.LookForValue(key);
        }


        public KVValue AsKeyValue()
        {
            return this;
        }


        public StringValue? AsStringValue()
        {
            return null;
        }

        public List<string>? AsStringList()
        {
            return null;
        }

        public HashSet<string> GetSubKeys()
        {
            return _map.GetAllKeys();
        }
    }
}
