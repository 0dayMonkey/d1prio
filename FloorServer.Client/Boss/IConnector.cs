using System;
using System.IO;

namespace FloorServer.Client.Boss
{

    internal struct HostInfo
    {
        public readonly string Name;
        public readonly int Port;
        internal HostInfo(string hostNameOrAddress, int port)
        {
            Name = hostNameOrAddress;
            Port = port;
        }

        public override string ToString()
        {
            return String.Format("{0}:{1}", Name, Port);
        }
    }

    /// <summary>
    /// TODO describe interface
    /// </summary>
    public interface IConnector
    {

        string Host
        {
            get;
            set;
        }

        int Port
        {
            get;
            set;
        }

        int BufferSize
        {
            get;
            set;
        }

        bool KeepAlive
        {
            get;
            set;
        }

        bool Connected
        {
            get;
        }

        /// <summary>
        /// Returns a TextReader instance.
        /// </summary>
        /// <value>TextReader instance used to read from the underlying stream.</value>
        TextReader Reader
        {
            get;
        }

        TextWriter Writer
        {
            get;
        }

        void AddHost(string hostNameOrAddress, int port);

        void Connect();

        void Close();

    }

}
