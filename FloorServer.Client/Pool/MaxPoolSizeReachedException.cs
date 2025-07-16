#region

using System;

#endregion

namespace FloorServer.Client.Pool
{
    [global::System.Serializable]
    public class MaxPoolSizeReachedException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public MaxPoolSizeReachedException() { }
        public MaxPoolSizeReachedException(string message) : base(message) { }
        public MaxPoolSizeReachedException(string message, Exception inner) : base(message, inner) { }
        protected MaxPoolSizeReachedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
