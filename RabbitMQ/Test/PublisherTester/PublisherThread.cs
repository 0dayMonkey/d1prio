using Microsoft.Extensions.Logging;
using RabbitMq.Messaging;
using RabbitMq.Messaging.Publisher;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace PublisherTester
{
    public class PublisherThread
    {
        private List<object> _messages = new List<object>();
        private List<int> _machineIds;
        private int _idx;
        private int _cpt = 0;
        private ILoggerFactory _logger;

        private List<string> _ackMessageIds = new List<string>();

        private RabbitMqPublisher _publisher;

        public bool Running { get; set; }
        public int SentMessageCount { get { return _cpt; } }

        public PublisherThread(List<int> machineIds, int idx, ILoggerFactory logger)
        {
            _machineIds = machineIds;
            _idx = idx;
            _logger = logger;
            Running = true;

            new Thread(Run).Start();
        }

        int idx = 1;

        public void PopulateQueue(int num)
        {
            var random = new Random();
            lock (_messages)
            {
                foreach (var id in _machineIds)
                {
                    for (int i = 1; i <= num; i++)
                    {
                        var card = random.Next(1, 999999999);
                        _messages.Add(new CardMessage { CasinoId = _idx, MachineId = id, CardMessageType = CardMessageType.cardin, CardNum = card });
                        _messages.Add(new CardMessage { CasinoId = _idx, MachineId = id, CardMessageType = CardMessageType.cardout, CardNum = card });

                        //_messages.Add(idx++);
                        //_messages.Add(idx++);
                    }
                }
            }
        }

        private void Run()
        {
            var conf = new RabbitMqConfiguration
            {
                Uri = new Uri("amqp://localhost:5672"),
                UserName = "galaxis",
                Password = "ttms",
                AutomaticRecoveryEnabled = true,
                VirtualHost = "/",
            };

            using (_publisher = new RabbitMqPublisher(conf, exchangeName: "test.topic", exchangeType: ExchangeType.Topic, durable: true, reliable: true, autoDelete: false, _logger.CreateLogger<RabbitMqPublisher>()))
            {
                _publisher.OnMessagesPublished += (messageIds) => {
                    if (messageIds.Length == 0)
                    {
                        Console.Write(0);
                    }
                    else
                    { 
                        _ackMessageIds.AddRange(messageIds);
                        Console.Write(".".PadLeft(messageIds.Length, '.'));
                    }
                };

                while (Running)
                {
                    object msg = null;
                    lock (_messages)
                    {
                        if (_messages.Count > 0)
                        {
                            msg = _messages.First();
                            _messages.Remove(msg);
                        }
                    }
                    if (msg != null)
                    {
                        var routingKey = msg is CardMessage ? $"slot.{_idx}_{(msg as CardMessage).MachineId}.{(msg as CardMessage).CardMessageType}" : "test.ordered";
                        var consistenthashing = msg is CardMessage ? $"{_idx}_{(msg as CardMessage).MachineId}" : null;
                        _publisher.Publish(msg, routingKey, Guid.NewGuid().ToString(), consistenthashing);
                        _cpt++;
                    }
                    else
                    {
                        Thread.Sleep(100);
                    }
                }
            }
        }

        public void DisplayInfo()
        {
            Console.WriteLine("Published messages: {0}, acknowleged: {1}", _cpt, _ackMessageIds.Count);
        }

        internal void ClearInfo()
        {
            _cpt = 0;
            _ackMessageIds.Clear();
        }
    }
}
