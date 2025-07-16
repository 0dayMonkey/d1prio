using Microsoft.Extensions.Logging;
using NotificationService.Infrastructure.RTDS.KeyValueTools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.RTDS
{
    public abstract class RTDSProxy : BackgroundWorker, IDisposable
    {
        private readonly ILogger<RTDSProxy> _logger;

        protected TcpClient? _tcpClient;
        protected StreamReader? _reader;
        protected StreamWriter? _writer;

        protected ConnectionState _state;

        protected const int DefaultTimeout = 5000;
        protected const int ReconnectionDelay = 10000;
        protected int _currentSequence;
        protected string? _currentName;

        protected TaskCompletionSource<bool>? _tcsInitialization;
        protected TaskCompletionSource<bool>? _tcsSendingMessage;


        protected RTDSProxy(ILogger<RTDSProxy> logger)
        {
            _state = ConnectionState.None;
            _logger = logger;
        }

        public void Close()
        {
            _tcpClient?.Close();
        }

        public async Task<bool> Open()
        {
            await TryOpenConnection();

            DoWork += RTDSServerProxy_DoWork;
            _tcsInitialization = new TaskCompletionSource<bool>();

            RunWorkerAsync();
            Task timeoutTask = Task.Delay(DefaultTimeout);
            await Task.WhenAny(_tcsInitialization.Task, timeoutTask);

            if (timeoutTask.IsCompleted)
            {
                _logger.LogError("RTDS server did not respond to init procedure on time");
                return false;
            }

            return _tcsInitialization.Task.Result;
        }


        private void SendRename(string sequence)
        {
            if (_writer == null)
            {
                _logger.LogError("Writer not initialized");
                return;
            }
            var resonse_sequence = GetResponseSequence(sequence);
            var response = new KeyValueMap();
            response.AddStrings("MSG_TYPE", "RENAME");
            string newName = $"NOTIFICATION_SERVICE*{Guid.NewGuid()}";
            _currentName = newName;
            response.AddStrings("MY_NAME", newName);
            response.AddStrings("SEQ_NBR", resonse_sequence);
            _writer.Write(response.ToString() + "\r");
            _writer.Flush();
        }


        private async void RTDSServerProxy_DoWork(object? sender, DoWorkEventArgs e)
        {
            if (_reader == null)
            {
                _logger.LogError("reader is null");
                throw new InvalidOperationException("reader is null");
            }
            while (true)
            {
                try
                {
                    var line = _reader.ReadLine();
                    if (line == null)
                    {
                        break;
                    }
                    var message = Parser.Parse(line);
                    var messageType = message.GetStringValue("MSG_TYPE");
                    var sequence = message.GetStringValue("SEQ_NBR");

                    if (messageType == null || sequence == null)
                    {
                        _logger.LogWarning("Invalid message from RTDSServer " + line);
                        continue;
                    }

                    switch (_state)
                    {
                        case ConnectionState.Opening:
                            if (messageType.Equals("HELLO"))
                            {
                                _state = ConnectionState.Renaming;
                                SendRename(sequence);
                            }
                            break;
                        case ConnectionState.Renaming:
                            if (messageType.Equals("ACK"))
                            {
                                _state = ConnectionState.Initialized;
                                _tcsInitialization?.TrySetResult(true);
                            }
                            break;
                        case ConnectionState.Sending:
                            if (messageType.Equals("ACK"))
                            {
                                _state = ConnectionState.Initialized;
                                _tcsSendingMessage?.TrySetResult(true);
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning($"Disconnection from RTDS-server{ex.Message}");
                    await TryOpenConnection();
                    _logger.LogInformation("Connection up again");
                }
            }
        }


        protected abstract string Server_address { get; }
        protected abstract string Server_port { get; }

        private static string GetResponseSequence(string sequence)
        {
            var id = sequence[1..];
            return $"C{id}";
        }


        protected async Task<bool> ConnectAsync()
        {
            _tcpClient = new TcpClient();

            var addr = Server_address;
            var portSt = Server_port;

            if (addr == null)
            {
                _logger.LogError("No defined ip address");
                return false;
            }
            if (portSt == null)
            {
                _logger.LogError("No port definied");
                return false;
            }
            if (!int.TryParse(portSt, out var port))
            {
                _logger.LogError("Cannot parse the port number for RTDS server");
                return false;
            }
            var connectTask = _tcpClient.ConnectAsync(addr, port);
            var timeoutTask = Task.Delay(DefaultTimeout);
            await Task.WhenAny(connectTask, timeoutTask);
            if (timeoutTask.IsCompleted)
            {
                _logger.LogError($"Connection time-out on {addr}");
                return false;
            }
            if (connectTask.IsFaulted)
            {
                _logger.LogError("Fail to connect to RTDS-Server");
                return false;
            }
            if (_tcpClient == null)
            {
                _logger.LogError("Invalid operation : no connection with RTDS server");
                return false;
            }
            var stream = _tcpClient.GetStream();
            _reader = new StreamReader(stream);
            _writer = new StreamWriter(stream);
            _state = ConnectionState.Opening;
            return true;
        }

        protected async Task TryOpenConnection()
        {
            _state = ConnectionState.None;
            _currentSequence = 1;
            while (true)
            {
                var connectionResult = await ConnectAsync();
                if (connectionResult) return;
                _logger.LogWarning("Connection fail, will retry in a short delay");
                await Task.Delay(ReconnectionDelay);
            }
        }

    }
}
