using Galaxis.Configuration.Transformations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Ini;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Galaxis.Configuration.Providers
{
    internal class GalaxisIniConfigurationProvider: IniConfigurationProvider, IGalaxisConfigurationProvider
    {
        private readonly IDictionary<string, string> _keys;

        public GalaxisIniConfigurationProvider(IniConfigurationSource source, IDictionary<string, string> keys) : base(source)
        {
            _keys = keys;
        }

        IDictionary<string, string> IGalaxisConfigurationProvider.ConcatDataTo(IDictionary<string, string> data)
        {
            var filteredData = Data.Where(pair => _keys.Keys.Contains(pair.Key));
            return data.Concat(filteredData.Select(pair => new KeyValuePair<string, string>(_keys[pair.Key], pair.Value)))
                .ToDictionary(k => k.Key, v => v.Value, StringComparer.InvariantCultureIgnoreCase);
        }

    }
}
