#region

using System.Collections.Generic;
using FloorServer.Client.Boss;
using Microsoft.Extensions.Logging;

#endregion

namespace FloorServer.Client.Communication
{
    public class BomQuery : Bom, IBomQuery
    {
        private static long queryNumber = 0;

        public BomQuery(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            PutString(Boss.Tags.ACT, Boss.Tags.QUERY);
            PutString(Boss.Tags.DST, Boss.Tags.FS);
            PutString(Keys.SEQQR, (queryNumber++).ToString());
        }

        public IBomQuery Using(string sourceName)
        {
            PutString(Boss.Tags.SRC, sourceName);
            return this;
        }

        public IBomQuery Get(params string[] fields)
        {
            if (fields != null)
                PutStrList(Boss.Tags.FIELDS, new List<string>(fields));
            return this;
        }

        public IBomQuery On(string table)
        {
            PutString(Boss.Tags.TABLE, table);
            return this;
        }

        public IBomQuery Where(string whereClause)
        {
            if (!string.IsNullOrEmpty(whereClause))
                PutString(Keys.WHERE, whereClause);
            return this;
        }
    }
}
