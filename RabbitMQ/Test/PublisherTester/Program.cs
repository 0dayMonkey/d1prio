using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PublisherTester
{
    class Program
    {
        private static List<int> _machineIds = new List<int>();

        const int ThreadNum = 4;

        static void Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args).Build();

            var logger = host.Services.GetRequiredService<ILoggerFactory>();

            Console.WriteLine("PUBLISHER");
            for (int i = 1; i <= 500; i++)
            {
                _machineIds.Add(i);
            }

            var threads = new List<PublisherThread>();
            for (int i = 1; i <= ThreadNum; i++)
            {
                threads.Add(new PublisherThread(_machineIds.ToList(), i, logger));
            }

            var cmd = Console.ReadLine().ToLower();
            while (!cmd.Equals("q"))
            {
                switch (cmd)
                {
                    case "e":
                        foreach (var t in threads)
                            t.PopulateQueue(50);
                        break;

                        case "i":
                            foreach (var t in threads)
                                t.DisplayInfo();
                            break;

                        case "r":
                            foreach (var t in threads)
                                t.ClearInfo();
                            break;
                }
                cmd = Console.ReadLine().ToLower();
            }
            foreach (var t in threads)
                t.Running = false;
        }

    }
}
