using FloorServer.Client;
using FloorServer.Client.Configuration;
using Microsoft.Extensions.Logging;

namespace FloorServer.Client.Pool
{
    public class FloorServerPooledClient : FloorServerClient
    {
        private readonly FloorServerClientPool _pool;
        private bool _free = true;

        internal bool Free
        {
            get { return _free; }
        }

        internal FloorServerPooledClient(FloorServerClientPool pool, FloorServerConfiguration config, ILoggerFactory loggerFactory, string clientName) : base(config, loggerFactory, clientName)
        {
            _pool = pool;
        }

        internal void MarkAsReleased()
        {
            _free = true;
        }

        internal void MarkAsUsed()
        {
            _free = false;
        }

        internal void Dispose(bool returnToPool)
        {
            if (_pool != null)
                _pool.Release(this);

            if (returnToPool)
                return;

            base.Dispose();
        }

        public override void Dispose()
        {
            Dispose(true);
        }
    }
}
