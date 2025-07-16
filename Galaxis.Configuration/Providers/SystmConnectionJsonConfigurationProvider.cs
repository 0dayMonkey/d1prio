using Galaxis.Configuration.Transformations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Galaxis.Configuration.Providers
{
    public class SystmConnectionJsonConfigurationProvider : JsonConfigurationProvider, IGalaxisConfigurationProvider
    {
        private readonly TransformationManager _transformationManager;

        public SystmConnectionJsonConfigurationProvider(JsonConfigurationSource source) : base (source)
        {
            _transformationManager = new TransformationManager(null);
        }

        IDictionary<string, string> IGalaxisConfigurationProvider.ConcatDataTo(IDictionary<string, string> data)
        {
            var transformedData = Data.Select(pair => new KeyValuePair<string, string>(
                pair.Key, 
                pair.Key.EndsWith(":PASSWORD") ? _transformationManager.Undo(pair.Value) : pair.Value));
            return data.Concat(transformedData).ToDictionary(k => k.Key, v => v.Value, StringComparer.InvariantCultureIgnoreCase);
        }
    }
}
