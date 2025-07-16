using System;

namespace FloorServer.Client.Boss
{
    /// <summary>
    /// Holds arguments for Boss message event.
    /// </summary>
    public class BomEventArgs : EventArgs
    {
        /// <summary>
        /// Constructor, what else?
        /// </summary>
        /// <param name="message">event data</param>
        public BomEventArgs(Bom message)
        {
            Message = message;
        }

        /// <summary>
        /// Gets/sets events data.
        /// </summary>
        public Bom Message { get; private set; }
    }
}