#region

using FloorServer.Client.Communication;

#endregion

namespace FloorServer.Client.Ect
{
    internal class EctResponseHandler : ResponseHandler
    {
        public EctResponseHandler(int timeout)
            : base(timeout)
        {
        }

        public IEctTransaction EctTransaction { get; set; }

        public bool HasBeenTimeout { get; private set; }

        public bool WaitOne()
        {
            var isResponseReceivedInTime = base.Wait();
            if (!isResponseReceivedInTime)
            {
                HasBeenTimeout = true;
            }

            return isResponseReceivedInTime;
        }
    }
}
