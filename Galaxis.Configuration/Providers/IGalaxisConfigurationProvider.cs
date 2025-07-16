using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Galaxis.Configuration.Providers
{
    internal interface IGalaxisConfigurationProvider : IConfigurationProvider
    {
        internal IDictionary<string, string> ConcatDataTo(IDictionary<string, string> data);
    }
}
