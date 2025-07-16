using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using RabbitMq.Messaging;
using RabbitMq.Messaging.Consumer;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsumerTester
{
    class Program
    {
        static int cpt = 0;
        static int ack = 0;
        static int dbl = 0;
        static int exc = 0;
        static int last = 0;

        static Dictionary<int, long> cardInserted = new Dictionary<int, long>();
        static Dictionary<int, int> egmMessageCount = new Dictionary<int, int>();
        static List<ulong> recievedTags = new List<ulong>();

        static Dictionary<int, string> messageIds = new Dictionary<int, string>();

        static void Main(string[] args)
        {

            IHost host = Host.CreateDefaultBuilder(args).Build();

            var loggerFactory = host.Services.GetRequiredService<ILoggerFactory>();

            var queueIdx = args.Length > 0 ? args[0] : null;

            Console.WriteLine("CONSUMER {0}", queueIdx);

            var conf = new RabbitMqConfiguration
            {
                Uri = new Uri("amqp://localhost:5672"),
                UserName = "external",
                Password = "ext",
                AutomaticRecoveryEnabled = true,
                VirtualHost = "/",
            };

            using (var consumer = new RabbitMqConsumer(conf, prefetch: 10, useDeduplication: true, loggerFactory.CreateLogger<RabbitMqConsumer>()))
            {
                consumer.AddQueue("glx.test", "galaxis.external", new[] {
                        new Subscription<CardMessage>("*.*.*", HandleMessage) },
                    durable: false, exclusive: true, autoDelete: true, autoAck: true);

                var cmd = Console.ReadLine().ToLower();
                while (!cmd.Equals("q"))
                {
                    switch (cmd)
                    {
                        case "i":
                            DisplayInfo();
                            break;

                        case "r":
                            ResetInfo();
                            break;
                    }
                    cmd = Console.ReadLine().ToLower();
                }
            }

            /*var conf = new RabbitMqConfiguration
            {
                Uri = new Uri("amqp://localhost:5672"),
                UserName = "galaxis",
                Password = "ttms",
                AutomaticRecoveryEnabled = true,
                VirtualHost = "/",
            };

            using (var consumer = new RabbitMqConsumer(conf, prefetch: 10, useDeduplication: true, loggerFactory.CreateLogger<RabbitMqConsumer>()))
            {
                consumer.AddExchangeMapping("test.topic", new[] { "slot.*.cardin", "slot.*.cardout" }, "test.consistenthash", ExtendedExchangeType.ConsistentHash, true, false);

                //consumer.AddQueue("Test_ordered" + queueIdx, new[] { new Subscription<int>("test.ordered", HandleIntegerMessage) },
                //    durable: true, exclusive: false, autoDelete: false, autoAck: false, hashWeight: "1");

                consumer.AddQueue("Test" + queueIdx, "test.consistenthash", new[] {
                        new Subscription<CardMessage>("slot.*.cardin", HandleCardIn),
                        new Subscription<CardMessage>("slot.*.cardout", HandleCardOut) },
                    durable: true, exclusive: false, autoDelete: false, autoAck: false, hashWeight: "1");

                var cmd = Console.ReadLine().ToLower();
                while (!cmd.Equals("q"))
                {
                    switch (cmd)
                    {
                        case "i":
                            DisplayInfo();
                            break;

                        case "r":
                            ResetInfo();
                            break;
                    }
                    cmd = Console.ReadLine().ToLower();
                }
            }*/
        }

        private static void HandleMessage(IRabbitMqConsumer arg1, CardMessage arg2, MessageEnvelope arg3)
        {
            Console.WriteLine("Received: {0}", arg3.RoutingKey);
        }

        public static void DisplayInfo()
        {
            Console.WriteLine();
            Console.WriteLine("Received: {0} (ack:{1}, no ack: {2}, exc:{3})                    ", cpt, ack, cpt - ack, exc);
            Console.WriteLine("Number of EGM: {0}", egmMessageCount.Count);

            KeyValuePair<int, int> maxPair = new KeyValuePair<int, int>(0, 0);
            KeyValuePair<int, int> minPair = new KeyValuePair<int, int>(0, int.MaxValue);
            foreach (var pair in egmMessageCount)
            {
                if (pair.Value > maxPair.Value)
                    maxPair = pair;
                if (pair.Value < minPair.Value)
                    minPair = pair;
                //Console.WriteLine("   egm {0} = {1}", pair.Key, pair.Value);
            }
            Console.WriteLine("   min: egm {0} = {1}", minPair.Key, minPair.Value);
            Console.WriteLine("   max: egm {0} = {1}", maxPair.Key, maxPair.Value);
        }

        public static void DisplayMessageReceived()
        {
            var pos = Console.GetCursorPosition();
            Console.SetCursorPosition(50, 0);
            Console.Write(cpt.ToString().PadLeft(10));
            Console.SetCursorPosition(pos.Left, pos.Top);
        }

        public static void ResetInfo()
        {
            cpt = 0;
            ack = 0;
            dbl = 0;
            exc = 0;
            last = 0;
            cardInserted.Clear();
            egmMessageCount.Clear();
            recievedTags.Clear();
        }

        public static void HandleIntegerMessage(IRabbitMqConsumer consumer, int msg, MessageEnvelope envelope)
        {
            try
            {
                cpt++;
                //Console.Write(" {0}", msg);
                //if (last > msg)
                //{
                //    Console.WriteLine(" ERROR FOR {0} (< {1})", msg, last);
                //}
                //if (messageIds.ContainsKey(msg))
                //{
                //    Console.WriteLine(" ERROR FOR {0} previous {1}, new {2}", msg, messageIds[msg], envelope.MessageId);
                //}
                messageIds[msg] = envelope.MessageId;
                consumer.Ack(envelope);
                ack++;
                last = Math.Max(msg, last);

                DisplayMessageReceived();
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR] {0}", e.Message);
                exc++;
            }
        }

        private static void HandleCardIn(IRabbitMqConsumer consumer, CardMessage msg, MessageEnvelope envelope)
        {
            try
            {
                cpt++;
                //Console.WriteLine("EGM: {0}, Card IN: {1}", msg.MachineId, msg.CardNum);
                var locationId = msg.MachineId * 10000 + msg.CasinoId;
                if (!egmMessageCount.ContainsKey(locationId))
                    egmMessageCount[locationId] = 0;
                egmMessageCount[locationId]++;

                if (cardInserted.ContainsKey(locationId))
                    Console.WriteLine("    EGM {0} ({1}) already has card inserted...", msg.MachineId, msg.CasinoId);

                cardInserted[locationId] = msg.CardNum;

                consumer.Ack(envelope);
                ack++;

                if (recievedTags.Contains(envelope.DeliveryTag))
                {
                    Console.WriteLine("    Messages already recieved: {0} / not acknowledged: {1}", ++dbl, cpt - ack);
                }
                recievedTags.Add(envelope.DeliveryTag);

                DisplayMessageReceived();
            }
            catch
            {
                exc++;
            }
        }

        private static void HandleCardOut(IRabbitMqConsumer consumer, CardMessage msg, MessageEnvelope envelope)
        {
            try
            {
                cpt++;
                //Console.WriteLine("EGM: {0}, Card OUT: {1}", msg.MachineId, msg.CardNum);
                var locationId = msg.MachineId * 10000 + msg.CasinoId;
                if (!egmMessageCount.ContainsKey(locationId))
                    egmMessageCount[locationId] = 0;
                egmMessageCount[locationId]++;

                if (!cardInserted.ContainsKey(locationId))
                    Console.WriteLine("    No card inserted in EGM {0} ({1}) ...", msg.MachineId, msg.CasinoId);

                cardInserted.Remove(locationId);

                consumer.Ack(envelope);
                ack++;

                if (recievedTags.Contains(envelope.DeliveryTag))
                {
                    Console.WriteLine("    Messages already recieved: {0} / not acknowledged: {1}", ++dbl, cpt - ack);
                }
                recievedTags.Add(envelope.DeliveryTag);

                DisplayMessageReceived();
            }
            catch
            {
                exc++;
            }
        }
    }
}
