using Microsoft.Extensions.Configuration.Xml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Galaxis.Configuration.Providers
{
    internal class GalaxisAddressBookConfigurationProvider : XmlStreamConfigurationProvider, IGalaxisConfigurationProvider
    {
        internal GalaxisAddressBookConfigurationProvider(XmlStreamConfigurationSource source) : base(source)
        {
        }

        IDictionary<string, string> IGalaxisConfigurationProvider.ConcatDataTo(IDictionary<string, string> data)
        {
            var filteredData = Data.Where(pair => pair.Key.EndsWith(":address") || pair.Key.EndsWith(":port"))
                .Select(pair => new KeyValuePair<string, string>(FormatKey(pair.Key), pair.Value));
            return data.Concat(filteredData).ToDictionary(k => k.Key, v => v.Value, StringComparer.InvariantCultureIgnoreCase);
        }

        private string FormatKey(string key)
        {
            if (key.Equals("services:service:BusService:address"))
            {
                return "MSG_BROKER:address";
            }
            else if (key.Equals("services:service:BusServiceHO:address"))
            {
                return "MSG_BROKER_HO:address";
            }
            return key.Replace("servers:server", "SERVICE").Replace("services:service", "SERVICE");
        }
    }
}
