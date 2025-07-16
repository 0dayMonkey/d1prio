using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FloorServer.Client.Commands
{
    public abstract class FSCommand
    {
        public string WhereClause;
        public bool Persisted;
    }
}
