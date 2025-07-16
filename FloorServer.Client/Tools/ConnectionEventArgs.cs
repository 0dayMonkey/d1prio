#region

using System;
using FloorServer.Client.Enums;

#endregion

namespace FloorServer.Client.Tools
{
    public class ConnectionStatusEventArgs : EventArgs
    {
        private ConnectionStatus status;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionStatusEventArgs"/> class.
        /// </summary>
        /// <param name="connected">if set to <c>true</c> [connected].</param>
        public ConnectionStatusEventArgs(ConnectionStatus status)
        {
            this.status = status;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ConnectionEventArgs"/> is connected.
        /// </summary>
        /// <value><c>true</c> if connected; otherwise, <c>false</c>.</value>
        public ConnectionStatus Status
        {
            get { return status; }
        }
    }
}
