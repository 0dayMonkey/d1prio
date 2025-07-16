using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Galaxis.Configuration
{
    public static class GalaxisConfigurationExtensions
    {

        public static IConfigurationBuilder AddGalaxis(this IConfigurationBuilder builder, IEnumerable<string> entries = null, bool optional = false)
            => builder.Add(new GalaxisConfigurationSource(builder.Build(), optional, entries));
    }
}
