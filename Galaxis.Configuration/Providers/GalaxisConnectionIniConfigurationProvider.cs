using Galaxis.Configuration.Transformations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Ini;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Galaxis.Configuration.Providers
{
    internal class GalaxisConnectionIniConfigurationProvider: IniConfigurationProvider, IGalaxisConfigurationProvider
    {

        internal static readonly Dictionary<string, string> IniFileNames = new Dictionary<string, string>() {
            { "MSG_BROKER", "Bus.ini" },
            { "MSG_BROKER_HO", "BusHO.ini" },
            { "DB:GLX", "Database.ini" },
            { "DB:QPONCASH", "QPonCash.ini" },
            { "DB:CASHWALLET", "CashWalletService.ini" },
            { "DB:SLOTEXPORTER", "SlotDataExporter.ini" },
            { "DB:MKTDTM", "MarketingDatamart.ini" },
            { "DB:SLOTDTM", "Datamart.ini" },
            { "DB:AUTH", "AuthenticationService.ini" },
            { "DB:EPAYGATE", "ePayGate.ini" },
            { "DB:AML", "Aml.ini"},
            { "DB:TBL", "Tbl.ini" },
        };

        private static readonly List<string> DataSourceKeys = new List<string>() { "DatabaseConnection:DBTnsAlias", "CONNEXION:DATASOURCE", "CONNECTION:DATASOURCE" };
        private static readonly List<string> UserNameKeys = new List<string>() { "DatabaseConnection:DBUser", "CONNEXION:USER NAME", "CONNECTION:USERNAME", "Connection:Username" };
        private static readonly List<string> PasswordKeys = new List<string>() { "DatabaseConnection:DBPass", "CONNEXION:PASSWORD", "CONNECTION:PASSWORD", "Connection:Password" };
        private static readonly List<string> SchemaKeys = new List<string>() { "CONNEXION:BIBLIOTHEQUEBASE", "CONNECTION:SCHEMA" };

        private readonly string _prefix;

        private readonly TransformationManager _transformationManager;

        public GalaxisConnectionIniConfigurationProvider(IniConfigurationSource source, string key) : base(source)
        {
            source.Path = IniFileNames[key];
            _prefix = key;

            _transformationManager = new TransformationManager(null);
        }

        IDictionary<string, string> IGalaxisConfigurationProvider.ConcatDataTo(IDictionary<string, string> data)
        {
            var acceptedKeys = DataSourceKeys.Concat(UserNameKeys).Concat(PasswordKeys).Concat(SchemaKeys).Append("PROVIDER");
            var filteredData = Data.Where(pair => acceptedKeys.Contains(pair.Key));
            return data.Concat(filteredData.Select(pair => new KeyValuePair<string, string>(TransformKey(pair.Key), TransformValue(pair)))).ToDictionary(k => k.Key, v => v.Value, StringComparer.InvariantCultureIgnoreCase);
        }

        private string TransformValue(KeyValuePair<string, string> pair)
        {
            if (PasswordKeys.Contains(pair.Key))
            {
                return _transformationManager.Undo(pair.Value);
            }
            return pair.Value;
        }

        private string TransformKey(string key)
        {
            if (PasswordKeys.Contains(key))
            {
                return $"{_prefix}:PASSWORD";
            }

            if (UserNameKeys.Contains(key))
            {
                return $"{_prefix}:USERNAME";
            }

            if (DataSourceKeys.Contains(key))
            {
                return $"{_prefix}:DATASOURCE";
            }

            if (SchemaKeys.Contains(key))
            {
                return $"{_prefix}:SCHEMA";
            }

            if (key.EndsWith(":PROVIDER"))
            {
                return $"{_prefix}:PROVIDER";
            }

            return $"{_prefix}:{key}";

        }
    }
}
