using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Galaxis.Configuration
{
    public class GalaxisConfigurationSource : IConfigurationSource
    {
        internal IConfiguration Configuration { get; private set; }

        internal bool Optional { get; private set; }

        internal IEnumerable<string> Entries { get; private set; }

        public GalaxisConfigurationSource(IConfiguration configuration, bool optional, IEnumerable<string> entries)
        {
            Configuration = configuration;
            Entries = entries;
            Optional = optional;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new GalaxisConfigurationProvider(this);
        }
    }
}
