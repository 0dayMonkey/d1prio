#region

using System.Threading;

#endregion

namespace FloorServer.Client.Ect
{
    public class EctResponse
    {
        #region Members

        private int _timeout;

        #endregion

        public EctResponse(int timeout)
        {
            _timeout = timeout;
            ResetEvent = new AutoResetEvent(false);
        }



        public AutoResetEvent ResetEvent { get; private set; }
        public IEctTransaction EctTransaction { get; set; }

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
