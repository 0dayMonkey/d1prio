using RabbitMQ.Client;
using System;

namespace RabbitMq.Messaging
{
    public class RabbitMqConfiguration
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string VirtualHost { get; set; } = "/";
        public TimeSpan NetworkRecoveryInterval { get; set; } = TimeSpan.FromSeconds(10);
        public bool AutomaticRecoveryEnabled { get; set; } = true;
        public TimeSpan RequestedHeartbeat { get; set; } = TimeSpan.FromSeconds(5);
        public SslOption SslOptions { get; set; } = new SslOption { Enabled = false };
        public Uri? Uri { get; set; }
    }
}
