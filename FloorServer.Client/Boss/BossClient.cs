using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;

namespace FloorServer.Client.Boss
{
    /// <summary>
    /// BOSS client class.
    /// Handles single connection to BOSS server process.
    /// </summary>
    public class BossClient : IDisposable, IBossClient
	{

		#region Public members

		/// <summary>
        /// Default port used if no port is provided.
        /// </summary>
        public static readonly int DefaultPort = 1666;

        /// <summary>
        /// Default remote host used if no IP address or host name is provided.
        /// </summary>
        public static readonly string DefaultHost = "localhost";

        #endregion

        #region Protected members

        protected readonly ILogger<BossClient> _logger;

		protected readonly ClientName ClientName;

        private readonly BomFactory _bomFactory;


        protected Thread ReaderThread;
		protected bool StopNow;

		protected readonly IConnector Connector;

        #endregion

        #region Constructors

        /// <summary>
        /// Create <see cref="BossClient"/> instance to connect to given host/port.
        /// </summary>
        /// <param name="host">Host name or IP address to connect to.</param>
        /// <param name="port">Port number to connect to.</param>
        public BossClient(string host, int port, BomFactory bomFactory, ILoggerFactory loggerfactory)
        {
            _logger = loggerfactory.CreateLogger<BossClient>();
            _bomFactory = bomFactory;
            ClientName = new ClientName(this, _bomFactory, loggerfactory);
            Connector = new TcpConnector(host, port, loggerfactory);
            BossMessageReceived += ClientName.ConsumeLogonMessage;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name assigned by the BOSS messaging system.
        /// </summary>
        /// <value>
        /// Name (unique identfier) assigned by BOSS messaging system. Returns
        /// String.Empty in case we are not connected to the messaging system.
        /// </value>
        public String Name
        {
			get { return ClientName.Assigned; }
			private set { ClientName.RequestNameChange(this, value); }
        }

        /// <summary>
        /// Host name or IP address to connect to.
        /// </summary>
        public String Host
        {
            get { return Connector.Host; }
            set { Connector.Host = value; }
        }

        /// <summary>
        /// Port to connect to.
        /// </summary>
        public int Port
        {
            get { return Connector.Port; }
            set { Connector.Port = value; }
        }

        /// <summary>
        /// Send/Receive buffer size in bytes.
        /// </summary>
        public int BufferSize
        {
            get { return Connector.BufferSize; }
            set { Connector.BufferSize = value; }
        }

        /// <summary>
        /// enable/disable KeepAlive.
        /// 
        /// If KeepAlive is enabled you get an LinkDown event if host is not reachable any more after
        /// a certain time (timeout is set in registry 
        /// HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\Tcpip\\Parameters\\KeepAliveTime).
        /// </summary>
        public bool KeepAlive
        {
            get { return Connector.KeepAlive; }
            set { Connector.KeepAlive = value; }
        }

        #endregion

        #region Events

        /// <summary>
        /// Fired for every <see cref="Bom"/> received from host.
        /// </summary>
        public event EventHandler<BomEventArgs> BossMessageReceived;

        /// <summary>
        /// Fired for every <see cref="BossClient"/> status change.
        /// </summary>
        public event EventHandler<BossStatusEventArgs> BossStatusChanged;

        /// <summary>
        /// Announces reception of a Boss message.
        /// </summary>
        /// <param name="msg">Received data as <see cref="Bom"/></param>
        public virtual void RaiseBossMessageReceivedEvent(Bom msg)
        {
            RaiseEvent(BossMessageReceived, new BomEventArgs(msg));
        }

        /// <summary>
        /// Announces <see cref="BossClient"/> status change.
        /// </summary>
        /// <param name="evt"></param>
        public virtual void RaiseBossStatusChangeEvent(BossStatusEventArgs evt)
        {
            RaiseEvent(BossStatusChanged, evt);
        }

        private void RaiseEvent<T>(EventHandler<T> handler, T arguments) 
            where T : EventArgs
        {
            // Work with deep copy (delegates '=' operation provides deep copy) to prevent
            // conflicts with adding/removing handler during we raise the event.
            EventHandler<T> tmp = handler;
            if (tmp != null)
            {
                tmp(this, arguments);
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Starts tcp client and background thread for reader.
        /// </summary>
        public virtual void Start()
        {
            _logger.LogDebug("Start connection process for BOSS client [{0}]", Connector);

            StopNow = false;
            try
            {
                Connector.Connect();
            	ReaderThread = new Thread(Reader) {IsBackground = true};
            	ReaderThread.Start();
                RaiseBossStatusChangeEvent(new BossStatusEventArgs(BossEventType.LinkUp, "Client started"));
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to connect to BOSS: [{0}]", Connector);
                _logger.LogError("", ex);
                throw;
            }

            _logger.LogDebug("Connected to BOSS [{0}]", Connector);
        }

        /// <summary>
        /// Stops reader thread and tcp client.
        /// </summary>
        public virtual void Stop()
        {
            _logger.LogDebug("Requesting client thread to stop.");

            if (!StopNow)
            {
                StopNow = true;
                try
                {
                    
                    if (ReaderThread != null)
                    {
                        _logger.LogDebug("Waiting to join thread.");
                        ReaderThread.Join(500);
                        _logger.LogDebug("Thread joined - informing status event handlers about client stop.");
                    }

                    RaiseBossStatusChangeEvent(new BossStatusEventArgs(BossEventType.LinkDown, "Client stopped"));
                    _logger.LogDebug("Informed all status event handlers about client stop.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, ex);
                    RaiseBossStatusChangeEvent(new BossStatusEventArgs(ex));
                }
                finally
                {
                    Dispose(true);
                    _logger.LogDebug("Client stopped.");
                }
            }
        }

        /// <summary>
        /// Sends message through tcp socket.
        /// </summary>
        /// <param name="msg">Message to send.</param>
        public void Write(string msg)
        {
            _logger.LogTrace("OUT: [{0}]", msg);

            try
            {
                Connector.Writer.WriteLine(msg);
            }
            catch (Exception ex)
            {
                _logger.LogError("Write:", ex);
                RaiseBossStatusChangeEvent(new BossStatusEventArgs(ex));
            }
        }

        /// <summary>
        /// Sets client nameused for unique identification within BOSS messaging system.
        /// </summary>
        /// <remarks>
        /// <para>If we are already connected to BOSS setting a new name
        /// sends a name change request to BOSS. In case we are not connected setting
        /// the name has the effect to automatically send a name change request after
        /// successfully connecting to BOSS.</para>
        /// <para>Depending whether the rename was accepted by BOSS a status event
        /// change is created upon reception of the response to the rename request
        /// (BossEventType.NewName or BossEventType.NameChangeRejected).
        /// </para></remarks>
        /// <exception cref="System.ArgumentException">When setting Name to <c>String.Empty</c>.</exception>
        /// <exception cref="System.ArgumentNullException">When setting Name to <c>Null</c>.</exception>
        /// <param name="newName">The new name.</param>
        public void SetName(string newName)
        {
            Name = newName;
        }

        /// <summary>
        /// Reader job
        /// </summary>
        public virtual void Reader()
        {
        	_logger.LogDebug("BOSS client thread [{0}] started", Connector);

            while (!StopNow)
            {
                try
                {
                    string line = Connector.Reader.ReadLine();
                    if (line == null)
                    {
                       // connection closed by host
                        Stop();
                        StopNow = true;
                        continue; 
                    }

					_logger.LogTrace(string.Format(" IN: [{0}]", line));
                    
					// TODO what about encoding?
                    Bom msg = _bomFactory.CreateBom(line);
                    // Do not send empty messages
                    if (msg.Count == 0)
                    {
                        _logger.LogWarning("Got empty message ,{0}>", line);
                        continue;
                    }
                    RaiseBossMessageReceivedEvent(msg);
                }
                catch (IOException ioe)
                {
                    if (!StopNow)
                    {
                        _logger.LogError(ioe.Message);
                        RaiseBossStatusChangeEvent(new BossStatusEventArgs(ioe));
                    }
                    if (!Connector.Connected)
                    {
                        Stop();
                    }
                    // Ignore IOException if stopping the reader
                }
				// catching exception for threading context is OK
                catch (Exception ex)
                {
                    _logger.LogError("", ex);
                    RaiseBossStatusChangeEvent(new BossStatusEventArgs(ex));
                }
            }

            _logger.LogDebug("Leaving BOSS client thread [{0}]", Connector);

        }

		#endregion

		#region IDisposable Members

		/// <summary>
        ///Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        /// <remarks>Since version 1.1.0</remarks>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// destructor! called, if GarbageCollector collect this object
        /// </summary>
        ~BossClient()
        {
            Dispose(false);
        }

        private bool _alreadyDisposed;

    	private void Dispose(bool calledThroughDispose)
        {
            if (_alreadyDisposed)
                return;

			ClientName.Reset();

            if (calledThroughDispose && Connector.Connected)
            {
                Connector.Close();
                _logger.LogInformation("Closed connection to [{0}]", Connector);
            }

            //for all not-IDisposable objects:

            _alreadyDisposed = true;
        }

        #endregion

    }
}
