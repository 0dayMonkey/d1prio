using Microsoft.Extensions.Configuration.Ini;
using Microsoft.Extensions.Configuration.Xml;
using Microsoft.Extensions.FileProviders;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Galaxis.Configuration.Tools;
using Galaxis.Configuration.Providers;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System;
using Microsoft.Extensions.Configuration.Json;

namespace Galaxis.Configuration
{
    internal class ConfigurationProviderFactory
    {
        internal IEnumerable<IGalaxisConfigurationProvider> CreateProvider(string entry, string path, IDictionary<string, IGalaxisConfigurationProvider> providers)
        {
            List<IGalaxisConfigurationProvider> createdProviders = new List<IGalaxisConfigurationProvider>();
            IGalaxisConfigurationProvider provider;

            var keys = entry.Split(":");
            if (keys.Length == 0)
            {
                return null;
            }

            switch (keys[0])
            {
                case "SERVICE":
                    if (!providers.ContainsKey("SERVICE"))
                    {
                        provider = CreateAddressBookProvider(Path.Combine(path, "AddressBook.xml"));
                        createdProviders.Add(provider);
                        providers.Add("SERVICE", createdProviders[0]); 
                    }
                    break;

                case "DB":
                    if (keys.Length <= 1)
                    {
                        return null;
                    }
                    if (!GalaxisConnectionIniConfigurationProvider.IniFileNames.ContainsKey($"DB:{keys[1]}"))
                    {
                        provider = providers.Values.FirstOrDefault(p => p is SystmConnectionJsonConfigurationProvider);
                        if (provider == null)
                        {
                            provider = new SystmConnectionJsonConfigurationProvider(new JsonConfigurationSource() { FileProvider = new PhysicalFileProvider(path), Path = "systm-db-configuration.json" });
                            createdProviders.Add(provider);
                        }
                        providers.Add($"DB:{keys[1]}", provider);
                    }
                    else
                    {
                        provider = CreateConnectionIniProvider(path, $"DB:{keys[1]}");
                        createdProviders.Add(provider);
                        providers.Add($"DB:{keys[1]}", provider);
                    }
                    break;

                case "CFG":
                    if (!providers.ContainsKey("CFG"))
                    {
                        if (keys.Length <= 1)
                        {
                            return null;
                        }
                        provider = CreateIniProvider(path, "Database.ini", new Dictionary<string, string> {
                            { "CONNEXION:CASINOID", "CFG:CASINO_ID" },
                            { "GENERAL:SOCIETE", "CFG:COMPANY_CODE" },
                            { "GENERAL:ETABLISSEMENT", "CFG:CASINO_CODE" },
                        });
                        createdProviders.Add(provider);
                        providers.Add("CFG", createdProviders[0]);
                    }
                    break;

                case "MSG_BROKER":
                    if (!providers.ContainsKey("SERVICE"))
                    {
                        provider = CreateAddressBookProvider(Path.Combine(path, "AddressBook.xml"));
                        createdProviders.Add(provider);
                        providers.Add("SERVICE", provider);
                    }
                    provider = CreateConnectionIniProvider(path, keys[0]);
                    createdProviders.Add(provider);
                    providers.Add("MSG_BROKER", provider);
                    break;

                case "MSG_BROKER_HO":
                    if (!providers.ContainsKey("SERVICE"))
                    {
                        provider = CreateAddressBookProvider(Path.Combine(path, "AddressBook.xml"));
                        createdProviders.Add(provider);
                        providers.Add("SERVICE", provider);
                    }
                    provider = CreateConnectionIniProvider(path, keys[0]);
                    createdProviders.Add(provider);
                    providers.Add("MSG_BROKER_HO", provider);
                    break;

            }
            return createdProviders;
        }

        internal IDictionary<string, IGalaxisConfigurationProvider> CreateProviders(IEnumerable<string> entries, string path)
        {
            var providers = new Dictionary<string, IGalaxisConfigurationProvider>();

            if (entries != null)
            {
                foreach (var entry in entries)
                {
                    CreateProvider(entry, path, providers);
                }
            }
            return providers;
        }

        private IGalaxisConfigurationProvider CreateAddressBookProvider(string addressBookPath)
        {
            var stream = new MemoryStream();
            var readStream = new FileStream(addressBookPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, bufferSize: 1, FileOptions.SequentialScan);

            var xDoc = XDocument.Load(readStream);
            xDoc.StripNamespace();
            xDoc.Save(stream);
            stream.Position = 0;
            return new GalaxisAddressBookConfigurationProvider(new XmlStreamConfigurationSource() { Stream = stream });
        }

        private IGalaxisConfigurationProvider CreateConnectionIniProvider(string path, string key)
        {
            return new GalaxisConnectionIniConfigurationProvider(new IniConfigurationSource() { FileProvider = new PhysicalFileProvider(path) }, key);
        }

        private IGalaxisConfigurationProvider CreateIniProvider(string path, string fileName, IDictionary<string, string> keys)
        {
            return new GalaxisIniConfigurationProvider(new IniConfigurationSource() { FileProvider = new PhysicalFileProvider(path), Path = fileName }, keys);
        }
    }
}
