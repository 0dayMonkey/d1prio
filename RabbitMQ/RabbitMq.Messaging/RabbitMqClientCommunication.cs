using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMq.Messaging
{

    public abstract class RabbitMqClientCommunication : IDisposable
    {
        protected bool _disposedValue;

        protected IConnection? Connection { get; private set; }
        protected IModel? Channel { get; private set; }
        protected RabbitMqConfiguration Configuration { get; set; }

        protected CancellationTokenSource CancellationTokenSource { get; set; }

        protected ILogger Logger;

        protected RabbitMqClientCommunication(RabbitMqConfiguration conf, ILogger logger)
        {
            Logger = logger;
            CancellationTokenSource = new CancellationTokenSource();
            Configuration = conf;
        }

        protected void CreateConnectionAsync(RabbitMqConfiguration conf)
        {
            var factory = new ConnectionFactory()
            {
                UserName = conf.UserName,
                Password = conf.Password,
                VirtualHost = conf.VirtualHost,
                NetworkRecoveryInterval = conf.NetworkRecoveryInterval,
                AutomaticRecoveryEnabled = conf.AutomaticRecoveryEnabled,
                RequestedHeartbeat = conf.RequestedHeartbeat,
                Ssl = conf.SslOptions,
            };

            try
            {
                Connection = factory.CreateConnection(new List<AmqpTcpEndpoint> { new AmqpTcpEndpoint(conf.Uri, conf.SslOptions) });
                Channel = Connection.CreateModel();
                Logger.LogInformation("Connected to the RabbitMq broker at {0}", conf.Uri);
                ChannelRegistration(Channel);
            }
            catch (BrokerUnreachableException e)
            {
                Logger.LogError(e, "Impossible to connect to the RabbitMq broker at {0}", conf.Uri);
                if (!CancellationTokenSource.Token.IsCancellationRequested)
                {
                    var t = Task.Delay(conf.NetworkRecoveryInterval, CancellationTokenSource.Token)
                        .ContinueWith((CancellationToken) => CreateConnectionAsync(conf));
                    t.Wait();
                }
            }
        }

        protected abstract void ChannelRegistration(IModel channel);

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    CancellationTokenSource.Cancel(false);
                    if (Channel != null)
                    {
                        Channel.Dispose();
                    }

                    if (Connection != null)
                    {
                        Connection.Dispose();
                    }
                }
                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
