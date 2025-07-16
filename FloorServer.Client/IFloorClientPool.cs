#region

using System;

#endregion

namespace FloorServer.Client
{
    public interface IFloorClientPool : IDisposable
    {
        void Initialize();
        void Initialize(int nbClient);
        int MaxClients { get; }
        IFloorClient GetClient(string clientName = null);
    }
}