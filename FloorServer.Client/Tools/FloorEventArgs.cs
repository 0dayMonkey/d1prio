#region

using System;
using System.Collections.Specialized;

#endregion

namespace FloorServer.Client.Tools
{
    public class FloorEventArgs : EventArgs
    {
        private long exception;
        private StringDictionary values;
        public DateTime TimestampUTC { get; private set; }

        public bool CancelMessage { get; set; }

        public long Exception
        {
            get { return exception; }
        }

        public long SequenceNumber { get; set; }

        public StringDictionary Values
        {
            get
            {
                if (values == null)
                    values = new StringDictionary();
                return values;
            }
        }

        public FloorEventArgs(long exception, long seq)
        {
            SequenceNumber = seq;
            this.exception = exception;
            CancelMessage = false;
        }

        public FloorEventArgs(long exception, StringDictionary values)
            : this(exception, values, 0)
        {            
        }

        public FloorEventArgs(long exception, StringDictionary values, long seq)
            : this(exception, seq)
        {
            this.values = values;
        }

        public FloorEventArgs(long exception, StringDictionary values, long seq, DateTime timestamp)
            : this(exception, values, seq)
        {
            this.TimestampUTC = timestamp;
        }
    }
}
