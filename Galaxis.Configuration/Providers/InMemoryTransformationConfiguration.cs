using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Galaxis.Configuration.Providers
{
    public class InMemoryTransformationConfiguration
    {
        private static Dictionary<string, string> Data = new Dictionary<string, string>()
        {
            { "TRANSFORM:P", "]__/!P@S5w0r]F0R4tRoN1[!_" },
            { "TRANSFORM:S", "s@1tV4lu3!" }
        };

        internal static IConfiguration CreateConfiguration()
        {
            return new ConfigurationBuilder().AddInMemoryCollection(Data).Build();
        }
    }
}
