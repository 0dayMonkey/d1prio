using System;
using System.Net.Sockets;
using System.IO;
using System.Net;
using Microsoft.Extensions.Logging;

namespace FloorServer.Client.Boss
{
    public class TcpConnector : IConnector
    {
        internal static readonly HostInfo DefaultHost = new HostInfo("localhost", 1666);

        #region Properties

        /// <summary>
        /// Host name or IP address to connect to.
        /// </summary>
        public string Host
        {
            get { return host.Name; }
            set
            {
                if (Connected)
                {
                    throw new InvalidOperationException("Unable to change remote host when connected");
                }
                host = new HostInfo(value, host.Port);
            }
        }

        /// <summary>
        /// Port to connect to.
        /// </summary>
        public int Port
        {
            get { return host.Port; }
            set
            {
                if (Connected)
                {
                    throw new InvalidOperationException("Unable to to change port when connected");
                }
                host = new HostInfo(host.Name, value);
            }
        }

        public bool Connected
        {
            get {
                if (client == null)
                    return false;
                return client.Connected;
            }
        }

        public TextReader Reader
        {
            get
            {
                if (reader == null)
                    // 1st we need a successfull connect!!!
                    throw new InvalidOperationException();
                return reader;
            }
        }

        /// <summary>
        /// Gets the writer.
        /// </summary>
        /// <value>The writer.</value>
        public TextWriter Writer
        {
            get
            {
                if (writer == null)
                    // 1st we need a successfull connect!!!
                    throw new InvalidOperationException();
                return writer;
            }
        }

        /// <summary>
        /// Send/Receive buffer size in bytes.
        /// </summary>
        public int BufferSize
        {
            get { return bufferSize; }
            set
            {
                bufferSize = value;
                if (client != null)
                {
                    client.SendBufferSize = bufferSize;
                    client.ReceiveBufferSize = bufferSize;
                }
            }
        }

        /// <summary>
        /// enable/disable KeepAlive.
        /// 
        /// If KeepAlive is enabled you get an LinkDown event if host is not reachable any more after
        /// a certain time (timeout is set in registry 
        /// HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\Tcpip\\Parameters\\KeepAliveTime).
        /// 
        /// </summary>
        public bool KeepAlive
        {
            get { return keepAlive; }

            set
            {
                keepAlive = value;
                if (client != null)
                    client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, value);
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TcpConnector"/> class.
        /// </summary>
        /// <param name="hostNameOrAddress">DNS name or IP address of the 
        /// remote host to which we intend to connect.</param>
        /// <param name="port">Port number of the remote host to which we 
        /// intend to connect.</param>
        /// <remarks>Validation of the provided parameters is done when connecting
        /// to the remote host. This lazy validation is provided to prevent a time 
        /// consuming DNS check in case no connection is made at all.</remarks>
        public TcpConnector(string hostNameOrAddress, int port, ILoggerFactory loggerfactory)
        {
            _logger = loggerfactory.CreateLogger<TcpConnector>();
            host = new HostInfo(hostNameOrAddress, port);
        }

        #endregion

        #region Public methods

        public override string ToString()
        {
            return host.ToString();
        }

        public void AddHost(string hostNameOrAddress, int port)
        {
            host = new HostInfo(hostNameOrAddress, port);
        }

        /// <summary>
        /// Connects to the remote host specified in the constructor.
        /// </summary>
        /// <exception cref="InvalidOperationException">If the connector is
        /// is already connected to a host.</exception>
        /// <exception cref="ArgumentNullException"><c>hostNameOrAddress</c>
        /// is a null reference</exception>
        /// <exception cref="ArgumentOutOfRangeException">The lenght of 
        /// <c>hostNameOrAddress</c> is greater than 126 characters or 
        /// <c>port</c> is less than MinPort or greater than MaxPort.
        /// </exception>
        /// <exception cref="SocketException">An error is encountered when 
        /// resolving <c>hostNameOrAddress</c> or when accessing the socket.
        /// </exception>
        /// <exception cref="ArgumentException"><c>hostNameOrAddress</c> is an 
        /// invalid IP address.</exception>
        public void Connect()
        {
            if (client != null)
                throw new InvalidOperationException("already connected");
            // This only binds the IP end point, but no connection is established
            client = new TcpClient();
            _logger.LogDebug("Connection attempt to [{0}]", this);
            client.Connect(Dns.GetHostAddresses(host.Name)[0], host.Port);
            _logger.LogDebug("Connected to [{0}]", this);
            client.SendBufferSize = BufferSize;
            client.ReceiveBufferSize = BufferSize;
            // FIXME set timeout
            client.SendTimeout = 50000;
            //tcpClient.ReceiveTimeout = 10000;
            NetworkStream nwStream = client.GetStream();
            //nwStream.ReadTimeout = 2000;
            reader = new StreamReader(nwStream);
            writer = new StreamWriter(nwStream);
            writer.AutoFlush = true;
        }

        public void Close()
        {
            _logger.LogDebug("Closing connection to [{0}]", this);

            // Closing a TextWriter/TextReader closes automatically the 
            // underlying (network-) stream.
            if (reader != null)
            {
                reader.Close();
                reader = null;
            }

            if (writer != null)
            {
                writer.Close();
                writer = null;
            }

            if (client != null)
            {

                client.Close();
                client = null;
            }
        }

        #endregion

        private readonly ILogger<TcpConnector> _logger;

        private HostInfo host;
        private TcpClient client = null;
        private int bufferSize = 4096;
        private bool keepAlive = false;
        private TextReader reader = null;
        private StreamWriter writer = null;
        
    }

}
