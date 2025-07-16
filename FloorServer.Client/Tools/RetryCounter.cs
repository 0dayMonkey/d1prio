#region

using System;

#endregion

namespace FloorServer.Client.Tools
{
    public class RetryCounter
    {
        private int retryNumber;
        private DateTime timestamp;
        private bool markedForDeletion;

        public bool MarkedForDeletion
        {
            get { return markedForDeletion; }
            set { markedForDeletion = value; }
        }


        public RetryCounter(DateTime timestamp)
        {
            this.timestamp = timestamp;
        }


        public int RetryNumber
        {
            get { return retryNumber; }
            set { retryNumber = value; }
        }

        public DateTime Timestamp
        {
            get { return timestamp; }
            set { timestamp = value; }
        }
    }
}
