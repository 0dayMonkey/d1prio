
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.RTDS.KeyValueTools
{
    public class KeyValueMap
    {
        private readonly Dictionary<string, IValue> map = new();

        public int Size()
        {
            return map.Count;
        }

        public IValue? Get(string key)
        {
            if (map.TryGetValue(key, out var result))
            {
                return result;
            }
            return null;
        }

        public HashSet<string> GetAllKeys()
        {
            var result = new HashSet<string>();
            foreach (var item in map)
            {
                result.Add(item.Key);
                var subKeys = item.Value.GetSubKeys();
                foreach (var subKey in subKeys) result.Add(subKey);
            }
            return result;
        }

        public Dictionary<string, IValue> GetMap()
        {
            return map;
        }


        public string? GetStringValue(string key)
        {
            var val = Get(key);
            if (val == null) return null;
            return val.AsString();
        }

        public long? GetLongValue(string key)
        {
            var val = Get(key);
            if (val == null) return null;
            return val.AsLong();
        }

        public int? GetIntValue(string key)
        {
            var val = Get(key);
            if (val == null) return null;
            return val.AsInt();
        }


        public DateTime? GetTimestampValue(string key)
        {
            var unixTS = GetLongValue(key);
            if (unixTS == null) return null;
            return new DateTime((unixTS ?? 0) * 1000);
        }

        public bool MatchFilter(Filter filter)
        {
            switch (filter.Operator)
            {
                case Operator.and:
                    {
                        foreach (var item in filter.Arguments)
                        {
                            var value = LookForStringValue(item.Item1);
                            if (value == null) return false;
                            if (value != item.Item2) return false;
                        }
                        return true;
                    }
                case Operator.or:
                    {
                        foreach (var item in filter.Arguments)
                        {
                            var value = LookForStringValue(item.Item1);
                            if (value == null) continue;
                            if (value == item.Item2)
                                return true;
                        }
                        return false;
                    }
                case Operator.user:
                    {
                        if (filter.FilterDelegate == null) throw new NoNullAllowedException("delegate cannot be null if user mode selected");
                        return filter.FilterDelegate.IsMatching(this);
                    }
            }
            throw new InvalidOperationException("something else than and or or");
        }

        public string? LookForStringValue(string key)
        {
            var value = LookForValue(key);
            if (value == null) return null;
            return value.AsString();
        }

        public string? LookForStringsValue(string key)
        {
            var value = LookForValue(key);
            if (value == null) return null;
            var valueList = value.AsStringList();
            if (valueList == null) return null;
            return string.Join(",", valueList);
        }


        /// <summary>
        ///  Look in all tag and sub tag a value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IValue? LookForValue(string key)
        {
            var value = Get(key);
            if (value != null) return value;

            foreach (var v in map.Values)
            {
                var innerValue = v.LookForValue(key);
                if (innerValue != null) return innerValue;
            }
            return null;
        }

        public void Put(IValue value)
        {
            if (value.GetKeyName().Length == 0) return;

            if (map.ContainsKey(value.GetKeyName()))
            {
                map[value.GetKeyName()] = value;
            }
            else
            {
                map.Add(value.GetKeyName(), value);
            }
        }

        public HashSet<string> KeySet()
        {
            return map.Keys.ToHashSet();
        }

        public List<IValue> Values()
        {
            return map.Values.ToList();
        }

        public HashSet<KeyValuePair<string, IValue>> EntrySet()
        {
            return map.ToHashSet();
        }

        public Dictionary<string, string> Flatten(bool removeNumericalKey)
        {
            var kv = new KVValue("", this);
            var map = kv.GetFlatRepresentation("");

            var result = new Dictionary<string, string>(1000);

            foreach (var item in map)
            {
                var key = item.Key;
                if (key.StartsWith("."))
                {
                    key = key[1..];
                }
                if (key.EndsWith("."))
                {
                    key = key[0..^1];
                }
                result.Add(key, item.Value);
            }

            if (removeNumericalKey)
            {
                var toDelete = new HashSet<string>();
                foreach (var key in result.Keys)
                {
                    var tokens = key.Split('.');
                    if (tokens.Any(z => int.TryParse(z, out int dummy))) toDelete.Add(key);
                }
                foreach (var key in toDelete) result.Remove(key);
            }

            return result;
        }

        public override string ToString()
        {
            var result = new StringBuilder();

            foreach (var item in map)
            {
                if (result.Length > 0) result.Append(' ');
                result.Append(item.Key).Append('=').Append(item.Value);
            }
            return result.ToString();
        }

        public bool SetForAllItemValue(string key, string value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var result = false;
            foreach (var entry in map)
            {
                throw new NotImplementedException("No idea what it is supposed to do");
            }
            return result;
        }

        public void AddKVMap(string key, KeyValueMap kvMap)
        {
            Put(new KVValue(key, kvMap));
        }

        public void AddStrings(string key, params string[] values)
        {
            AddStringList(key, values.ToList());
        }

        public void AddStringList(string key, List<string> values)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var stList = new StringList();
            foreach (var st in values) stList.AddAutoSeparator(st);
            Put(new StringValue(key, stList));
        }

        /// <summary>
        /// add a value in a values sublist
        /// </summary>
        /// <param name="key">the key to add.</param>
        /// <param name="subListKey">should always be VALUES</param>
        /// <param name="value"></param>
        /// <returns>true if value has been added.</returns>
        public bool Add(string key, string subListKey, string value)
        {

            if (!map.TryGetValue(subListKey, out IValue? subList)) return false;
            if (subList is KVValue)
            {
                ((KVValue)subList).GetMap().Put(new StringValue(key, value));
                return true;
            }
            return false;
        }
    }
}
