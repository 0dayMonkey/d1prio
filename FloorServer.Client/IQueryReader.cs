#region

using System;
using System.Collections.Specialized;

#endregion

namespace FloorServer.Client
{
    public interface IQueryReader : IDisposable
    {
        StringDictionary Current { get; }
        bool Read();
        IQueryReaderHelper GetHelper();
    }
}