#region

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using FloorServer.Client.Communication;
using FloorServer.Client.Boss;

#endregion

namespace FloorServer.Client.Tools
{
    public class FloorQueryArgs : EventArgs
    {
        private List<StringDictionary> rows;

        public FloorQueryArgs()
        {
            rows = new List<StringDictionary>();
        }

        public FloorQueryArgs(Bom msg)
            : this()
        {
            int count = int.Parse(msg.GetString(Keys.COUNT));
            List<string> fields = msg.GetStrList(Tags.FIELDS);
            Bom values = msg.GetMsg(Tags.VALUES);

            for (int i = 1; i <= count; i++)
            {
                List<string> tmp = values.GetStrList(i.ToString());
                StringDictionary dico = new StringDictionary();
                if (tmp.Count == 0)
                    continue;
                
                for (int j = 0; j < fields.Count; j++)
                    dico.Add(fields[j], tmp[j]);

                Rows.Add(dico);
            }

        }

        public List<StringDictionary> Rows
        {
            get { return rows; }
        }
    }
}
