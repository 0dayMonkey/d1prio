#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FloorServer.Client.Configuration;
using FloorServer.Client.Pool;
using Microsoft.Extensions.Logging;

#endregion

namespace FloorServer.Client
{
    public class FloorServerClientPool : IFloorClientPool
    {
        #region Members

        private int _sequence;
#if DEBUG
        private const int DefaultNumberOfClients = 1;
#else
        private const int DefaultNumberOfClients = 5;
#endif
        private int _nbClient;
        private readonly List<FloorServerPooledClient> _anonymousClients = new List<FloorServerPooledClient>();
        private readonly List<FloorServerPooledClient> _namedClients = new List<FloorServerPooledClient>();
        private readonly FloorServerConfiguration _configuration;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger _logger;
        private readonly object _syncRoot = new object();
        private const string RegExp = @"([]()[$#{""}\\\=])";

        private bool _isDisposed = false;

        #endregion

        public FloorServerClientPool(FloorServerConfiguration configuration, ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
            _logger = _loggerFactory.CreateLogger<FloorServerClientPool>();
            _configuration = configuration;
        }

        #region Implementation of IFloorClientPool

        /// <summary>
        /// 
        /// </summary>
        public int MaxClients
        {
            get { return 50; }
        }


        public IFloorClient GetClient(string clientName = null)
        {
            if (_isDisposed)
                throw new ObjectDisposedException("FloorServerClientPool");

            //Due to floorserver limitation we can't allow some characters in client name.
            var reg = new Regex(RegExp);
            if (clientName != null && reg.IsMatch(clientName))
                throw new ArgumentException("Invalid character in clientName");

            FloorServerPooledClient client;
            lock (_syncRoot)
                client = !string.IsNullOrWhiteSpace(clientName)
                             ? _namedClients.FirstOrDefault(c => c.ClientName == clientName)
                             : _anonymousClients.FirstOrDefault(c => c.Free);

            if (client == null)
            {
                _logger.LogDebug("No client has been found -> Creating new connection ...");

                client = CreateClient(clientName);
            }
            else if (!client.Free)
            {
                _logger.LogError("The client has been found with the name {0} but is not available ...", clientName);
                throw new ClientNameAlreadyUsedException(clientName);

            }

            client.MarkAsUsed();
            return client;
        }


        public void Initialize(int nbClient)
        {
            _nbClient = nbClient;
            _logger.LogDebug("Initializing FloorServerClientPool -> Number of clients to be created = {0}", _nbClient);

            while (_anonymousClients.Count + _namedClients.Count < _nbClient)
                CreateClient();
        }

        public void Initialize()
        {
            Initialize(DefaultNumberOfClients);
        }

        #endregion

        #region Internal Methods

        internal void Release(FloorServerPooledClient c)
        {
            c.MarkAsReleased();
        }

        #endregion

        #region Private Methods

        private FloorServerPooledClient CreateClient(string clientName = null)
        {
            if (_anonymousClients.Count + _namedClients.Count > MaxClients)
                throw new MaxPoolSizeReachedException(string.Format("The maximum number of clients has been reached (MAX = {0})", MaxClients));

            var name = clientName ?? string.Concat(_configuration.DefaultClientName, string.Format("_P{0}", _sequence));
            var c = new FloorServerPooledClient(this, _configuration, _loggerFactory, name);

            if (string.IsNullOrWhiteSpace(clientName))
            {
                c.Connect();
                _sequence++;
                AddAnonymousClient(c);
            }
            else
            {
                _sequence++;
                AddNamedClient(c);
            }

            _logger.LogDebug("New client created with name '{0}' -> Current number of clients = {1}", name, _nbClient);

            return c;
        }

        private void AddAnonymousClient(FloorServerPooledClient c)
        {
            lock (_syncRoot)
                _anonymousClients.Add(c);
        }

        private void AddNamedClient(FloorServerPooledClient c)
        {
            lock (_syncRoot)
                _namedClients.Add(c);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            lock (_syncRoot)
                _anonymousClients.ForEach(c => c.Dispose(false));
            _anonymousClients.Clear();

            lock (_syncRoot)
                _namedClients.ForEach(c => c.Dispose(false));
            _namedClients.Clear();

            _isDisposed = true;
        }

        #endregion
    }
}
