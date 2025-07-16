#region

using System.Threading;

#endregion

namespace FloorServer.Client.Communication
{
    internal abstract class ResponseHandler
    {
        #region Members

        private readonly int _timeout;

        #endregion

        internal ResponseHandler(int timeout)
        {
            _timeout = timeout;
            ResetEvent = new AutoResetEvent(false);
        }

        public AutoResetEvent ResetEvent { get; private set; }

        #region Methods

        public bool Wait()
        {
            return ResetEvent.WaitOne(_timeout);
        }

        public void Set()
        {
            ResetEvent.Set();
        }

        #endregion
    }
}
