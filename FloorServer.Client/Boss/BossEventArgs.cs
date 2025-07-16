using System;

namespace FloorServer.Client.Boss
{
    /// <summary>
    /// Event type enumeration.
    /// </summary>
    public enum BossEventType
    {
        /// <summary>
        /// Boss link up.
        /// Message contains ip address.
        /// </summary>
        LinkUp = 0,
        /// <summary>
        /// Boss link down.
        /// Message contains ip address.
        /// </summary>
        LinkDown = 1,
        /// <summary>
        /// ERROR
        /// Message contains error text.
        /// </summary>
        Error = 3,
        /// <summary>
        /// Boss client got new name ( TYP=.1.)
        /// Message contains new name (DST field)
        /// </summary>
        NewName = 4,
        /// <summary>
        /// BOSS rejected the request to rename the client
        /// </summary>
        NameChangeRejected = 5,
    }

    /// <summary>
    /// </summary>
    public class BossStatusEventArgs : EventArgs
    {
        /// <summary>
        /// Event type.
        /// </summary>
        public readonly BossEventType Type;

        /// <summary>
        /// Event message.
        /// May bee empty.
        /// </summary>
        public readonly string Message;

        /// <summary>
        /// Create <see cref="BossStatusEventArgs"/> from type and message.
        /// </summary>
        /// <param name="type">Type of event</param>
        /// <param name="message">Event data</param>
        public BossStatusEventArgs(BossEventType type, string message)
        {
            Type = type;
            Message = message;
        }

        /// <summary>
        /// Create <see cref="BossStatusEventArgs"/> object from Exception
        /// with event type set to <see cref="BossEventType.Error"/>
        /// </summary>
        /// <param name="ex">Exception to create event from.</param>
        public BossStatusEventArgs(Exception ex)
        {
            Type = BossEventType.Error;
            Message = ex.Message;
        }
    }
}