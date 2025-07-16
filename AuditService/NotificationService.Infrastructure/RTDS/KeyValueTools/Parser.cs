using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.RTDS.KeyValueTools
{

    public class Parser
    {

        public static KeyValueMap Parse(string line)
        {
            KeyValueMap result = new();

            StringBuilder key = new();
            StringBuilder value = new();
            StringList stList = new();

            var currentParsing = Parsing.key;
            char currentStringSeparator = '"';
            int acoBalance = 0;

            foreach (var c in line + " ")
            {
                switch (currentParsing)
                {
                    case Parsing.key:
                        {
                            if (c == ' ')
                            {
                                key.Clear();
                                continue;
                            }
                            if (c == '=')
                            {
                                currentParsing = Parsing.anyKindOfValue;
                                continue;
                            }
                            key.Append(c);
                            break;
                        }
                    case Parsing.anyKindOfValue:
                        {
                            if (c == '{')
                            {
                                currentParsing = Parsing.kvValue;
                                value.Clear();
                                acoBalance = 1;
                                continue;
                            }
                            else
                            {
                                currentStringSeparator = c;
                                currentParsing = Parsing.stringValue;
                                value.Clear();
                                stList = new StringList();
                            }
                            break;
                        }
                    case Parsing.kvValue:
                        {
                            if (c == '{')
                            {
                                acoBalance++;
                            }
                            if (c == '}')
                            {
                                acoBalance--;
                            }
                            if (c == '}' && acoBalance == 0)
                            {
                                KeyValueMap map = Parse(value.ToString());
                                result.Put(new KVValue(key.ToString(), map));
                                value.Clear();
                                key.Clear();
                                currentParsing = Parsing.key;
                                continue;
                            }
                            value.Append(c);
                            break;
                        }
                    case Parsing.endString:
                        {
                            if (c == ' ')
                            {
                                result.Put(new StringValue(key.ToString(), stList));
                                key.Clear();
                                value.Clear();
                                currentParsing = Parsing.key;
                            }
                            else
                            {
                                currentStringSeparator = c;
                                currentParsing = Parsing.stringValue;
                                value.Clear();
                                continue;
                            }
                            break;
                        }
                    case Parsing.stringValue:
                        {
                            if (c == currentStringSeparator)
                            {
                                stList.Add(value.ToString(), currentStringSeparator);
                                value.Clear();
                                currentParsing = Parsing.endString;
                                continue;
                            }
                            value.Append(c);
                            break;
                        }
                }
            }
            return result;
        }

    }

}
