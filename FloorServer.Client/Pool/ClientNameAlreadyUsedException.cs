using System;

namespace FloorServer.Client.Pool
{
    public class ClientNameAlreadyUsedException : Exception
    {
        public ClientNameAlreadyUsedException(string clientName)
        {
            ClientName = clientName;
        }

        public string ClientName { get; set; }
    }
}