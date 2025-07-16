using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.RTDS.KeyValueTools
{
    public class StringValue : Value, IValue
    {
        private readonly StringList _values;

        public StringValue(string keyName, StringList value)
            : base(keyName)
        {
            _values = value;
        }

        public StringValue(string keyName, string singleValue)
            : base(keyName)
        {
            _values = new StringList();
            _values.AddAutoSeparator(singleValue);
        }

        public StringList GetValues()
        {
            return _values;
        }

        public bool SetForAllItemValue(string key, string value)
        {
            if (GetKeyName().Equals(key))
            {
                _values.Clear();
                _values.AddAutoSeparator(value);
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return "" + _values;
        }

        public int? AsInt()
        {
            if (_values.Size() != 1)
            {
                return null;
            }
            if (int.TryParse(_values.Get(0).GetStValue(), out var parsedInt))
            {
                return parsedInt;
            }
            else
            {
                return null;
            }
        }


        public long? AsLong()
        {
            if (_values.Size() != 1)
            {
                return null;
            }
            if (long.TryParse(_values.Get(0).GetStValue(), out var parsedInt))
            {
                return parsedInt;
            }
            else
            {
                return null;
            }
        }


        public Dictionary<string, string> GetFlatRepresentation(string prefix)
        {
            var result = new Dictionary<string, string>();

            int cpt = 0;
            if (_values.Size() == 1)
            {
                result.Add(prefix + keyName + ".", _values.Get(0).GetStValue());
            }
            else
            {
                foreach (StringItem st in _values)
                {
                    var c = 'a' + cpt;
                    string cptSt = "" + (char)c;
                    result.Add(prefix + keyName + "." + cptSt + ".", st.GetStValue());
                    cpt++;
                }
            }
            return result;
        }


        public string AsString()
        {
            if (_values.Size() == 0)
            {
                return "";
            }
            return _values.Get(0).GetStValue();
        }


        public IValue? LookForValue(string key)
        {
            return null;
        }

        public KVValue? AsKeyValue()
        {
            return null;
        }


        public StringValue AsStringValue()
        {
            return this;
        }

        public List<string> AsStringList()
        {
            return _values.Select(z => z.GetStValue()).ToList();
        }

        public HashSet<string> GetSubKeys()
        {
            return new HashSet<string>();
        }
    }
}
