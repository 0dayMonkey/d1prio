#region

using FloorServer.Client.Communication;
using FloorServer.Client.Tools;

#endregion

namespace FloorServer.Client
{
    internal class QueryResponseHandler : ResponseHandler
    {
        public QueryResponseHandler(int timeout)
            : base(timeout)
        {
        }

        public FloorQueryResponse QueryResult { get; set; }
    }
}
