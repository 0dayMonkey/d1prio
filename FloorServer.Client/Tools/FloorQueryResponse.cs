#region

using System.Collections.Generic;
using System.Collections.Specialized;
using FloorServer.Client.Communication;
using FloorServer.Client.Boss;

#endregion

namespace FloorServer.Client.Tools
{
    public class FloorQueryResponse
    {
        private readonly List<StringDictionary> _rows;

        public FloorQueryResponse()
        {
            _rows = new List<StringDictionary>();
        }

        public FloorQueryResponse(Bom msg)
            : this()
        {
            var count = int.Parse(msg.GetString(Keys.COUNT));
            var fields = msg.GetStrList(Tags.FIELDS);
            var values = msg.GetMsg(Tags.VALUES);

            for (var i = 1; i <= count; i++)
            {
                var tmp = values.GetStrList(i.ToString());
                var dico = new StringDictionary();
                if (tmp.Count == 0)
                    continue;
                for (var j = 0; j < fields.Count; j++)
                    dico.Add(fields[j], tmp[j]);

                Rows.Add(dico);
            }

        }

        public List<StringDictionary> Rows
        {
            get { return _rows; }
        }
    }
}
