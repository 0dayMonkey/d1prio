#region

using System;
using FloorServer.Client.Enums;
using FloorServer.Client.Tools;

#endregion

namespace FloorServer.Client
{
    public interface IFloorJackpotClient : IDisposable
    {
        #region events

        event EventHandler<FloorQueryArgs> QueryResponseReceived;

        #endregion

        ConnectionStatus Status
        { get; }

        /// <summary>
        /// Sends a query to the Floor to find one jackpot information.
        /// </summary>
        FloorServerQueryReader JackpotInfosQuery(string jackpotGroup);
       
    }
}
