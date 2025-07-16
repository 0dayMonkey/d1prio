#region

using System.Net;
using System.Xml.Serialization;

#endregion

namespace FloorServer.Client.Configuration
{
    public class FloorServerConfiguration
    {
        #region Constants

        public const string FloorServerClientNameKey = "FloorServer_ClientName";
        public const string FloorServerReconnectionDelayKey = "FloorServer_ReconnectionDelay";
        public const string FloorServerQueryTimeoutKey = "FloorServer_QueryTimeout";
        public const string FloorServerConnectionTimeoutKey = "FloorServer_ConnectionTimeout";
        public const string FloorServerWinsizeKey = "FloorServer_Winsize";

        #endregion

        #region Members

        private string _clientName = string.Empty;
        private int _reconnectionDelay = 5000;
        private int _connectionTimeOut = 1000;
        private int _queryTimeout = 5000;
        private IPEndPoint _endpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1666);

        #endregion

        #region Constructors


        /// <summary>
        /// Initializes a new instance of the <see cref="FloorServerConfiguration"/> class.
        /// </summary>
        public FloorServerConfiguration()
        {
        }


        #endregion

        #region Properties


        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>The address.</value>
        public IPEndPoint EndPoint
        {
            get { return _endpoint; }
            set
            {
                _endpoint = value;
            }
        }

        public int QueryTimeout
        {
            get { return _queryTimeout; }
            set { _queryTimeout = value * 1000; }
        }

        /// <summary>
        /// Gets or sets the name of the client.
        /// </summary>
        /// <value>The name of the client.</value>
        [XmlElement()]
        public string DefaultClientName
        {
            get { return _clientName; }
            set { _clientName = value; }
        }

        [XmlElement]
        public int ReconnectionDelay
        {
            get { return _reconnectionDelay; }
            set { _reconnectionDelay = value * 1000; }
        }

        public int ConnectionTimeOut
        {
            get { return _connectionTimeOut; }
            set { _connectionTimeOut = value; }
        }

        public int? Winsize { get; set; }

        #endregion

    }
}
