using Galaxis.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NotificationService.Infrastructure.Messages;
using NotificationService.Infrastructure.RabbitMQ;
using NotificationService.Infrastructure.Workflows;
using Serilog;
using System;

namespace NotificationService.Service
{
    class Program
    {
        private static IConfiguration? _configuration;

        static void Main(string[] args)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .AddGalaxis();

            _configuration = configurationBuilder.Build();

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithThreadId()
                .WriteTo.Console()
                .ReadFrom.Configuration(_configuration)
                .CreateLogger();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton(_configuration!);
                IWorkflowInitializer workflow;

                if (_configuration == null)
                {
                    throw new InvalidOperationException("Configuration not initiaized");
                }

                var serviceMode = _configuration.GetValue<string>("ServiceMode");

                Log.Logger.Information($"Starting mode is [{serviceMode}]");

                if (serviceMode?.Equals("Central") ?? false)
                {
                    workflow = new GlobalWorkflowInitializer(_configuration);
                }
                else if (serviceMode?.Equals("LocalMultiSite") ?? false)
                {
                    workflow = new LocalWorkflowInitializer(_configuration, true);
                }
                else if (serviceMode?.Equals("Local") ?? false)
                {
                    workflow = new LocalWorkflowInitializer(_configuration, false);
                }
                else
                {
                    throw new InvalidOperationException("Unknow mode, Service mode should have one of this value (Central, LocalMultiSite, Local)");
                }
                services.AddSingleton(workflow);
                services.AddSingleton<IMessageConsumer, RabbitMQProxy>();

                workflow.AddRabbitMQHandlers(services);
                workflow.AddMessageManagers(services);

                services.AddHostedService<InitNotificationService>();
            })
            .ConfigureLogging(logger =>
            {
                logger.ClearProviders();
                logger.AddSerilog(dispose: true);
            });
    }
}
