using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.RTDS.KeyValueTools
{
    public class StringList : IEnumerable<StringItem>
    {

        private readonly List<StringItem> _list = new();

        public int Size()
        {
            return _list.Count;
        }

        public void Add(StringItem e)
        {
            _list.Add(e);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public StringItem Get(int index)
        {
            return _list[index];
        }

        public StringItem Set(int index, StringItem element)
        {
            return _list[index] = element;
        }


        public override string ToString()
        {
            StringBuilder builder = new();
            _list.ForEach(z => builder.Append(z));
            return builder.ToString();
        }

        public void Add(string value, char separator)
        {
            _list.Add(new StringItem(value, separator));
        }

        public void AddAutoSeparator(string dirtyValue)
        {
            var value = CleanString(dirtyValue);
            if (value == null) return;
            var possibleSeparator = new char[] { '"', '@', '%', '\'' };
            char? c = null;
            foreach (char item in possibleSeparator)
            {
                if (!value.Contains("" + item))
                {
                    c = item;
                    break;
                }
            }
            if (c == null)
            {
                throw new InvalidOperationException("String not supported");
            }
            Add(new StringItem(value, c.Value));
        }

        private string CleanString(string dirtyValue)
        {
            StringBuilder builder = new StringBuilder();
            foreach(char item in dirtyValue)
            {
                if((int)item >= 32) builder.Append(item);
            }
            return builder.ToString().Trim();
        }

        public IEnumerator<StringItem> GetEnumerator()
        {
            return ((IEnumerable<StringItem>)_list).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<StringItem>)_list).GetEnumerator();
        }
    }
}
