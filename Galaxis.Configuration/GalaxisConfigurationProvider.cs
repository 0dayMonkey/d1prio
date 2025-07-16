using Galaxis.Configuration.Providers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Galaxis.Configuration
{
    public class GalaxisConfigurationProvider : ConfigurationProvider
    {
        private readonly GalaxisConfigurationSource _source;
        private readonly ConfigurationProviderFactory _configurationProviderFactory;
        private readonly string _configurationPath;

        private IDictionary<string, IGalaxisConfigurationProvider> _providers;

        public GalaxisConfigurationProvider(GalaxisConfigurationSource source)
        {
            _source = source;          
            if (_source.Configuration["galaxis_home"] == null && _source.Optional)
                return;

            _configurationPath = Path.Combine(_source.Configuration["galaxis_home"], "Program", "Common");
            _configurationProviderFactory = new ConfigurationProviderFactory();
            _providers = _configurationProviderFactory.CreateProviders(_source.Entries, _configurationPath);
            Data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        public override void Load()
        {
            if (_configurationProviderFactory == null && _source.Optional)
            {
                return;
            }
            foreach (var provider in _providers.Values)
            {
                LoadProvider(provider);
            }
        }

        public override IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string parentPath)
        {
            if (_configurationProviderFactory == null && _source.Optional)
            {
                return null;
            }
            if (parentPath != null && !_providers.Keys.Any(k => parentPath.StartsWith(k)))
            {
                var providers = _configurationProviderFactory.CreateProvider(parentPath, _configurationPath, _providers);
                foreach (var provider in providers) 
                { 
                    LoadProvider(provider); 
                }
            }
            return base.GetChildKeys(earlierKeys, parentPath);
        }

        public override bool TryGet(string key, out string value)
        {
            if (_configurationProviderFactory == null && _source.Optional)
            {
                value = null;
                return false;
            }
            if (!_providers.Keys.Any(k => key.StartsWith(k)))
            {
                var providers = _configurationProviderFactory.CreateProvider(key, _configurationPath, _providers);
                foreach (var provider in providers)
                {
                    LoadProvider(provider);
                }
            }
            return base.TryGet(key, out value);
        }

        private void LoadProvider(IGalaxisConfigurationProvider provider)
        {
            if (provider == null)
            {
                return;
            }
            provider.Load();
            Data = provider.ConcatDataTo(Data);
        }
    }
}
